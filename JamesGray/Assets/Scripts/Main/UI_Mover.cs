using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    UI Image 오브젝트를 부모의 정 가운데로 이동시키는 스크립트
*/
public class UI_Mover : MonoBehaviour
{
    public bool parentOnStart;  //시작시 캔버스 화면에 위치 (참일때)
    [Header("원하는 위치")]
    public GameObject showPosition;//캔버스 가운데 대신 특정 오브젝트 가운데로 가고 싶을때
    [Header("활성화 및 비활성화 이벤트")]
    public UnityEngine.Events.UnityEvent set2Parent, set2Return;    //활성화 비활성화를 알수 있게 이벤트 발생

    Vector3 pos;

    void Start()
    {
        pos = transform.position;

        if(parentOnStart) SetPos2Parent();  // 시작시 참이면 캔버스 중앙에 위치
    }

    public void SetPos2Parent()
    {
        transform.position = transform.parent.position;
        set2Parent.Invoke();
    }

    public void Set2ReturnPos()
    {
        transform.position = pos;
        set2Return.Invoke();
    }

    public void Set2ShowPosition()
    {
        if(showPosition == null) return;
        transform.position = showPosition.transform.position;
    }
}
