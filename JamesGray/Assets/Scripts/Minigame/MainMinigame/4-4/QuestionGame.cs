using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class QuestionGame : MonoBehaviour
{
    VoiceManager voice;
    bool flag = true; // 녹음 동작용 플래그
    public TextMeshProUGUI orderText; // 입력된 텍스트
    string recordText; // 녹음된 텍스트
    List<string> answerList = new List<string> {"홀리 몰리.", "제임스.", "로즈.", "자동차.", "타워."};
    string correctText;
    public static bool isClear;
    int ran, count = 0;

    void Start()
    {
        voice = GameObject.Find("Panel_Minigame").GetComponent<VoiceManager>();
    }

    public void OnStartClick()
    {
        ShowQuestion(true);
    }

    void ShowQuestion(bool flag)
    {
        if(flag)
        {
            ran = Random.Range(0, answerList.Count - 1);
            count++;
        }
        switch (answerList[ran])
        {
            case "홀리 몰리.":
                Debug.Log("저희 마을의 이름은 무엇입니까?");
                correctText = answerList[ran];
                break;
            case "제임스.":
                Debug.Log("당신이 플레이하고 있는 캐릭터의 이름은 무엇입니까?");
                correctText = answerList[ran];
                break;
            case "로즈.":
                Debug.Log("당신과 동행하고 있는 아이의 이름은 무엇입니까?");
                correctText = answerList[ran];
                break;
            case "자동차.":
                Debug.Log("제임스가 저희 마을에 올 때 이용한 수단은 무엇이었습니까?");
                correctText = answerList[ran];
                break;
            case "타워.":
                Debug.Log("아이를 구출한 장소는 어디였습니까?");
                correctText = answerList[ran];
                break;
        }
    }

    public void OnClickRecording() // 명령 버튼 클릭 시, 동작할 함수
    {
        if (flag) 
        {
            voice.startRecording(); // 녹음 시작
            flag =! flag;
        }
        else
        {
            StopRecording(); // 녹음 종료
            flag =! flag;
        }
    }

    void StopRecording()
    {
        voice.stopRecording();
        voice.CallPost(); // 녹음된, wav 파일 전송
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        yield return new WaitForSeconds(2f);
        voice.CallGetText(); // 변형된 text 파일 수신
    }

    public void SetText() // STTDone 이벤트를 통해 불러질 함수
    {
        recordText = voice.Text;
        orderText.text = "현재 입력된 명령 : " + recordText;
        if(recordText == correctText) 
        {
            answerList.RemoveAt(ran);
            if(count >= 1)
            {
                isClear = true;
                Invoke("SceneChanger", 2f);
            }
            else
            {
                ShowQuestion(true);
            }
        }
        else
        {
            ShowQuestion(false);
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
