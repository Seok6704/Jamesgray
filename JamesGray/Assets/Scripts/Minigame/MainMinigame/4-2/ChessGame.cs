using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChessGame : MonoBehaviour
{
    string bishopW, bishopB, look, pone, knight;
    GameObject clickObj;
    Vector3 pos;
    int raw, cul;
    string nowCell;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnChessClick()
    {
        clickObj = EventSystem.current.currentSelectedGameObject; // 클릭 오브젝트 받아오기
        pos = clickObj.transform.position;
        raw = ((int)pos.x / 200 + 2);
        cul = ((int)pos.y / 200 + 2);
        nowCell = raw.ToString() + cul.ToString();
        Debug.Log(nowCell);
    }
}

/*
체스 말 이미지를 터치하면 해당 좌표의 값을 받아온 후, 해당 칸의 좌표와 일치하는 체스판 번호를 찾음.
이후, 체스말 정보에 따라 진행 할 수 있는 방향의 칸을 Outline을 그려 유저에게 알려줌.
Outline이 활성화 된 칸에 한하여, 클릭 할 경우, 해당 칸으로 체스말이 이동.
*/
