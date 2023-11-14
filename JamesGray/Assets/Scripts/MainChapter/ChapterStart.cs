using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterStart : MonoBehaviour
{
    Animator startAnim;
    GameObject Dialog;
    PlayerController_v3 pc;
    bool flag = true;
    bool isFirst = true;

    void Start()
    {
        isFirst = !(SAVEManager.saveSceneName == SceneManager.GetActiveScene().name);   //현재 씬과 세이브 파일의 씬 이름이 같다면 false로 오프닝을 스킵
        //if(!isFirst) return;

        pc = GameObject.Find("Player").GetComponent<PlayerController_v3>();
        startAnim = GameObject.Find("StartNPC").GetComponent<Animator>();
        Dialog = GameObject.Find("Panel_Dialog");
    }

    public void AnimStart()
    {
        if(!isFirst) return;
        
        startAnim.SetBool("OnStart", true);
    }

    public void DialogStart()
    {
        if(!isFirst) return;

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

    public void OffForce()
    {
        GameObject.Find("force").SetActive(false);
    }
}
