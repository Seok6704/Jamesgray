using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Panel의 활성화를 관리하는 스크립트
//이벤트를 통해 UI의 페이딩 효과를 주도록 할수있음
//충분한 시간을 기다려 비활성화(setActive(false))로 인한 자식 스크립트들의 코루틴의 동작을 보장

public class Panel : MonoBehaviour
{
    //[Tooltip ("판넬이 비활성화 될때, Fadeout할 오브젝트 이벤트")]
    public UnityEvent deactivatePanel;
    //[Tooltip ("판넬이 활성화 될때, Fadein할 오브젝트 이벤트")]
    public UnityEvent activatePanel;

    [Tooltip ("Start() 실행시 Active상태 여부 체크 (판넬이 겹친상태에서 둘 다 활성화 상태면 제대로 동작안할수있음)")]
    public bool active_at_Start;    //게임 시작할때 Active여부 설정

    void Start() {
        gameObject.SetActive(active_at_Start);
    }

    public void turnOnPanel(){
        gameObject.SetActive(true);
        activatePanel.Invoke();
        //StartCoroutine(waitOn());
    }
    public void turnoffPanel(){
        deactivatePanel.Invoke();
        StartCoroutine(waitOff());
    }

    IEnumerator waitOff(){  //다른 객체들이 사라질때 까지 대기하는 코루틴
        for(int i = 0; i < 1000; i++) yield return null;
        gameObject.SetActive(false);
    }
    
    /*IEnumerator waitOn(){       //액티브가 되어있지 않으면 동작하지 않는 요류있음 쓰지말자
        for(int i = 0; i < 1000; i++) yield return null;
        //gameObject.SetActive(true);
    }*/
    
}
