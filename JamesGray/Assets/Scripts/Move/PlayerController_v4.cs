using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class PlayerController_v4 : MonoBehaviour
{
    public UnityEvent onAction, endAction;
    public Tilemap tileMap;    //타일맵

    [Range(0.001f, 5f)]
    public float speed;

    public VirtualKeyPad keyPad;    //가상 키패드

    Vector3Int currentCell;
    Vector3 dirVec;
    Animator animator;
    GameObject scanObject, tempScanObj;
    bool isOnAction, isOnFreeze, isWalk;

    Vector3 targetPos;
    Rigidbody2D rigid;
    void Awake() 
    {
        animator = GetComponent<Animator>();

        currentCell = tileMap.WorldToCell(this.transform.position); //얕은 복사, 클래스를 단순히 복사하면 참조만 함.

        dirVec = Vector3.down;  //기본적으로 아래를 보고있으므로...

        isOnAction = false; isOnFreeze = false; isWalk = false;
        tempScanObj = null;
        scanObject = null;

        rigid = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        Vector3 preDir = dirVec;    //캐릭터 이동시 방향을 전환해도 애니메이션이 바뀌지 않는 문제가있음(백스텝 밟음) 애니메이션 업데이트를 강제하기위한 변수

        if(isOnAction) 
        {
            animator.SetBool("isWalk", false);
            return;
        }

        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dirVec, 1f, LayerMask.GetMask("Object"));
        if(!ReferenceEquals(rayHit.collider, null))
        {
            tempScanObj = rayHit.collider.gameObject;
        }
        else 
        {
            tempScanObj = null;
        }

        if(!isWalk)
        {
            if((Input.GetKeyDown(KeyCode.E) || keyPad.ACTION) && !ReferenceEquals(tempScanObj, null) && tempScanObj.CompareTag("NPC"))
            {
                scanObject = tempScanObj;
                OnAction();
            }
            else if((Input.GetKey(KeyCode.A) || keyPad.LEFT) || Input.GetKey(KeyCode.LeftArrow))
            {
                dirVec = Vector3.left;
                if(ReferenceEquals(Physics2D.Raycast(transform.position, dirVec, 1f, LayerMask.GetMask("Object")).collider, null))
                { 
                    isWalk = true;
                    targetPos = transform.position;
                    targetPos.x -= 1;
                }
            }
            else if((Input.GetKey(KeyCode.D) || keyPad.RIGHT) || Input.GetKey(KeyCode.RightArrow))
            {   
                dirVec = Vector3.right;
                if(ReferenceEquals(Physics2D.Raycast(transform.position, dirVec, 1f, LayerMask.GetMask("Object")).collider, null))
                { 
                    isWalk = true;
                    targetPos = transform.position;
                    targetPos.x += 1; 
                }
            }
            else if((Input.GetKey(KeyCode.W) || keyPad.UP) || Input.GetKey(KeyCode.UpArrow))
            {
                dirVec = Vector3.up;
                if(ReferenceEquals(Physics2D.Raycast(transform.position, dirVec, 1f, LayerMask.GetMask("Object")).collider, null))
                { 
                    isWalk = true;
                    targetPos = transform.position;
                    targetPos.y += 1;
                }
            }
            else if((Input.GetKey(KeyCode.S) || keyPad.DOWN) || Input.GetKey(KeyCode.DownArrow))
            {
                dirVec = Vector3.down;
                if(ReferenceEquals(Physics2D.Raycast(transform.position, dirVec, 1f, LayerMask.GetMask("Object")).collider, null))
                { 
                    isWalk = true;
                    targetPos = transform.position;
                    targetPos.y -= 1;
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
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if(transform.position == targetPos)
            {
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
