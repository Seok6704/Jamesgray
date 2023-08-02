using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    Animator ele0, ele1, ele2, james;

    void Start()
    {
        ele0 = GameObject.Find("Elevator0").GetComponent<Animator>();
        ele1 = GameObject.Find("Elevator1").GetComponent<Animator>();
        ele2 = GameObject.Find("Elevator2").GetComponent<Animator>();
        james = GameObject.Find("James").GetComponent<Animator>();
    }

    public void LeftElevatorClick()
    {
        james.SetTrigger("LeftClick");
        StartCoroutine(eleTrigger(ele0));
    }

    public void RightElevatorClick()
    {
        james.SetTrigger("RightClick");
        StartCoroutine(eleTrigger(ele2));
    }

    public void MiddleElevatorClick()
    {
        james.SetTrigger("MiddleClick");
        StartCoroutine(eleTrigger(ele1));
    }

    IEnumerator eleTrigger(Animator anim)
    {
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("Open");
    }
}
