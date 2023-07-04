using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
    [Header ("대화를 불러올 판넬")]
    //public Panel talk_Panel;
    public GameObject Dialog;

    [Header ("NPC 이름")]
    public string s_NPC_Name;
    public int ID;   //npc 번호 부여
    public int i_Story;
    public bool b_visited = false;

    public bool isAlready = false;

    /*public void Onclick() {     //NPC가 선택되었을 때 이벤트로 불어올 함수
        Dialog.GetComponent<DialogManger>().SetDialog(s_NPC_Name, ID, i_Story++, b_visited);
        b_visited = true;
    }*/                
    //Dialog.GetComponent<DialogManger>().SetDialog(s_NPC_Name, ID, i_Story, b_visited);


    public void OnAction()
    {
        if(!isAlready)
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(ID, i_Story, this);
            isAlready = true;
        }
        //b_visited = true;
    }
    
    public void EndAction() //일단 임시로 비활성화됨
    {
        //i_Story++;
        //b_visited = true;
        isAlready = false;
    }

}
