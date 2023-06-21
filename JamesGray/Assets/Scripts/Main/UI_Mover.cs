using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    UI Image 오브젝트를 부모의 정 가운데로 이동시키는 스크립트
*/
public class UI_Mover : MonoBehaviour
{
    public bool parentOnStart;  //시작시 캔버스 화면에 위치 (참일때)
    Vector3 pos;
    
    void Start()
    {
        pos = transform.position;

        if(parentOnStart) SetPos2Parent();  // 시작시 참이면 캔버스 중앙에 위치
    }

    public void SetPos2Parent(){
        transform.position = transform.parent.position;
    }

    public void Set2ReturnPos(){
        transform.position = pos;
    }
}
