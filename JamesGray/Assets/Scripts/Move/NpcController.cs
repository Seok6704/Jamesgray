using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcController : MonoBehaviour
{
    public UnityEvent destArrival;      //npc 목적지 좌표에 도달시 발생하는 이벤트
    public Vector3 destPos;             //npc 목적지 좌표
    public float speed;
    Rigidbody2D rigid;
    Animator animator;
    Vector3 prevPos;                    //이전 좌표 기억
    float temp_speed;
    private void Awake() 
    {
        prevPos = this.transform.position;
        rigid = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        temp_speed = speed;
    }

    private void Update() 
    {
        int v, h;
        Vector3 dir = destPos - this.transform.position;

        dir = Vector3.Normalize(dir);
        
        //rigid.AddForce(dir, ForceMode2D.Force);
        rigid.velocity = dir * speed;

        if(Mathf.RoundToInt(this.transform.position.x) == (int)destPos.x && Mathf.RoundToInt(this.transform.position.y) == (int)destPos.y)
        {
            destArrival.Invoke();       //목적지 도착!
        }

        v = (int)rigid.velocity.y;
        h = (int)rigid.velocity.x;


        if(v != 0)          //균일화
        {
            v = v > 0 ? 1 : -1;
        }
        if(h != 0)
        {
            h = h > 0 ? 1 : -1;
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
        else if(v == 0 && h == 0)
        {
            animator.SetBool("isWalk", false);
        }
    }

    public void ReverseDest()
    {
        destPos = prevPos;
        prevPos.x = Mathf.RoundToInt(this.transform.position.x);
        prevPos.y = Mathf.RoundToInt(this.transform.position.y);
    }

}
