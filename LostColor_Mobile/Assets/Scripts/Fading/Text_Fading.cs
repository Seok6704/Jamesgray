using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// 원하는 글자(TMPro)를 지정하여 페이딩을 하도록 할 수 있습니다. 알파값을 조정하여 페이딩하며 투명해졌다가 불투명해지기를 반복합니다.
// 추천 페이딩 속도는 0.002
//
// 나중에 자동으로 텍스트를 잡도록 수정할것. (가능한 빨리)
public class Text_Fading : MonoBehaviour
{
    public TMP_Text text;           //페이딩 기능이 추가될 텍스트
    public float f_fading_speed;    //글자의 페이딩 속도 결정 0.002 추천

    public bool is_fading;
    bool stop_rising;


    void Awake() 
    {
        text = GetComponent<TMP_Text>();    
    }
    void Start(){
        //is_fading = true;   //기본값은 페이딩 시작 모드
        stop_rising = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(is_fading) {
            if(text.alpha >= 1.0f) StartCoroutine(fading());
            else if(text.alpha <= 0.0f) StartCoroutine(rising());
        }
    }

    public void Set_fading(bool set){
        is_fading = set;
    }
    public void Stop_fading(){  //페이딩 하지마
        is_fading = false;
        text.alpha = 1.0f;
    }
    public void Start_fading(){   //페이딩 시작
        is_fading = true;
    }

    public void Never_fading(){ //다시는 안보이게 만들기
        is_fading = false;
        stop_rising = true;
        //StopCoroutine(rising(text));
        StartCoroutine(fading());
        //Destroy(this);
    }

    public void SetTransparent() //글자를 바로 투명하게 바꿈. 초기 설정을 위해 작성됨.
    {
        text.alpha = 0;
    }

    IEnumerator fading(){   //글자의 알파값 증감 코루틴
        for(float i = text.alpha; i >= 0.02f; i -= f_fading_speed){
            text.alpha = i;
            yield return null;
        }
        text.alpha = 0;
    }

    IEnumerator rising(){
        for(float i = text.alpha; i <= 0.99f; i += f_fading_speed) {
            if(stop_rising) break;
            text.alpha = i;
            yield return null;
        }
        if(!stop_rising) text.alpha = 1.0f;
    }

    public void ShowUp(){
        StartCoroutine(rising());
    }
    public void ShowDown(){
         StartCoroutine(fading());
    }
}
