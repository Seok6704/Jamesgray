using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Forward : MonoBehaviour
{
    public UnityEvent moveForward;

    public void OnClickForward()
	{
		moveForward.Invoke();
	}
}
