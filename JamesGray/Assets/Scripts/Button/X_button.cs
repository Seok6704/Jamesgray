using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X_button : MonoBehaviour           //Setting창의 X버튼에 대한 동작 구현
{

    Vector3 pos;
    GameObject Setting;
    void Start()
    {
        Setting = GameObject.Find("Setting");   //Setting 오브젝트 찾기
        pos = Setting.transform.position;
    }

    public void OnClickX()                      //X 버튼 눌렸을시 원 위치로 이동
	{
        Setting.transform.position = pos;
        Debug.Log("X");
	}
    
}
