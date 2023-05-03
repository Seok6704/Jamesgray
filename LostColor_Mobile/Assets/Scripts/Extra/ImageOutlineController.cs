using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageOutlineController : MonoBehaviour //이미지에 아웃라인이 있을 경우 컨트롤하는 스크립트
{
    public Color effectColor; //원하는 컬러 설정
    Outline outline;

    Color defaultColor;

    void Start() {
        outline = GetComponent<Outline>();
        effectColor = new Color(effectColor.r, effectColor.g, effectColor.b, 1.0f);
        defaultColor = new Color(effectColor.r, effectColor.g, effectColor.b, 0.0f);
    }

    public void OnSelect(){
        outline.effectColor = effectColor;
    }

    public void OnUnselect(){
        outline.effectColor = defaultColor;
    }
}
