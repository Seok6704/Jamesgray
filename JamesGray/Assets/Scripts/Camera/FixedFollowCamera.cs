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

    public UnityEngine.Tilemaps.Tilemap border;

    bool dialogueOn = false, flag = false, isForce = false;
    public GameObject player;

    PlayerController_v3 playerCon;
    Vector3 dialVec, playerPos;
    //Coroutine co;

    Vector2 maxSize, minSize; //맵의 최소, 최대 좌표

    private void Awake() 
    {
        //x = 3.5f; //다른 씬 다 수정하기 귀찮으니 3.5 로 고정

        //playerPos = player.transform.position;
        dialVec = new Vector3(x, y, 0);
        //co = null;   

        SetMapSize();
    }

    private void Start()
    {
        playerCon = player.GetComponent<PlayerController_v3>();
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }

    private void Update() 
    {
        //Vector3 pos;
        //playerPos = player.transform.position;
        //playerPos.z = -10;

        //if(ReferenceEquals(co, null))
        //{
            //pos = new Vector3(Mathf.Clamp(playerPos.x, minSize.x, maxSize.x), Mathf.Clamp(playerPos.y, minSize.y, maxSize.y), playerPos.z);
            //transform.position = pos;
        //}


        ////////////////////////////////////////////////////////////
        //float dialspeed = 25;
        //sbyte mod = 0;      // -128 ~ 127
        Vector3 dir = player.transform.position - this.transform.position;  //방향 구하기
        Vector3 dest = player.transform.position + dialVec;

        if(dialogueOn)
        {
            //mod = 1;
            //dialspeed = dialSpeed;
            //zoom = Mathf.Lerp(zoom, zoomlv, zoomSpeed);

            transform.position = Vector3.MoveTowards(transform.position, dest, Time.deltaTime * dialSpeed);
        }
        else
        {
            if(flag)  //다이얼로그 끝나고 원상복구시키기
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * dialSpeed);
                if(transform.position.x - player.transform.position.x < 0.08 && transform.position.y - player.transform.position.y < 0.08)
                {
                    playerCon.SetCamera(false);
                    flag = false;
                }
            }
            else
            {
                transform.position = player.transform.position;
            }
        }

        if(isForce)
        {
            transform.position = dest;
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minSize.x, maxSize.x), Mathf.Clamp(transform.position.y, minSize.y, maxSize.y), -10);   //맵 경계 제한 적용
    }

    /*IEnumerator MoveDialogue()
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
            if(transform.position.x == player.transform.position.x && transform.position.y == player.transform.position.y) break;

            yield return null;
        }

        co = null;
    }*/

    public void SetFlag()   //이벤트로 호출되면 플레그를 반전, 다이얼로그에서 호출
    {
        dialogueOn = !dialogueOn;

        if(!dialogueOn)
        {
            playerCon.SetCamera(true);
            flag = true;
        }
        //if(ReferenceEquals(co, null)) 
        //{   
        //    co = StartCoroutine(MoveDialogue());   
        //}
    }

    /// <summary>
    /// 강제로 카메라가 플레이어를 따라가도록 설정합니다.
    /// </summary>
    /// <param name="enable">True = 활성화</param>
    public void SetForceFollow(bool enable)
    {
        isForce = enable;
    }

    void SetMapSize()
    {
        Vector2 bias = new Vector2(-3.5f, -1);                  //bias를 사용하여 챕터 1에서 맵이 짤리는 문제 방지
        float height = Camera.main.orthographicSize;            //orthographicSize * 2 = Height // 우리가 필요한 것은 중간값이므로 * 2생략
        float width = height * Screen.width / Screen.height;    //Height * aspect = Width //위에서 Height에 / 2를 하였으므로 너비의 중간값을 구할수있다.
        height += bias.y; width += bias.x;

        BoundsInt bound = border.cellBounds;       //맵 관련 정보 가져오기

        maxSize.x = bound.xMax - width; maxSize.y = bound.yMax - height;    //카메라 이동 제한 좌표 추가
        minSize.x = bound.xMin + width; minSize.y = bound.yMin + height;
        //Debug.Log(maxSize + "   "  + minSize);
    }
}
