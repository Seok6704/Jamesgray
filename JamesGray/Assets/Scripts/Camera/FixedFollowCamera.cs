using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    플레이어를 화면 중앙에 고정시키는 카메라 움직임
*/
public class FixedFollowCamera : MonoBehaviour
{
    [Header("다이얼로그 위치 조정")]
    [Range(-15, 15)]
    public float x;
    [Range(-15, 15)]
    public float y;

    [Header("다이얼로그 위치 이동 속도")]
    [Range(1, 25f)]
    public float dialSpeed;

    bool dialogueOn = false;
    public GameObject player;
    Vector3 dialVec, playerPos;
    Coroutine co;

    private void Awake() 
    {
        playerPos = player.transform.position;
        dialVec = new Vector3(x, y, -10f);
        co = null;    
    }

    private void Update() 
    {
        playerPos = player.transform.position;
        playerPos.z = -10;

        if(co == null)
        {
            transform.position = playerPos;
        }
    }

    IEnumerator MoveDialogue()
    {
        Vector3 dest = player.transform.position + dialVec;
        while(true)
        {
            if(dialogueOn)
            {
                transform.position = Vector3.MoveTowards(transform.position, dest, Time.deltaTime * dialSpeed);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, playerPos, Time.deltaTime * dialSpeed);
            }
            if(transform.position == player.transform.position) break;

            yield return null;
        }
        co = null;
    }

    public void SetFlag()   //이벤트로 호출되면 플레그를 반전, 다이얼로그에서 호출
    {
        dialogueOn = !dialogueOn;
        if(co != null) StopCoroutine(co);

        co = StartCoroutine(MoveDialogue());
    }
}
