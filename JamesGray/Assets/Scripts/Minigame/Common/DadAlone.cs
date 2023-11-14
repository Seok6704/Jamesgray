using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadAlone : MonoBehaviour
{
    GameObject Dialog;
    bool flag;

    void Start()
    {
        Dialog = GameObject.Find("Panel_Dialog");
        Invoke("OnDadDialogue", 0.2f);
    }

    void OnDadDialogue()
    {
        flag = true;
        Dialog.GetComponent<DialoguesManager>().SetDialogue(803, 0);
        Dialog.GetComponent<UI_Mover>().SetPos2Parent();
    }

    public void OffDadDialouge()
    {
        if(flag) 
        {
            flag = false;
        }
        else return;
    }
}
