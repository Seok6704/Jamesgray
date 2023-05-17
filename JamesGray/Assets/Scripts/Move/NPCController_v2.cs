using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class NPCController_v2 : MonoBehaviour
{
    public UnityEvent destArrival;
    public Tilemap tileMap;
    public float speed;
    
    Rigidbody2D rigid;
    Animator animator;
    Vector3 dirVec;
    Vector3Int currentCell;
    Coroutine co;


    private void Awake() 
    {
        rigid = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
    }

    IEnumerator MovePlayer(Vector3Int cellPos)
    {
        //Debug.DrawRay(transform.position, dirVec * 0.7f, new Color(1,1,1)); // 게임 뷰에서는 보이지 않지만 플레이 버튼 누르고 씬뷰로 전환하면 보임!     
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dirVec, 1f, LayerMask.GetMask("Object"));   //진행 경로에 오브젝트가 존재하는지 체크
        
        if(rayHit.collider != null)
        {
            //Debug.Log(rayHit.collider.name + " is Blocking.");
            co = null;
            yield break; 
        }
        
        while(true)
        {
            if(transform.position == CelltoWorld(cellPos))
            {
                //if(!checkKey())
                    break;

            }
            transform.position = Vector3.MoveTowards(transform.position, CelltoWorld(cellPos), speed);
            yield return null;
        }
        currentCell = cellPos;
        co = null;
    }

    Vector3 CelltoWorld(Vector3Int cellPos) //게임내 타일 중앙의 좌표값을 구해주는 함수
    {
        Vector3 worldPos = tileMap.CellToWorld(cellPos);
        worldPos.x += 0.5f;
        worldPos.y += 0.5f;
        return worldPos;
    }
}
