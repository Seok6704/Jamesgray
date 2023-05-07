using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Yes : MonoBehaviour
{
	public void OnClickYes()
	{
		string temp = PlayerPrefs.GetString("Previous");    //이전 씬 이름 가져오기
		Debug.Log(temp + "!");
		if (temp == "") Debug.Log("Key is not found!");     //Key값과 일치하는 것이 없다면 Default값으로 ""이 리턴되므로 오류 코드 발생
		else
		{
			PlayerPrefs.DeleteKey("Previous");              //이전 씬 이름 삭제
			PlayerPrefs.SetString("Previous", "SampleScene");
			SceneManager.LoadScene(temp);                   //이전 씬으로 전환
		}
	}
}
