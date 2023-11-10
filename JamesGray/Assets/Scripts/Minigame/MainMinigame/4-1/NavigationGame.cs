using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class NavigationGame : MonoBehaviour
{
    public FixedFollowCamera cam;
    int round = 0;
    GameObject Dialog;
    PlayerController_v3 pc;

    void Start()
    {
        cam.SetForceFollow(true);
        Dialog = GameObject.Find("Panel_Dialog");
        pc = GameObject.Find("Player").GetComponent<PlayerController_v3>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.name)
        {
            case "DB" :
                break;
            case "JS" :
                break;
            case "GS" :
                break;
            case "SG" :
                break;
            case "UR" :
                break;
            case "BB" :
                if(round == 0) round++;
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 1);
                break;
            case "SS" :
                if(round == 1) round++;
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 2);
                break;
            case "GSA" :
                if(round == 2) round++;
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 3);
                break;
            case "SGA" :
                if(round == 3) round++;
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 4);
                break; 
            case "URA" :
                if(round == 4) round++;
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 5);
                break;
        }
    }
}
