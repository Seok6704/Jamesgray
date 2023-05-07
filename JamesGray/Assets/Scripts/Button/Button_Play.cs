using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Play : MonoBehaviour
{
	string Scene = "PlayScene";
    public void OnClickPlay()
	{
		Debug.Log("Play");
		PlayerPrefs.SetString("Previous", "SampleScene"); //다음 씬으로 넘어가기 전에 이전 씬이 무엇인지 저장
		SceneManager.LoadScene(Scene);
	}
}
