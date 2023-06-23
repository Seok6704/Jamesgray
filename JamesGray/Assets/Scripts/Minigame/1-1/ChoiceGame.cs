using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceGame : MonoBehaviour
{
    public int Correct;
    public int Fail; // 실패 횟수 기록 변수
    public GameObject Dialog;

    void Start()
    {
        Correct = GameObject.Find("Start_Button").GetComponent<NPCManager>().i_Story; //Correct 변수에 Start_Button 오브젝트의 NPCManager 스크립트 안에 i_Stroy 변수 값 저장
        Fail = 0;
    }

    public void Choice_Car()
    {
        if(Correct == 4)
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 6);
            Invoke("SceneChanger", 5f);
        }
        else
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            Fail = Fail + 1;
            if(Fail > 2)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 7);
                Invoke("SceneChanger", 5f);
            }
        }
    }
    

    public void Choice_Subway()
    {
        if(Correct == 3)
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 6);
            Invoke("SceneChanger", 5f);
        }
        else
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            Fail = Fail + 1;
            if(Fail > 2)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 7);
                Invoke("SceneChanger", 5f);
            }
        }
    }

    public void Choice_Ambulance()
    {
        if(Correct == 2)
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 6);
            Invoke("SceneChanger", 5f);
        }
        else
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            Fail = Fail + 1;
            if(Fail > 2)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 7);
                Invoke("SceneChanger", 5f);
            }
        }
    }

    public void Choice_Phone()
    {
        if(Correct == 1)
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 6);
            Invoke("SceneChanger", 5f);
        }
        else
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            Fail = Fail + 1;
            if(Fail > 2)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 7);
                Invoke("SceneChanger", 5f);
            }
        }
    }

    public void Choice_Door()
    {
        if(Correct == 0)
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 6);
            Invoke("SceneChanger", 5f);
        }
        else
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            Fail = Fail + 1;
            if(Fail > 2)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 7);
                Invoke("SceneChanger", 5f);
            }
        }
    }

    void SceneChanger() //씬 전환 함수
    {
        SceneManager.LoadScene("Chapter0");
    }
}
