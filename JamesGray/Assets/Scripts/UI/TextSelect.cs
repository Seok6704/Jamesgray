using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSelect : MonoBehaviour
{
    public Color color;             //아웃라인 색상
    [Range(0,5)]
    public float outlinePadding;    //아웃라인 두께
    public TMPro.TMP_Text[] options;
    sbyte index, buffer;    //-128 ~ 127, 버퍼는 가장 최신 값을 1개만 저장하고, 비워지면 -1로 초기화됨

    private void Awake() 
    {
        index = -1;
        buffer = -1;
        for(sbyte i = 0; i < options.Length; i++)   //아웃라인 초기화
        {
            options[i].outlineColor = color;
            options[i].outlineWidth = 0f;
            options[i].transform.parent.GetChild(1).gameObject.SetActive(false);
            options[i].GetComponent<MouseOver_TextSelect>().SetData(i);
        }
    }

    public void SelectUP()
    {
        if(index == -1) index = 0;
        if(index != 0)
            index -= 1;

        if(index <= -1) index = 0;      //혹시라도 배열의 범위를 초과하는 일 방지.

        SetOutline();
    }
    public void SelectDOWN()
    {
        if(index != options.Length - 1)
            index += 1;

        if(index >= options.Length) index = (sbyte)(options.Length - 1);

        SetOutline();
    }

    private void Update()   //화살표 입력 받기.
    {
        if(buffer != -1)    //버퍼안에 값이 존재한다면 우선 처리
        {
            ResetOutline();
            index = buffer; 
            buffer = -1;    //버퍼 비우기
            SetOutline();
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) 
        {
            ResetOutline();
            SelectUP();
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) 
        {
            ResetOutline();
            SelectDOWN();
        }    
    }

    void SetOutline()
    {
        options[index].outlineWidth = outlinePadding;
        options[index].transform.parent.GetChild(1).gameObject.SetActive(true);
    }
    void ResetOutline()
    {
        if(index < 0 || index > options.Length - 1) return;

        options[index].outlineWidth = 0f;
        options[index].transform.parent.GetChild(1).gameObject.SetActive(false);
    }

    public void Enqueue(sbyte data)
    {
        buffer = data;
    }
}
