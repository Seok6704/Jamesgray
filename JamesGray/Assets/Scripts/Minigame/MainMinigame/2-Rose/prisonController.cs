using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prisonController : MonoBehaviour
{
    Animator anim;
    Renderer rose;

    void Start()
    {
        anim = GameObject.Find("Prison").GetComponent<Animator>();
        rose = GameObject.Find("Rose").GetComponent<Renderer>();
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
