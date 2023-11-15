using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/*
미니게임 4-3 관련 문서입니다.
*/

public class PasswordGame : MonoBehaviour
{
    //Dictionary<int, int> pWord = new Dictionary<int, int> (); 딕셔너리로 구성하여, Value 값을 호출할 수 있는 횟수로 하려 했으나, 랜덤성을 부여하는 과정에서 적합하지 않아 반려함.
    public GameObject Dialog;
    List<int> password = new List<int> (); // 비밀번호 리스트 0~9
    int ran; // 랜덤 변수
    public TextMeshProUGUI text; // 입력된 암호 텍스트
    string answer = "36"; // 정답 암호 텍스트 (임시, 본 정답은 36)
    int now; // 현재 리스트에서 꺼낸 암호
    bool fail;
    bool isClear;

    void Start()
    {
        SetpWord(); // 리스트 세팅
    }

    public void OnStartClick() // 시작 버튼 클릭 후, 비밀번호 재생 시작
    {
        StartCoroutine("EnterPassword");
    }

    void SetpWord() // 패스워드 세팅, 리스트를 다시 초기화 하기 위해 제작, 초기화 하지 않을 경우, 암호가 랜덤으로 지급되어, 후에 입력하는 암호가 먼저 나올 경우, 입력할 수 없게 됨.
    {
        for(int i = 1; i < 10; i++)
        {
            password.Add(i);
        }
    }

    public void EnterClick() // 입력 버튼 클릭
    {
        if(!fail)
        {
            text.text += now.ToString(); // 현재 암호 텍스트에 입력
            StopCoroutine("EnterPassword");
            SetpWord();
            StartCoroutine("EnterPassword");
        
            if(text.text == answer)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 1);
                ClearAndFail.GameClear();
                Invoke("NextScene", 4f);
                StopCoroutine("EnterPassword");
            }
        }
    }

    IEnumerator EnterPassword() // 암호 재생 코루틴
    {
        while(true)
        {
            if(text.text.Length >= 3)
            {
                fail = true;
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 2);
                ClearAndFail.GameFail();
                Invoke("SceneChanger", 4f);
                yield break;
            }
            else if(password.Count <= 0)
            {
                fail = true;
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 2);
                ClearAndFail.GameFail();
                Invoke("SceneChanger", 4f);
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(2f);
                ran = Random.Range(0, password.Count - 1);
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, password[ran] + 2);
                now = password[ran];
                password.RemoveAt(ran);
            }
            
        }
    }

    /*void EnterpWord()
    {
        ran = Random.Range(0, 10);
        if(pWord.Any(item => item.Value.Equals(1)) || pWord.Any(item => item.Value.Equals(2)))
        {
            if(pWord[ran] != 0)
            {
                Debug.Log(ran);
                pWord[ran] -= 1;
            }
            else
            {
                EnterpWord();
            }
        }
        else flag = false;
    }*/
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

    void NextScene()
    {
        SceneManager.LoadScene("4-5");
    }

}
