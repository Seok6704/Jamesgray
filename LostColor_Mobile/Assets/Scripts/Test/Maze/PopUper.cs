using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopUper : MonoBehaviour	//Maze game popup 떳을때 동작 추후 수정할 예정
{
    public GameObject pop;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = pop.transform.position; 
    }

    public void OnClickYes()
	{
		string temp = PlayerPrefs.GetString("Previous");    //이전 씬 이름 가져오기
		Debug.Log(temp + "!");
		if (temp == "") Debug.Log("Key is not found!");     //Key값과 일치하는 것이 없다면 Default값으로 ""이 리턴되므로 오류 코드 발생
		else
		{
			PlayerPrefs.DeleteKey("Previous");              //이전 씬 이름 삭제
			SceneManager.LoadScene(temp);                   //이전 씬으로 전환
		}
	}
    public void OnClickNo()
	{
        pop.transform.position = pos;
	}
}
