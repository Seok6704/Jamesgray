using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent attackDone;

    public void AttackDone()
    {
        attackDone.Invoke();
        this.gameObject.SetActive(false);
    }
}
