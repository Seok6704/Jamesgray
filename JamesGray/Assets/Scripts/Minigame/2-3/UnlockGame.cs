using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UnlockGame : MonoBehaviour
{
    VoiceManager voice;
    bool flag = true; // 녹음 동작용 플래그
    public TextMeshProUGUI orderText; // 입력된 텍스트
    string recordText; // 녹음된 텍스트
    string correctText = "사용을 종료하겠습니다."; // 정답 문구
    public bool isClear; // 클리어 확인 변수, 클리어 시, 감옥 해제 애니메이션 재생을 위해 해당 스크립트는 public으로 작성되었음.

    void Start()
    {
        voice = GameObject.Find("Panel_Minigame").GetComponent<VoiceManager>();
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
            isClear = true;
            Invoke("SceneChanger", 2f);
        }
        else
        {
            isClear = false;
            Invoke("SceneChanger", 2f);
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
