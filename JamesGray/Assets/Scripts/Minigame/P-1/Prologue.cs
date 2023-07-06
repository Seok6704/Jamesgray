using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prologue : MonoBehaviour
{
    GameObject kid;
    GameObject Dialog;
    Vector3 pos;
    bool flag = true;


    void Start()
    {
        kid = GameObject.Find("Rose");
        Dialog = GameObject.Find("Panel_Dialog");
    }


    void Update()
    {
        pos = kid.transform.position;
        if( pos.y >= -2.5 && flag ) 
        {
            kid.GetComponent<NPCManager>().OnAction();
            Dialog.GetComponent<UI_Mover>().SetPos2Parent();
            flag = false;
        }
    }
}
