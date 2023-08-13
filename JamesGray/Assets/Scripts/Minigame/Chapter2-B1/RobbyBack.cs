using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobbyBack : MonoBehaviour
{
    GameObject Dialog;

    void Start()
    {
        Dialog = GameObject.Find("Panel_Dialog");
        Invoke("OnJamesDialogue", 1f);
    }

    void OnJamesDialogue()
    {
        Dialog.GetComponent<DialoguesManager>().SetDialogue(907, 0);
        Dialog.GetComponent<UI_Mover>().SetPos2Parent();
    }

}
