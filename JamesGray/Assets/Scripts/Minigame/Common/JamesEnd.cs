using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JamesEnd : MonoBehaviour
{
    GameObject Dialog;
    bool flag;

    void Start()
    {
        Dialog = GameObject.Find("Panel_Dialog");
        Invoke("OnJamesDialogue", 0.2f);
    }

    void OnJamesDialogue()
    {
        flag = true;
        Dialog.GetComponent<DialoguesManager>().SetDialogue(801, 0);
        Dialog.GetComponent<UI_Mover>().SetPos2Parent();
    }

    public void OffJamesDialouge()
    {
        if(flag) 
        {
            flag = false;
        }
        else return;
    }
}
