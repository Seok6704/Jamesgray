using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Setting : MonoBehaviour
{
	GameObject Setting;

	private void Start()
	{
		Setting = GameObject.Find("Setting");	//Setting이름의 오브젝트 검색
	}
	public void OnClickSetting()
	{ 
		Vector3 mid = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);	//카메라 정중앙 위치 계산
		Setting.transform.position = mid;		//카메라 중앙으로 Setting창 이동
		Debug.Log("Setting");
	}
}
