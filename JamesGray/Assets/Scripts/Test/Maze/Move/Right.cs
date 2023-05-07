using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Right : MonoBehaviour
{
	public UnityEvent moveRight;

	public void OnClickRight()
	{
		moveRight.Invoke();
	}
}
