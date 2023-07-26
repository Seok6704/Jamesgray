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

    //public VirtualKeyPad keyPad;    //가상 키패드

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
        //Vector3 preDir = new Vector3(animator.GetInteger("Horizontal"), animator.GetInteger("Vertical"), 0);

        if(Input.GetKey(KeyCode.A) && !isWalk)
        {
            isWalk = true;
            targetPos = transform.position;
            targetPos.x -= 1;
            dirVec = Vector3.left;
        }
        else if(Input.GetKey(KeyCode.D) && !isWalk)
        {
            isWalk = true;
            targetPos = transform.position;
            targetPos.x += 1;
            dirVec = Vector3.right;
        }
        else if(Input.GetKey(KeyCode.W) && !isWalk)
        {
            isWalk = true;
            targetPos = transform.position;
            targetPos.y += 1;
            dirVec = Vector3.up;
        }
        else if(Input.GetKey(KeyCode.S) && !isWalk)
        {
            isWalk = true;
            targetPos = transform.position;
            targetPos.y -= 1;
            dirVec = Vector3.down;
        }

        animator.SetInteger("Horizontal", (int)dirVec.x);
        animator.SetInteger("Vertical", (int)dirVec.y);
        if(animator.GetBool("isWalk") != isWalk)
        {
            animator.SetBool("isWalk", isWalk);
            if(isWalk) animator.SetTrigger("Walk");
        }

        /*if(dirVec != preDir)
        {
            animator.SetInteger("Horizontal", (int)dirVec.x);
            animator.SetInteger("Vertical", (int)dirVec.y);
        }
        if(animator.GetBool("isWalk") != isWalk)
        {
            if(isWalk) animator.SetTrigger("Walk");
            animator.SetBool("isWalk", isWalk);
        }*/
        
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

}
