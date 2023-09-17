using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class PlayerController_v3 : MonoBehaviour
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
    bool isOnAction, isOnFreeze;

    Rigidbody2D rigid;
    //RaycastHit2D rayHit;

    SerialCOM serial; //유선 연결 패드

    private void Awake() 
    {
        animator = GetComponent<Animator>();

        currentCell = tileMap.WorldToCell(this.transform.position); //얕은 복사, 클래스를 단순히 복사하면 참조만 함.
        co = null;
        dirVec = Vector3.down;  //기본적으로 아래를 보고있으므로...

        isOnAction = false; isOnFreeze = false;
        tempScanObj = null;
        scanObject = null;

        rigid = GetComponent<Rigidbody2D>();

        serial = new SerialCOM(9600, 8);    //9600hz 11번 포트
    }

    /*private void FixedUpdate() 
    {
        currentCell = tileMap.WorldToCell(this.transform.position); //주기적으로 위치 업데이트
    }*/

    private void Update() 
    {
        int h = (int)dirVec.x, v = (int)dirVec.y;
        Vector3Int nextCell = currentCell;

        serial.GetInput();  //SerialCom 객체는 함수로 호출해야 값을 가져온다.

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


        if(co == null && !isOnFreeze)
        {
            //h = 0; 이곳에 있으면 달리는 도중 Idle로 전환되는 문제 발견
            //v = 0;
            if((Input.GetKeyDown(KeyCode.E) || keyPad.ACTION) && tempScanObj != null && tempScanObj.CompareTag("NPC"))
            {
                co = StartCoroutine(WaitCoroutine());
                scanObject = tempScanObj;
                OnAction();
            }
            else if(Input.GetKey(KeyCode.W) || keyPad.UP || Input.GetKey(KeyCode.UpArrow) || serial.UP)    
            {
                dirVec = Vector3.up;
                nextCell.y += 1;
                co = StartCoroutine(MovePlayer(nextCell));
            }
            else if(Input.GetKey(KeyCode.S) || keyPad.DOWN || Input.GetKey(KeyCode.DownArrow) || serial.DOWN)
            {
                dirVec = Vector3.down;
                nextCell.y -= 1;
                co = StartCoroutine(MovePlayer(nextCell));
            } 
            else if(Input.GetKey(KeyCode.A) || keyPad.LEFT || Input.GetKey(KeyCode.LeftArrow) || serial.LEFT)
            {
                dirVec = Vector3.left;
                nextCell.x -= 1;
                co = StartCoroutine(MovePlayer(nextCell));
            } 
            else if(Input.GetKey(KeyCode.D) || keyPad.RIGHT || Input.GetKey(KeyCode.RightArrow) || serial.RIGHT)
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
        if(isOnAction)
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

    public void ChangeisOn() // isOnFreeze, isOnAction 값을 조정하기 위한 함수(이석현 작성)
    {
        isOnFreeze = !isOnFreeze;
        isOnAction = !isOnAction;
    }
}
