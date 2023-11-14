using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CoreTutorial : MonoBehaviour
{
    public bool isMain; // 튜토리얼인지 본 게임인지 구분하는 변수
    public GameObject Dialog;
    GameObject click;
    int ran, allClear, fail;
    string problem;
    int incorrect = 0;
    int round = 0;
    bool isChoice = true; // 중복 클릭 방지
    bool isClear = false;
    public static bool isClear2 = false; // 3-2 클리어 확인용 변수

    void Start()
    {
        if(isMain) 
        {
            allClear = 1;
            fail = 2;
        }
        else
        {
            allClear = 1;
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
            if(round >= allClear)
            {
                if(isMain) isClear2 = true;
                isClear = true;
                ClearAndFail.GameClear();
                Invoke("SceneChanger", 1f);
            }
            else Invoke("TalkProblem", 2f);
        }
        else
        {
            Debug.Log("incorrect!");
            round++;
            incorrect++;
            if(round >= allClear)
            {
                if(isMain) isClear2 = true;
                isClear = true;
                ClearAndFail.GameClear();
                Invoke("SceneChanger", 1f);
            }
            else if(incorrect >= fail)
            {
                ClearAndFail.GameFail();
                Invoke("SceneChanger", 1f); // 실패. 본 버전에서는 해당 부분이 씬 전환으로 대체 될 예정
            }
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
                Dialog.GetComponent<DialoguesManager>().SetDialogue(800, 4);
                problem = "BtnJTri";
                break;
            case 1:
                Debug.Log("제임스 사각형");
                Dialog.GetComponent<DialoguesManager>().SetDialogue(800, 5);
                problem = "BtnJSqu";
                break;
            case 2:
                Debug.Log("제임스 오각형");
                Dialog.GetComponent<DialoguesManager>().SetDialogue(800, 6);
                problem = "BtnJPen";
                break;
            case 3:
                Debug.Log("제임스 원");
                Dialog.GetComponent<DialoguesManager>().SetDialogue(800, 7);
                problem = "BtnJCir";
                break;
            case 4:
                Debug.Log("로즈 삼각형");
                Dialog.GetComponent<DialoguesManager>().SetDialogue(801, 0);
                problem = "BtnRTri";
                break;
            case 5:
                Debug.Log("로즈 사각형");
                Dialog.GetComponent<DialoguesManager>().SetDialogue(801, 1);
                problem = "BtnRSqu";
                break;
            case 6:
                Debug.Log("로즈 오각형");
                Dialog.GetComponent<DialoguesManager>().SetDialogue(801, 2);
                problem = "BtnRPen";
                break;
            case 7:
                Debug.Log("로즈 원");
                Dialog.GetComponent<DialoguesManager>().SetDialogue(801, 3);
                problem = "BtnRCir";
                break;
            case 8: // 8, 9 번은 3-2 미니게임에서만 적용
                Debug.Log("로즈 육각형");
                Dialog.GetComponent<DialoguesManager>().SetDialogue(801, 4);
                problem = "BtnRHex";
                break;
            case 9:
                Debug.Log("제임스 육각형");
                Dialog.GetComponent<DialoguesManager>().SetDialogue(800, 8);
                problem = "BtnJHex";
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
