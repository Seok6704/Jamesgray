using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prisonController : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GameObject.Find("Prison").GetComponent<Animator>();
    }

    public void UnlockPrison()
    {
        if(UnlockGame.isClear)
        {
            anim.SetBool("isUnlock", true);
        }
    }

    public void DestroyPrison()
    {
        Destroy(GameObject.Find("Prison"));
    }
}
