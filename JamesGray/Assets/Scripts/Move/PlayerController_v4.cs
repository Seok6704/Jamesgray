using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class PlayerController_v4 : MonoBehaviour
{
    public UnityEvent onAction, endAction;
    public Tilemap tileMap;    //타일맵

    [Range(0.001f, 10f)]
    public float speed;        //이동속도

    public VirtualKeyPad keyPad;    //가상 키패드

    Vector3Int currentCell;
    Vector3 dirVec;     //플레이어가 바라보는 방향
    Animator animator;
    GameObject scanObject, tempScanObj;
    bool isOnAction, isOnFreeze, isWalk;

    Vector3 targetPos;  //이동할 좌표
    Vector2 moveLength; //플레이어 이동 거리를 위한 값, 타일에서 다른 타일로 이동하기 위해 필요한 거리 (x,y), (타일 반지름 * 2) + 타일간 거리로 계산 //거리는 1이기때문에 필요없지만 추후 변동사항을 위한 거리 계산
    Rigidbody2D rigid;

    SerialCOM serial;   //유선 연결 패드
    void Awake() 
    {
        animator = GetComponent<Animator>();

        currentCell = tileMap.WorldToCell(this.transform.position);
        
        Vector3 length = tileMap.cellSize + tileMap.cellGap;    //타일 반지름 * 2 + 타일간 거리 = 타일 중심에서 인접 타일 중심간 거리
        moveLength = new Vector2(length.x, length.y);

        dirVec = Vector3.down;  //기본적으로 아래를 보고있으므로...

        isOnAction = false; isOnFreeze = false; isWalk = false;
        tempScanObj = null;
        scanObject = null;

        rigid = GetComponent<Rigidbody2D>();

        serial = new SerialCOM(9600, 11);    //9600hz 11번 포트
    }

    void Update() 
    {
        RaycastHit2D rayHit;
        Vector3 preDir = dirVec;    //캐릭터 이동시 방향을 전환해도 애니메이션이 바뀌지 않는 문제가있음(백스텝 밟음) 애니메이션 업데이트를 강제하기위한 변수

        if(isOnAction)              //대화 및 액션 상태일때, 이동 애니메이션 중지 및 입력받기 중단
        {
            animator.SetBool("isWalk", false);
            return;
        }

        if(!isWalk)                 //이동중이 아닐때만 입력을 받음
        {
            rayHit = Physics2D.Raycast(transform.position, dirVec, 1f, LayerMask.GetMask("Object"));   //전방에 오브젝트 감지
            if(!ReferenceEquals(rayHit.collider, null))
            {
                tempScanObj = rayHit.collider.gameObject;   //전방에 오브젝트가 있다면, 임시로 객체 기억
            }
            else 
            {
                tempScanObj = null;
            }

            if((Input.GetKeyDown(KeyCode.E) || keyPad.ACTION) && !ReferenceEquals(tempScanObj, null) && tempScanObj.CompareTag("NPC")) //액션 명령키가 눌렸고, 오브젝트가 NPC일경우 동작
            {
                scanObject = tempScanObj;   
                OnAction();
            }
            else if(Input.GetKey(KeyCode.A) || keyPad.LEFT || Input.GetKey(KeyCode.LeftArrow) || serial.LEFT)
            {
                dirVec = Vector3.left;
                if(ReferenceEquals(Physics2D.Raycast(transform.position, dirVec, 1f, LayerMask.GetMask("Object")).collider, null))
                { 
                    isWalk = true;
                    targetPos = transform.position;
                    targetPos.x -= moveLength.x;
                }
            }
            else if(Input.GetKey(KeyCode.D) || keyPad.RIGHT || Input.GetKey(KeyCode.RightArrow) || serial.RIGHT)
            {   
                dirVec = Vector3.right;
                if(ReferenceEquals(Physics2D.Raycast(transform.position, dirVec, 1f, LayerMask.GetMask("Object")).collider, null))
                { 
                    isWalk = true;
                    targetPos = transform.position;
                    targetPos.x += moveLength.x; 
                }
            }
            else if(Input.GetKey(KeyCode.W) || keyPad.UP || Input.GetKey(KeyCode.UpArrow) || serial.UP)
            {
                dirVec = Vector3.up;
                if(ReferenceEquals(Physics2D.Raycast(transform.position, dirVec, 1f, LayerMask.GetMask("Object")).collider, null))
                { 
                    isWalk = true;
                    targetPos = transform.position;
                    targetPos.y += moveLength.y;
                }
            }
            else if(Input.GetKey(KeyCode.S) || keyPad.DOWN || Input.GetKey(KeyCode.DownArrow) || serial.DOWN)
            {
                dirVec = Vector3.down;
                if(ReferenceEquals(Physics2D.Raycast(transform.position, dirVec, 1f, LayerMask.GetMask("Object")).collider, null))
                { 
                    isWalk = true;
                    targetPos = transform.position;
                    targetPos.y -= moveLength.y;
                }
            }
        }

        animator.SetInteger("Horizontal", (int)dirVec.x);
        animator.SetInteger("Vertical", (int)dirVec.y);
        if(animator.GetBool("isWalk") != isWalk || dirVec != preDir)
        {
            animator.SetBool("isWalk", isWalk);
            if(isWalk) animator.SetTrigger("Walk");
        }
    }

    private void FixedUpdate() 
    {
        if(isWalk)
        {
            //transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);    //기기마다 차이가 있다고 해서 델타타임을 곱해주라는 말이 있음
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed); //deltaTime 없이하기

            if(transform.position == targetPos)
            {
                transform.position = targetPos;
                isWalk = false;
            }
        }
    }

    void OnAction()
    {
        scanObject.GetComponent<NPCManager>().OnAction();   //모든 Object layer는 NPCManager 스크립트를 가지고 있어야 정상 작동... 
        onAction.Invoke();
        isOnAction = true;
    }

    public void EndAction()
    {
        scanObject.GetComponent<NPCManager>().EndAction();
        //StopCoroutine(co);
        //co = null;
        isOnAction = false;
        scanObject = null;
        endAction.Invoke();
    }

    public void ToggleFreeze()    //플레이어 멈추기 입력을 받기를 멈춘다.
    {
        isOnFreeze = !isOnFreeze;
    }
}
