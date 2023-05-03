using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class No : MonoBehaviour
{
	public GameObject pop;
	Vector3 pos;
	// Start is called before the first frame update
	void Start()
	{
		pos = pop.transform.position;
	}
	public void OnClickNo()
	{
		pop.transform.position = pos;
	}

}
