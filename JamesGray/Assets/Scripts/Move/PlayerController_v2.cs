using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class PlayerController_v2 : MonoBehaviour
{
    public UnityEvent onAction;
    public Tilemap tileMap;    //타일맵

    [Range(0.01f, 0.1f)]
    public float speed;

    Vector3Int currentCell;
    Vector3 dirVec;

    Coroutine co;
    Animator animator;
    GameObject scanObject, tempScanObj;
    bool isOnAction;

    private void Awake() 
    {
        animator = GetComponent<Animator>();

        currentCell = tileMap.WorldToCell(this.transform.position);
        co = null;
        dirVec = Vector3.down;  //기본적으로 아래를 보고있으므로...

        isOnAction = false;
        tempScanObj = null;
        scanObject = null;
    }

    private void Update() 
    {
        int h = (int)dirVec.x, v = (int)dirVec.y;
        Vector3Int nextCell = currentCell;

        //Debug.DrawRay(transform.position, dirVec * 0.7f, new Color(0,1,0)); // 게임 뷰에서는 보이지 않지만 플레이 버튼 누르고 씬뷰로 전환하면 보임!
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dirVec, 1f, LayerMask.GetMask("Object"));
        if(rayHit.collider != null)
        {
            tempScanObj = rayHit.collider.gameObject;
        }
        else 
        {
            tempScanObj = null;
        }


        if(co == null)
        {
            h = 0; 
            v = 0;
            if(Input.GetKeyDown(KeyCode.E) && tempScanObj.CompareTag("NPC"))
            {
                co = StartCoroutine(WaitCoroutine());
                scanObject = tempScanObj;
                OnAction();
            }
            else if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))    
            {
                dirVec = Vector3.up;
                nextCell.y += 1;
                co = StartCoroutine(MovePlayer(nextCell));
            }
            else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                dirVec = Vector3.down;
                nextCell.y -= 1;
                co = StartCoroutine(MovePlayer(nextCell));
            } 
            else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                dirVec = Vector3.left;
                nextCell.x -= 1;
                co = StartCoroutine(MovePlayer(nextCell));
            } 
            else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                dirVec = Vector3.right;
                nextCell.x += 1;
                co = StartCoroutine(MovePlayer(nextCell));
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
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dirVec, 1f, LayerMask.GetMask("Object"));
        
        if(rayHit.collider != null)
        {
            Debug.Log(rayHit.collider.name + " is Blocking.");
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

    bool checkKey()
    {
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
        scanObject.GetComponent<NPCManager>().EndAction();
        StopCoroutine(co);
        co = null;
        isOnAction = false;
        scanObject = null;
    }
}
