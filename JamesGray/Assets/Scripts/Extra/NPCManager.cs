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
    void Awake()
    {                  //Dialog.GetComponent<DialogManger>().SetDialog(s_NPC_Name, ID, i_Story, b_visited);
        if(ID >= 900) //미니게임 랜덤성 부여를 위해 추가 삽입. by 이석현
        {
            i_Story = Random.Range(0,5); // 모든 미니게임의 랜덤 경우의 수는 5가지로 설정. 추후 수정 가능 **미니게임은 해당 부분 원활한 동작을 위해 랜덤성이 없더라도 lineID를 다섯가지 경우 만들어 둘것**
        }
    }


    public void OnAction()
    {
        if(!isAlready)
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(ID, i_Story);
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
