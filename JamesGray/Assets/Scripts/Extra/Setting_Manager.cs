using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Setting_Manager : MonoBehaviour
{
    public UnityEvent ev_Show, ev_Hide;
    public Canvas canvas;
    Vector3 pos;
    void Start(){
        pos = transform.position;
    }

    public void ShowSetting(){
        Time.timeScale = 0; //시간을 멈추기 우리 게임에도 필요할까?
        transform.position = canvas.transform.position;
        ev_Show.Invoke();
    }

    public void HideSetting() {
        Time.timeScale = 1;
        transform.position = pos;
        ev_Hide.Invoke();
    }
}
