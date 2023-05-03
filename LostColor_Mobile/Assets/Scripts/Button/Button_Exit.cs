using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Exit : MonoBehaviour
{
   public void OnClickBtn()
	{
		Application.Quit();
		Debug.Log("Exit");
	}
}
