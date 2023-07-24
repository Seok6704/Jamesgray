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

    bool dialogueOn = false;
    public GameObject player;
    Vector3 dialVec, playerPos;
    Coroutine co;

    Vector2 maxSize, minSize; //맵의 최소, 최대 좌표

    private void Awake() 
    {
        playerPos = player.transform.position;
        dialVec = new Vector3(x, y, -10f);
        co = null;   
        SetMapSize();
    }

    private void Update() 
    {
        Vector3 pos;
        playerPos = player.transform.position;
        playerPos.z = -10;

        if(co == null)
        {
            pos = new Vector3(Mathf.Clamp(playerPos.x, minSize.x, maxSize.x), Mathf.Clamp(playerPos.y, minSize.y, maxSize.y), playerPos.z);
            transform.position = pos;
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

    void SetMapSize()
    {
        Vector2 bias = new Vector2(-3.5f, -1);
        float height = Camera.main.orthographicSize;            //orthographicSize * 2 = Height // 우리가 필요한 것은 중간값이므로 * 2생략
        float width = height * Screen.width / Screen.height;    //Height * aspect = Width //위에서 Height에 / 2를 하였으므로 너비의 중간값을 구할수있다.
        height += bias.y; width += bias.x;

        BoundsInt bound = border.cellBounds;

        maxSize.x = bound.xMax - width; maxSize.y = bound.yMax - height;
        minSize.x = bound.xMin + width; minSize.y = bound.yMin + height;

        Debug.Log(maxSize + "   "  + minSize);
    }
}
