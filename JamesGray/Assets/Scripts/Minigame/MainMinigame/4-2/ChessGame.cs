using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
4-2 체스 미니게임 스크립트입니다.
*/

public class ChessGame : MonoBehaviour
{
    GameObject clickObj, clickCell;
    Outline outline;
    List<GameObject> onOutline = new List<GameObject>();
    Animator chess;
    Vector3 pos;
    List<string> nowPos = new List<string>();
    int row, cul, row2, cul2;
    List<string> nowCell = new List<string>();
    int ran;
    NPCManager npcManager;
    Vector3 ponePos, knightPos, bishopWPos, bishopBPos, rookPos;
    bool isClear;
    int blinkCount;

    void Start()
    {
        ran = Random.Range(0,3);
        npcManager = GameObject.Find("Start_Button").GetComponent<NPCManager>();
        npcManager.i_Story = ran;
        blinkCount = 0;
        chess = GameObject.Find("Chess").GetComponent<Animator>();
    }

    public void MakeProblem() // 문제 출제 함수, Start 버튼 클릭 시, 한번 호출됨.
    {
        switch (ran)
        {
            case 0 :
                ponePos = GameObject.Find("23").transform.position;
                knightPos = GameObject.Find("33").transform.position;
                bishopWPos = GameObject.Find("32").transform.position;
                bishopBPos = GameObject.Find("42").transform.position;
                rookPos = GameObject.Find("00").transform.position;
                break;
            case 1 :
                ponePos = GameObject.Find("21").transform.position;
                knightPos = GameObject.Find("04").transform.position;
                bishopWPos = GameObject.Find("03").transform.position;
                bishopBPos = GameObject.Find("13").transform.position;
                rookPos = GameObject.Find("44").transform.position;
                break;
            case 2 :
                ponePos = GameObject.Find("24").transform.position;
                knightPos = GameObject.Find("03").transform.position;
                bishopWPos = GameObject.Find("41").transform.position;
                bishopBPos = GameObject.Find("20").transform.position;
                rookPos = GameObject.Find("12").transform.position;
                break;
        }
    }

    void ProblemCheck() // 문제 확인 함수, 말 이동 직후 호출
    {
        if(ponePos == GameObject.Find("pone").transform.position && knightPos == GameObject.Find("knight").transform.position && bishopWPos == GameObject.Find("bishopW").transform.position && bishopBPos == GameObject.Find("bishopB").transform.position && rookPos == GameObject.Find("rook").transform.position)
        {
            isClear = true;
            SceneChanger();
        }
    }

    public void PiecePosUpdate() // 현재 체스 말 위치 업데이트 함수 - 게임 시작 버튼 클릭 직후, 체스 말 이동 직후에 호출됨.
    {
        GameObject [] nowObj = GameObject.FindGameObjectsWithTag("piece");
        for(int i = 0; i < nowObj.Length; i++)
        {
            cul = (((int)nowObj[i].transform.position.x - 170) / 200);
            row = (((int)nowObj[i].transform.position.y - 140) / 200);
            nowPos.Add(cul.ToString() + row.ToString());
        }
    }

    public void OnChessClick() // 체스 말 클릭 함수
    {
        if(onOutline != null) // 아웃라인이 이미 활성화 되어 있는지 검사(아웃라인이 이미 활성화 되어 있다 => 다른 말을 클릭한적이 있다)
        {
            for(int i = 0; i < onOutline.Count; i++) // 아웃라인이 이미 활성화 되어 있었을 경우, 활성화 된 아웃라인 제거
            {
                onOutline[i].GetComponent<Outline>().enabled = false;
            }
            onOutline.Clear(); // 아웃라인 리스트 초기화
            nowCell.Clear(); // 이동 가능한 위치 리스트 초기화
        }
        clickObj = EventSystem.current.currentSelectedGameObject; // 클릭 오브젝트 받아오기
        pos = clickObj.transform.position; // 오브젝트 위치 값 저장
        cul = (((int)pos.x-170) / 200); // 좌표값으로 변경
        row = (((int)pos.y-140) / 200);
        cul2 = cul;
        row2 = row;
        OutlineControll(clickObj.name); // 아웃라인 관리 함수 호출
    }

    void OutlineControll(string obj) // 아웃라인 관리 함수
    {
        switch (obj) // 클릭한 체스 말 종류에 따라 각각 다른 이동 경로를 보여주기 위해, 스위치문을 이용하여 클릭한 오브젝트의 이름을 구분.
        {
            case "pone" : // 폰일 경우
                row += 1; // y축 값 1 증가(체스 룰에서는 첫 이동은 최대 2칸까지 가능하나, 5*5의 작은 체스이기도 하기에 무조건 1칸 이동으로 고정하였음)
                if(row > 4) break; // 폰이 체스판의 끝에 도달한 경우, 더이상 이동X
                nowCell.Add(cul.ToString() + row.ToString()); // 이동 가능 위치 리스트에 좌표 값 추가

                if(nowPos.Contains(nowCell[nowCell.Count-1])) nowCell.RemoveAt(nowCell.Count - 1); // 폰 이동 위치에 다른 말이 있을 경우, 이동 위치 삭제
                
                for(int i = 0; i < nowCell.Count; i++) // 이동 가능 위치 아웃라인 활성화
                {
                    onOutline.Add(GameObject.Find(nowCell[i]));
                    outline = onOutline[i].GetComponent<Outline>();
                    outline.enabled = true;
                }
                break;

            case "knight" : // 나이트일 경우 이동 가능 위치가 총 8개가 존재. (자신의 가로축, 세로축 및 가장 가까운 대각선을 제외하고 가장 가까운 칸 8개가 이동 가능 위치임.)
                cul2 = cul - 2;
                if(cul2 >= 0 && cul2 <= 4)
                {
                    row2 = row + 1;
                    if(row2 >= 0 && row2 <= 4) nowCell.Add(cul2.ToString() + row2.ToString());
                    row2 = row - 1;
                    if(row2 >= 0 && row2 <= 4) nowCell.Add(cul2.ToString() + row2.ToString());
                }

                cul2 = cul - 1;
                if(cul2 >= 0 && cul2 <= 4)
                {
                    row2 = row + 2;
                    if(row2 >= 0 && row2 <= 4) nowCell.Add(cul2.ToString() + row2.ToString());
                    row2 = row - 2;
                    if(row2 >= 0 && row2 <= 4) nowCell.Add(cul2.ToString() + row2.ToString());
                }

                cul2 = cul + 1;
                if(cul2 >= 0 && cul2 <= 4)
                {
                    row2 = row + 2;
                    if(row2 >= 0 && row2 <= 4) nowCell.Add(cul2.ToString() + row2.ToString());
                    row2 = row - 2;
                    if(row2 >= 0 && row2 <= 4) nowCell.Add(cul2.ToString() + row2.ToString());
                }

                cul2 = cul + 2;
                if(cul2 >= 0 && cul2 <= 4)
                {
                    row2 = row + 1;
                    if(row2 >= 0 && row2 <= 4) nowCell.Add(cul2.ToString() + row2.ToString());
                    row2 = row - 1;
                    if(row2 >= 0 && row2 <= 4) nowCell.Add(cul2.ToString() + row2.ToString());
                }
                //해당 이동 규칙 관련 부분은 추후 보완 방법이 떠오르면 보완할 예정.

                for(int i = 0; i < nowPos.Count; i++) // 말 위치와 일치하는 이동 위치 삭제
                {
                    nowCell.Remove(nowPos[i]);
                }

                for(int i = 0; i < nowCell.Count; i++)
                {
                    onOutline.Add(GameObject.Find(nowCell[i]));
                    outline = onOutline[i].GetComponent<Outline>();
                    outline.enabled = true;
                }
                break;

            case string a when (a == "bishopW" || a == "bishopB") : // 비숍일 경우 대각선 4방향이 이동 가능 위치
                for(int i = row + 1; i <= 4; i++) // 우측 상단 대각선
                {
                    cul += 1;
                    if(cul > 4) break;
                    nowCell.Add(cul.ToString() + i.ToString());
                    if(nowPos.Contains(nowCell[nowCell.Count-1])) // 가장 마지막에 추가 된 이동 위치가 다른 말의 위치와 일치할 경우, 그 뒤 위치는 담지 않음.
                    {
                        nowCell.RemoveAt(nowCell.Count - 1); // 겹친 위치 삭제
                        break;
                    } 
                }
                cul = cul2;
                for(int i = row + 1; i <= 4; i++) // 좌측 상단 대각선
                {
                    cul -= 1;
                    if(cul < 0) break;
                    nowCell.Add(cul.ToString() + i.ToString());
                    if(nowPos.Contains(nowCell[nowCell.Count-1]))
                    {
                        nowCell.RemoveAt(nowCell.Count - 1);
                        break;
                    }
                }
                cul = cul2;
                for(int i = row - 1; i >= 0; i--) // 우측 하단 대각선
                {
                    cul += 1;
                    if(cul > 4) break;
                    nowCell.Add(cul.ToString() + i.ToString());
                    if(nowPos.Contains(nowCell[nowCell.Count-1]))
                    {
                        nowCell.RemoveAt(nowCell.Count - 1);
                        break;
                    }
                }
                cul = cul2;
                for(int i = row - 1; i >= 0; i--) // 좌측 하단 대각선
                {
                    cul -= 1;
                    if(cul < 0) break;
                    nowCell.Add(cul.ToString() + i.ToString());
                    if(nowPos.Contains(nowCell[nowCell.Count-1]))
                    {
                        nowCell.RemoveAt(nowCell.Count - 1);
                        break;
                    }
                }

                //해당 이동 규칙 관련 부분도 좀 더, 개선할 방안이 떠오르면 개선할 예정. 현재 방법은 상당히 비효율적으로 생각.

                for(int i = 0; i < nowCell.Count; i++)
                {
                    onOutline.Add(GameObject.Find(nowCell[i]));
                    outline = onOutline[i].GetComponent<Outline>();
                    outline.enabled = true;
                }
                break;

            case "rook" : // 룩일 경우, 자신 기준 직선 4방향이 이동 가능 위치
                for(int i = row - 1; i >= 0; i--) // 하단 이동
                {
                    nowCell.Add(cul.ToString() + i.ToString());
                    if(nowPos.Contains(nowCell[nowCell.Count-1])) // 다른 말의 위치와 겹칠 경우, 제거하고 그 뒤는 생성하지 않음.
                    {
                        nowCell.RemoveAt(nowCell.Count - 1);
                        break;
                    }
                }

                for(int i = row + 1; i <= 4; i++) // 상단 이동
                {
                    nowCell.Add(cul.ToString() + i.ToString());
                    if(nowPos.Contains(nowCell[nowCell.Count-1])) // 다른 말의 위치와 겹칠 경우, 제거하고 그 뒤는 생성하지 않음.
                    {
                        nowCell.RemoveAt(nowCell.Count - 1);
                        break;
                    }
                }

                for(int i = cul - 1; i >= 0; i--) // 좌측 이동
                {
                    nowCell.Add(i.ToString() + row.ToString());
                    if(nowPos.Contains(nowCell[nowCell.Count-1])) 
                    {
                        nowCell.RemoveAt(nowCell.Count - 1);
                        break;
                    }
                }

                for(int i = cul + 1; i <= 4; i++) // 우측 이동
                {
                    nowCell.Add(i.ToString() + row.ToString());
                    if(nowPos.Contains(nowCell[nowCell.Count-1])) 
                    {
                        nowCell.RemoveAt(nowCell.Count - 1);
                        break;
                    }
                }

                //해당 이동은 하나의 반복문만으로 처리하였었으나, 다른 말에 막히는 부분이 구현이 불가능하여, 네 방위로 쪼개서 구현하였음. 추후, 더 좋은 구현 법이 생각나면 수정 할 예정.

                for(int i = 0; i < nowCell.Count; i++)
                {
                    onOutline.Add(GameObject.Find(nowCell[i]));
                    outline = onOutline[i].GetComponent<Outline>();
                    outline.enabled = true;
                }
                break;
        }
    }

    public void OnMove() // 말 이동 함수
    {
        clickCell = EventSystem.current.currentSelectedGameObject; // 클릭 칸 확인
        outline = clickCell.GetComponent<Outline>();
        if(outline.enabled) // 클릭 칸의 아웃라인이 활성화 되어 있을 경우 -> 이동이 가능한 칸일 경우
        {
            clickObj.transform.position = clickCell.transform.position; // 위치값 변경
            for(int i = 0; i < onOutline.Count; i++)
            {
                onOutline[i].GetComponent<Outline>().enabled = false; // 전체 아웃라인 비활성화
            }
            onOutline.Clear();
            nowCell.Clear();
        }
        else // 아웃라인이 활성화 되어 있지 않을 경우 -> 이동이 불가능하고 허공을 클릭하였으므로, 아웃라인 및 말 선택 초기화
        {
            for(int i = 0; i < onOutline.Count; i++)
            {
                onOutline[i].GetComponent<Outline>().enabled = false;
            }
            onOutline.Clear();
            nowCell.Clear();
        }
        ProblemCheck();
        nowPos.Clear();
        PiecePosUpdate();
    }

    public void PlusBlinkCount()
    {
        blinkCount++;
        OnBlink();
    }

    public void MinusBlinkCount()
    {
        blinkCount--;
        OnBlink();
    }

    void OnBlink()
    {
        Debug.Log(blinkCount);        
        chess.SetBool("PoneBlink", false);
        chess.SetBool("RookBlink", false);
        chess.SetBool("KnightBlink", false);
        chess.SetBool("BishopBlink", false);

        switch (blinkCount)
        {
            case 2:
                chess.SetBool("PoneBlink", true);
                break;
            case 3:
                chess.SetBool("RookBlink", true);
                break;
            case 4:
                chess.SetBool("KnightBlink", true);
                break;
            case 5:
                chess.SetBool("BishopBlink", true);
                break;
        }
    }

    void SceneChanger() //씬 전환 함수
    {
        SceneManager.UnloadSceneAsync(gameObject.scene);    //현재 씬 종료
        SceneManager.SetActiveScene(LoadingScene.preScene); //기억하고 있던 이전 씬을 액티브로 전환
        
        GameObject[] objects = SceneManager.GetActiveScene().GetRootGameObjects();

        for(int i = 0; i < objects.Length; i++)
        {
            if(objects[i].name == "SceneManager" || objects[i].name == "Scene Manager")
            {
                objects[i].GetComponent<SceneController>().AdditiveEnded(isClear);
                break;
            }
        }
    }
}

/*
체스 말 이미지를 터치하면 해당 좌표의 값을 받아온 후, 해당 칸의 좌표와 일치하는 체스판 번호를 찾음.
이후, 체스말 정보에 따라 진행 할 수 있는 방향의 칸을 Outline을 그려 유저에게 알려줌.
Outline이 활성화 된 칸에 한하여, 클릭 할 경우, 해당 칸으로 체스말이 이동.
*/
