using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectRotate : MonoBehaviour
{
    public UnityEvent onRotate, offRotate;
    //Vector3 eulerAngel = new Vector3(0f, 0f, 1f);   //z축 기준으로 1도 회전
    bool activeRotate = false;

    public void setRotate() 
    {
        activeRotate = !activeRotate;

        if(activeRotate)
        {
            onRotate.Invoke();
        }
        else
        {
            transform.rotation = Quaternion.Euler(0,0,0);
            offRotate.Invoke();
        }
    }
    private void FixedUpdate() 
    {
        if(activeRotate)
        {
            transform.rotation *= Quaternion.Euler(Time.deltaTime * 0f, 0f, 1f);
        }
    }
}
