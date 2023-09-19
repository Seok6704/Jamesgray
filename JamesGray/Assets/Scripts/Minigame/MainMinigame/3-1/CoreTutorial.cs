using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoreTutorial : MonoBehaviour
{
    public bool isMain; // 튜토리얼인지 본 게임인지 구분하는 변수
    GameObject click;
    int ran, allClear, fail;
    string problem;
    int incorrect = 0;
    int round = 0;
    bool isChoice = true; // 중복 클릭 방지

    void Start()
    {
        if(isMain) 
        {
            allClear = 10;
            fail = 2;
        }
        else
        {
            allClear = 8;
            fail = 3;
        }
    }

    public void OnStartClick()
    {
        Invoke("TalkProblem", 2f);
    }

    public void OnChoice()
    {
        if(isChoice) return;
        else isChoice = true;
        click = EventSystem.current.currentSelectedGameObject; // 클릭 오브젝트 받아오기
        if(click.name == problem)
        {
            Debug.Log("correct!");
            round++;
            if(round >= allClear) Debug.Log("Clear!"); // 클리어. 본 버전에서는 해당 부분이 씬 전환으로 대체 될 예정
            else Invoke("TalkProblem", 2f);
        }
        else
        {
            Debug.Log("incorrect!");
            round++;
            incorrect++;
            if(round >= allClear) Debug.Log("Clear!");
            else if(incorrect >= fail) Debug.Log("Fail!"); // 실패. 본 버전에서는 해당 부분이 씬 전환으로 대체 될 예정
            else Invoke("TalkProblem", 2f);
        }
    }

    void TalkProblem() // 문제 제출 함수
    {
        isChoice = false;
        if(!isMain) ran = Random.Range(0, 8);
        else ran = Random.Range(0, 10);
        switch (ran)
        {
            case 0:
                Debug.Log("제임스 삼각형"); // 임시. 본 버전에서는 해당 부분이 음성으로 대체 될 예정
                problem = "BtnJTri";
                break;
            case 1:
                Debug.Log("제임스 사각형");
                problem = "BtnJSqu";
                break;
            case 2:
                Debug.Log("제임스 오각형");
                problem = "BtnJPen";
                break;
            case 3:
                Debug.Log("제임스 원");
                problem = "BtnJCir";
                break;
            case 4:
                Debug.Log("로즈 삼각형");
                problem = "BtnRTri";
                break;
            case 5:
                Debug.Log("로즈 사각형");
                problem = "BtnRSqu";
                break;
            case 6:
                Debug.Log("로즈 오각형");
                problem = "BtnRPen";
                break;
            case 7:
                Debug.Log("로즈 원");
                problem = "BtnRCir";
                break;
            case 8: // 8, 9 번은 3-2 미니게임에서만 적용
                Debug.Log("로즈 육각형");
                problem = "BtnRHex";
                break;
            case 9:
                Debug.Log("제임스 육각형");
                problem = "BtnJHex";
                break;

        }

    }
}
