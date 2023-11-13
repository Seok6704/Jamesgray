using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class PlayerController_v3 : MonoBehaviour, IBLE
{
    public UnityEvent onAction, endAction;
    public Tilemap tileMap;    //타일맵

    [Range(0.001f, 10f)]
    public float speed;

    public VirtualKeyPad keyPad;    //가상 키패드

    Vector3Int currentCell;
    Vector3 dirVec;

    Coroutine co;
    Animator animator;
    GameObject scanObject, tempScanObj;
    bool isOnAction, isOnFreeze, isCamera;

    Rigidbody2D rigid;
    //RaycastHit2D rayHit;

    //SerialCOM serial; //유선 연결 패드, 최종은 블루투스이기때문에 사용X


    ///BLE변수들
    private BluetoothController BLE = null;

    public bool BLE_Connection;
    public bool BLE_Ready2Read;
    public bool BLE_Scan;
    string BLEinput;
    ///BLE end

    private void Awake() 
    {
        animator = GetComponent<Animator>();

        currentCell = tileMap.WorldToCell(this.transform.position); //얕은 복사, 클래스를 단순히 복사하면 참조만 함.
        co = null;
        dirVec = Vector3.down;  //기본적으로 아래를 보고있으므로...

        isOnAction = false; isOnFreeze = false; isCamera = false;
        tempScanObj = null;
        scanObject = null;

        rigid = GetComponent<Rigidbody2D>();

        //serial = new SerialCOM(9600, 8);    //9600hz 11번 포트, 시리얼 사용하지않기
        //serial = SerialCOM.getInstance();   //인스턴스 가져오기
    }

    /*private void FixedUpdate() 
    {
        currentCell = tileMap.WorldToCell(this.transform.position); //주기적으로 위치 업데이트
    }*/

    private void Update() 
    {
        int h = (int)dirVec.x, v = (int)dirVec.y;
        Vector3Int nextCell = currentCell;


        //serial.GetInput();  //SerialCom 객체는 함수로 호출해야 값을 가져온다.
        Read(2);

        //Debug.DrawRay(transform.position, dirVec * 0.7f, new Color(0,1,0)); // 게임 뷰에서는 보이지 않지만 플레이 버튼 누르고 씬뷰로 전환하면 보임!
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dirVec, 1f, LayerMask.GetMask("Object"));
        if(!ReferenceEquals(rayHit.collider, null))
        {
            tempScanObj = rayHit.collider.gameObject;
        }
        else 
        {
            tempScanObj = null;
        }


        if(co == null && !isOnFreeze && !isCamera)
        {
            //h = 0; 이곳에 있으면 달리는 도중 Idle로 전환되는 문제 발견
            //v = 0;
            if((Input.GetKeyDown(KeyCode.E) || keyPad.ACTION) && tempScanObj != null && tempScanObj.CompareTag("NPC"))
            {
                co = StartCoroutine(WaitCoroutine());
                scanObject = tempScanObj;
                OnAction();
            }
            else if(Input.GetKey(KeyCode.W) || keyPad.UP || Input.GetKey(KeyCode.UpArrow) || BLEinput == "w")    
            {
                dirVec = Vector3.up;
                nextCell.y += 1;
                co = StartCoroutine(MovePlayer(nextCell));
            }
            else if(Input.GetKey(KeyCode.S) || keyPad.DOWN || Input.GetKey(KeyCode.DownArrow) || BLEinput == "s")
            {
                dirVec = Vector3.down;
                nextCell.y -= 1;
                co = StartCoroutine(MovePlayer(nextCell));
            } 
            else if(Input.GetKey(KeyCode.A) || keyPad.LEFT || Input.GetKey(KeyCode.LeftArrow) || BLEinput == "a")
            {
                dirVec = Vector3.left;
                nextCell.x -= 1;
                co = StartCoroutine(MovePlayer(nextCell));
            } 
            else if(Input.GetKey(KeyCode.D) || keyPad.RIGHT || Input.GetKey(KeyCode.RightArrow) || BLEinput == "d")
            {
                dirVec = Vector3.right;
                nextCell.x += 1;
                co = StartCoroutine(MovePlayer(nextCell));
            }
            else
            {
                h = 0;
                v = 0;
            }
        }
        if(isOnAction || isOnFreeze || isCamera)
        {
            h = 0; v = 0;
        }

        if(animator.GetInteger("hAxisRaw") != h)
        {
            animator.SetInteger("hAxisRaw", h);
            animator.SetBool("isWalk", true);
        }
        else if(animator.GetInteger("vAxisRaw") != v)
        {
            animator.SetInteger("vAxisRaw", v);
            animator.SetBool("isWalk", true);
        }
        else 
        {
            animator.SetBool("isWalk", false);
        }
    }

    IEnumerator MovePlayer(Vector3Int cellPos)
    {
        //Debug.DrawRay(transform.position, dirVec * 0.7f, new Color(1,1,1)); // 게임 뷰에서는 보이지 않지만 플레이 버튼 누르고 씬뷰로 전환하면 보임!     
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dirVec, 1f, LayerMask.GetMask("Object"));   //진행 경로에 오브젝트가 존재하는지 체크
        
        if(!ReferenceEquals(rayHit.collider, null))
        {
            //Debug.Log(rayHit.collider.name + " is Blocking.");
            co = null;
            yield break; 
        }
        
        while(true)
        {
            if(transform.position == tileMap.GetCellCenterWorld(cellPos))
            {
                //if(!checkKey())
                break;
                
            }
            transform.position = Vector3.MoveTowards(transform.position, tileMap.GetCellCenterWorld(cellPos), speed);
            //Vector3 towardsPos = Vector3.MoveTowards(transform.position, tileMap.GetCellCenterWorld(cellPos), speed);
            //rigid.MovePosition(towardsPos);
            yield return null;
        }
        if(checkKey())
        {
            co = StartCoroutine(MovePlayer(new Vector3Int(cellPos.x + (int)dirVec.x, cellPos.y + (int)dirVec.y, cellPos.z)));
        }
        else
        {
            co = null;
        }
        currentCell = cellPos;
    }

    Vector3 CelltoWorld(Vector3Int cellPos) //게임내 타일 중앙의 좌표값을 구해주는 함수  이미 멤버 함수가 존재하여 더이상 필요없음
    {
        Vector3 worldPos = tileMap.CellToWorld(cellPos);
        worldPos.x += 0.5f;
        worldPos.y += 0.5f;
        return worldPos;
    }

    bool checkKey()
    {
        if(isOnFreeze || isOnAction) return false;

        Vector3 current = new Vector3(0,0,0);

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))    
        {
            current = Vector3.up;
        }
        else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            current = Vector3.down;
        } 
        else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            current = Vector3.left;
        } 
        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            current = Vector3.right;
        }

        if(current == dirVec) 
            return true;
        else
            return false;

    }

    IEnumerator WaitCoroutine() //종료되기를 기다리는 무한루프 코루틴
    {
        while(true)
            yield return new WaitForSecondsRealtime(0.1f);
    }

    void OnAction()
    {
        //co = StartCoroutine(WaitCoroutine());
        scanObject.GetComponent<NPCManager>().OnAction();   //모든 Object layer는 NPCManager 스크립트를 가지고 있어야 정상 작동... 
        onAction.Invoke();
        isOnAction = true;
    }

    public void EndAction()
    {
        if(ReferenceEquals(scanObject, null)) return; // 프롤로그 씬 NullReferenceReception에 대응하기 위해 추가된 문장. 23.08.09 추후 문제 발생 시, 해당 부분을 삭제하면 스크립트 동작 자체가 이전과 동일해집니다.
        scanObject.GetComponent<NPCManager>().EndAction(); 
        StopCoroutine(co);
        co = null;
        isOnAction = false;
        scanObject = null;
        endAction.Invoke();
    }

    public void ToggleFreeze()    //플레이어 멈추기 입력을 받기를 멈춘다.
    {
        isOnFreeze = !isOnFreeze;
    }


    /// <summary>
    /// 카메라가 다이얼로그로 인해 바뀔 때, 플레이어 컨트롤러 통제를 위한 함수. 카메라 모듈에서 사용
    /// </summary>
    /// <param name="isOn">활성화 비활성화 여부</param>
    public void SetCamera(bool isOn)
    {
        isCamera = isOn;
    }

    public void ChangeisOn() // isOnFreeze, isOnAction 값을 조정하기 위한 함수(이석현 작성)
    {
        isOnFreeze = !isOnFreeze;
        isOnAction = !isOnAction;
    }

    //////////////////////////////////////////BLE////////////////////////////////////////////
    
    private void Start() 
    {
        //if(SettingManager.useController)
        //{
        //   return;
        //}
        if (Application.platform != RuntimePlatform.Android) return;

        BLE = BluetoothController.GetInstance();
        BLE_Connection = false;
        BLE_Ready2Read = false;
        BLE_Scan = false;
        BLEinput = "n";
        
        BLE.SetReceiverName(gameObject.name);   //JAVA로 부터 메세지를 받을 객체
        Debug.Log(gameObject.name + "is Receiver");
        
        //ScanBLE();
    }

    /// <summary>
    /// 블루투스에게 데이터 요청하기. Characteristic이 여러개인 경우 num으로 지정해야함.
    /// </summary>
    /// <param name="num">Characteristic Index</param>
    public void Read(int num)   //num = 2로 일단 해보기
    {
        //Debug.Log("Call READ");
        if(!BLE_Connection || !BLE_Ready2Read)
        {
            BLEinput = "n";
            return;
        }   
        
        BLE.ReadCharacteristic(num);
    }
    
    /// <summary>
    /// 컨트롤러 데이터가 들어오는 곳
    /// </summary>
    public void OutPutLog(string msg)
    {
        Debug.Log("From Java - " + msg);
        BLEinput = msg;
        //BLE.SendLog("Unity Receive - " + msg);
    }

    public void SetBLEConnection(string msg)
    {
        if(msg == "TRUE")
        {
            BLE_Connection = true;
        }
        else
        {
            BLE_Connection = false;
        }
    }

    public void SetReady2Read(string msg)
    {
        if(msg == "TRUE")
        {
            BLE_Ready2Read = true;
        }
        else
        {
            BLE_Ready2Read = false;
        }
    }
    /// <summary>
    /// 디버그 로그가 들어오는 곳
    /// </summary>
    public void setLog(string msg)
    {
        Debug.Log("From Java Debug Log - " + msg);
    }

    public void ScanBLE()
    {
        BLE_Scan = !BLE_Scan;
        BLE.ScanBLE();
    }
    
}
