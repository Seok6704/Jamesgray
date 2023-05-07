using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Vito API로 받아온 데이터의 유효성을 검사하는 스크립트

public class VoiceRecognition : MonoBehaviour
{
    public TMPro.TMP_Text msg;      //STT 결과 값이 출력되는 Text 객체를 사용하여 유효성 검사
    [Tooltip("오차 허용 값 (0 : 오차 없음 ~ 10 : 모든 오차 허용)")]
    public int similarity = 2;

    string answer = "안녕하세요";

    private void Start() 
    {
        if(similarity > 10) similarity = 10;
        if(similarity < 0) similarity = 0;    
    }

    public void CheckSimilarity()
    {
        CompareString();
    }

    void CompareString()
    {
        //int error_count = (msg.text.Length * similarity) / 10;
        int error_count = 2; //두글자 오차 허용
        for(int i = 0; i < msg.text.Length && i < answer.Length; i++)
        {
            if(msg.text[i] != answer[i])
            {
                error_count--;
            }
        }  
        if(error_count < 0)
        {
            Debug.Log("틀렸습니다.");
        }
        else
        {
            Debug.Log("맞았습니다");
        }
    }
}
