using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoseAlone : MonoBehaviour
{
    GameObject Dialog;
    PlayerController_v3 pc;
    bool flag;

    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController_v3>();
        Dialog = GameObject.Find("Panel_Dialog");
        Invoke("OnRoseDialogue", 1f);
    }

    void OnRoseDialogue()
    {
        flag = true;
        pc.ChangeisOn();
        Dialog.GetComponent<DialoguesManager>().SetDialogue(800, 0);
        Dialog.GetComponent<UI_Mover>().SetPos2Parent();
    }

    public void OffRoseDialouge()
    {
        if(flag) 
        {
            flag = false;
            pc.ChangeisOn();
        }
        else return;
    }

}