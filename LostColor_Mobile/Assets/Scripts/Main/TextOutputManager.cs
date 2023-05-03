using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TextOutputManager : MonoBehaviour
{
    public UnityEvent typeDone;
    [Range(0, 10)]
    [Tooltip("1초")]
    public float f_dial_Speed;  
    TMP_Text tmp_Text;
    Coroutine co;
    
    private void Awake() 
    {
        SetText();
    }
    void SetText() 
    {
        tmp_Text = GetComponent<TMP_Text>();
        tmp_Text.text = "";
    }

    public void PrintDirect(string s_dial) 
    {    //바로 출력하는 함수
        tmp_Text.text = s_dial;
    }

    public void ClearText() //문장 비우기
    {
        tmp_Text.text = "";
    }

    public void StopTyping()
    {
        if(co != null) StopCoroutine(co);
    }

    public void Typing(string s_dial) 
    {     //힌글자씩 천천히 출력하는 함수
        co = StartCoroutine(PutText(s_dial));
    }
    IEnumerator PutText(string s_dial) 
    {  //천천히 출력하는 코루틴
        foreach (char c in s_dial.ToCharArray()) 
        {
            tmp_Text.text += c;
            yield return new WaitForSeconds(f_dial_Speed);
        }
        typeDone.Invoke();
    }

}
