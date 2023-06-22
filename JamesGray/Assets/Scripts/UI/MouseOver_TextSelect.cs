using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//TextSelect 스크립트와 연계되어 동작하는 스크립트, 마우스가 UI위로 올라오면 해당 UI인덱스를 TextSelect로 전송

public class MouseOver_TextSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject SelectManager;    //TextSelect 스크립트를 가진 오브젝트
    sbyte data;
    private void OnMouseOver()
    {
        SelectManager.GetComponent<TextSelect>().Enqueue(data);
    }

    public void SetData(sbyte data)
    {
        this.data = data;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectManager.GetComponent<TextSelect>().Enqueue(data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SelectManager.GetComponent<TextSelect>().Enqueue(-1);
    }
}
