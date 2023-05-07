using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Press : MonoBehaviour
{
    public UnityEvent event_started;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PressAnyKeytoStart");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown || Input.touchCount > 0) 
        {
            Debug.Log("KeyDown");
            event_started.Invoke();
            Destroy(this);
        }
    }
}
