using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public UnityEvent onAction;
    public float PlayerSpeed;
    Rigidbody2D rid2D;
    Animator animator;
    Vector3 dirVec;
    GameObject scanObject;

    float temp_speed;

    private void Awake() 
    {
        rid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        temp_speed = PlayerSpeed;
    }

    void MovePlayer()
    {
        int v, h;
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))     //오른쪽
        {
            rid2D.AddForce(new Vector2(PlayerSpeed, 0), ForceMode2D.Force);
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))    //왼쪽
        {
            rid2D.AddForce(new Vector2(-PlayerSpeed, 0), ForceMode2D.Force);
        }
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))    //위
        {
            rid2D.AddForce(new Vector2(0, PlayerSpeed), ForceMode2D.Force);
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))    //아래
        {
            rid2D.AddForce(new Vector2(0, -PlayerSpeed), ForceMode2D.Force);
        }

        v = (int)rid2D.velocity.y;
        h = (int)rid2D.velocity.x;

        if(v != 0)          //균일화
        {
            v = v > 0 ? 1 : -1;
        }
        if(h != 0)
        {
            h = h > 0 ? 1 : -1;
        }

        if(v == 1 )
        {
            dirVec = Vector3.up;
        }
        else if(v == -1)
        {
            dirVec = Vector3.down;
        }
        else if(h == 1)
        {
            dirVec = Vector3.right;
        }
        else if(h == -1)
        {
            dirVec = Vector3.left;
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
    private void Update() {
        MovePlayer();

        Debug.DrawRay(transform.position, dirVec * 0.7f, new Color(0,1,0)); // 게임 뷰에서는 보이지 않지만 플레이 버튼 누르고 씬뷰로 전환하면 보임!

        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

        if(rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else 
        {
            scanObject = null;
        }

        if(Input.GetKey(KeyCode.E) && scanObject != null)
        {
            scanObject.GetComponent<NPCManager>().OnAction();   //모든 Object layer는 NPCManager 스크립트를 가지고 있어야 정상 작동... 
            OnAction();                                  //좀더 깔끔하게 수정할 필요 있음!
        }
    }

    void OnAction()
    {
        onAction.Invoke();
        PlayerSpeed = 0f;       //이벤트 발생시 플레이어 움직임 멈춤
    }

    public void EndAction()
    {
        scanObject.GetComponent<NPCManager>().EndAction();
        PlayerSpeed = temp_speed;
    }
}
