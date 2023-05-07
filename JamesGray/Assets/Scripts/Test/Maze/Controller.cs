using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
	public Button forward;

	public void DisableForward()
	{
		forward.gameObject.SetActive(false);
	}
	
	//able
	public void AbleForward()
	{
		forward.gameObject.SetActive(true);
	}
}
