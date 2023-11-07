using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterStart : MonoBehaviour
{
    Animator startAnim;
    GameObject Dialog;
    PlayerController_v3 pc;
    bool flag = true;
    

    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController_v3>();
        startAnim = GameObject.Find("StartNPC").GetComponent<Animator>();
        Dialog = GameObject.Find("Panel_Dialog");
    }

    public void AnimStart()
    {
        startAnim.SetBool("OnStart", true);
    }

    public void DialogStart()
    {
        Dialog.GetComponent<DialoguesManager>().SetDialogue(801, 0);
        Dialog.GetComponent<UI_Mover>().SetPos2Parent();
    }

    public void DialogOff()
    {
        if(flag) 
        {
            flag = false;
            pc.ChangeisOn();
        }
    }
}
