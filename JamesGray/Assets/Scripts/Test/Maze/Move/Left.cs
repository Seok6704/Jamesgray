using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Left : MonoBehaviour
{
	public UnityEvent moveLeft;

	public void OnClickLeft()
	{
		moveLeft.Invoke();
	}
}
