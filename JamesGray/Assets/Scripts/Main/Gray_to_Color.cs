using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
    코루틴을 통해 잠시 기다렸다가 쉐이더를 끄도록 하는 기능 구현
    (Panel 스크립트)추상화를 위해 따로 분리한 스크립트
*/
public class Gray_to_Color : MonoBehaviour
{
    public UnityEvent waitDone;

    public void SetColor(){
        Debug.Log("SetCOlor");
        StartCoroutine(Wait());
    }
    IEnumerator Wait() {
        for(int i = 0; i < 700; i++) {
            yield return null;
        }
        waitDone.Invoke();
    }
}
