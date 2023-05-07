using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Backward : MonoBehaviour
{
	public UnityEvent moveBackward;

	public void OnClickBackward()
	{
		moveBackward.Invoke();
	}
}
