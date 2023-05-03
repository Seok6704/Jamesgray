using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    UI Image 오브젝트를 부모의 정 가운데로 이동시키는 스크립트
*/
public class UI_Mover : MonoBehaviour
{
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    public void SetPos2Parent(){
        transform.position = transform.parent.position;
    }

    public void Set2ReturnPos(){
        transform.position = pos;
    }
}
