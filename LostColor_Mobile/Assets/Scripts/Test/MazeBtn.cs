using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeBtn : MonoBehaviour
{
	string Scene = "Maze";
	public void OnClickPlay()
	{
		Debug.Log("Blind Maze");
		PlayerPrefs.SetString("Previous", "PlayScene"); //다음 씬으로 넘어가기 전에 이전 씬이 무엇인지 저장 //추후 수정
		SceneManager.LoadScene(Scene);
	}
}
