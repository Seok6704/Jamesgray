using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//이미지 페이딩 스크립트
//서서히 투명화 되도록 함.
//사용법 : 원하는 이미지에 해당 스크립트를 넣은 다음, 원하는 속도 및 f_max(최대 불투명도), f_min(최대 투명도)를 설정하여 이벤트로 불러와서 사용.
//텍스트 페이딩과 기본적으로 비슷함.
public class Img_Fading : MonoBehaviour
{
    [Header ("페이딩 이미지")]
    public Image img;   //페이딩 될 이미지
    [Tooltip ("0.002 추천")]
    public float f_fading_speed;
    public float f_max, f_min;

    bool is_blinking;
    Color color;

    void Set_Color(){
        img.color = color;
    }
    void Start(){
        is_blinking = false;
        color = img.color;
        f_min = 0;
    }
    public void Set_blink(bool set){
        is_blinking = set;
    }

    public void Stop_fading(){  //페이딩 하지마
        is_blinking = false;
        color = new Color(color.r, color.g, color.b, f_max);
        Set_Color();
    }
    public void Start_fading(){   //페이딩 시작
        is_blinking = true;
    }

    public void Never_fading(){ //다시는 안보이게 만들기
        Stop_fading();
        color = new Color(color.r, color.g, color.b, 0.0f);
        Set_Color();
        Destroy(this);
    }

    public void SetTransparent()    //f_min으로 설정된 최소 투명도를 기준으로 바로 투명화.... 초기 설정을 위해 작성됨.
    {
        color = new Color(color.r, color.g, color.b, f_min);
        Set_Color();
    }

    void Update() {
        if(is_blinking) {
            if(color.a >= f_max) StartCoroutine(fading());
            else if(color.a <= f_min) StartCoroutine(rising());
        }
    }

    IEnumerator fading(){   //이미지의 알파값 증감 코루틴
        for(float i = color.a; i >= f_min; i -= f_fading_speed){
            color = new Color(color.r, color.g, color.b, i);
            Set_Color();
            yield return null;
        }
        color = new Color(color.r, color.g, color.b, f_min);
        Set_Color();
    }

    IEnumerator rising(){
        for(float i = color.a; i <= f_max; i += f_fading_speed) {
            color = new Color(color.r, color.g, color.b, i);
            Set_Color();
            yield return null;
        }
        color = new Color(color.r, color.g, color.b, f_max);
        Set_Color();
    }
    public void ShowUp(){
        StartCoroutine(rising());
    }
    public void ShowDown(){
        StartCoroutine(fading());
    }
}
