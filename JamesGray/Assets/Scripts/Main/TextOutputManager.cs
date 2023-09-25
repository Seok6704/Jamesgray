using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;

/*
    텍스트를 출력하는 스크립트
    인스펙터 창에서 변수를 세부 조정할 수 있다.
    원하는 TMPro 개체에 컴포넌트로 넣어서 사용한다.
*/

public class TextOutputManager : MonoBehaviour
{
    public UnityEvent typeDone, typeStart;
    [Range(0, 10)]
    [Tooltip("1초")]
    public float f_dial_Speed;  
    TMP_Text tmp_Text;
    Coroutine co;
    
    private void Awake() 
    {
        SetText();

        if(typeDone == null)
        {
            typeDone = new UnityEvent();    //이벤트 생성... 스크립트로 이벤트 함수를 사용하려고 하면 이렇게 다들하는데 왜그런지 모르겠음...
        }
        if(typeStart == null)
        {
            typeStart = new UnityEvent();
        }
    }
    void SetText() //초기화 함수, 자기자신의 TMPro 컴포넌트를 불러오고, 글자를 모두 초기화한다.
    {
        tmp_Text = GetComponent<TMP_Text>();
        tmp_Text.text = "";
    }

    public void PrintDirect(string s_dial) //바로 출력하는 함수
    {   
        StopTyping();
        tmp_Text.text = s_dial;
        typeDone.Invoke();
    }

    public void ClearText() //문장 비우기
    {
        StopTyping();
        tmp_Text.text = "";
    }

    public void StopTyping()    //출력중이라면 멈추기
    {
        if(co != null) StopCoroutine(co);
        co = null;
    }

    public void Typing(string s_dial) //힌글자씩 천천히 출력하는 함수
    {
        StopTyping();
        co = StartCoroutine(PutText(s_dial));
    }
    IEnumerator PutText(string s_dial) //한글자씩 코루틴으로 출력 Typing 함수가 불러오는 코루틴
    {  //천천히 출력하는 코루틴
        typeStart.Invoke(); //문장 출력 시작 이벤트
        foreach (char c in s_dial.ToCharArray()) 
        {
            tmp_Text.text += c;
            yield return new WaitForSeconds(f_dial_Speed);
        }
        typeDone.Invoke();  //문장 출력 완료 이벤트
    }

}
