using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceGame : MonoBehaviour
{
    public int Correct;
    public GameObject Dialog;

    void Start()
    {
        Correct = GameObject.Find("Start_Button").GetComponent<NPCManager>().i_Story; //Correct 변수에 Start_Button 오브젝트의 NPCManager 스크립트 안에 i_Stroy 변수 값 저장
    }

    public void Choice_Car()
    {
        switch (Correct)
        {
            case 0:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 1:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 2:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 3:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 4:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 6);
            Invoke("SceneChanger", 5f);
            break;
        }
    }

    public void Choice_Subway()
    {
        switch (Correct)
        {
            case 0:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 1:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 2:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 3:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 6);
            Invoke("SceneChanger", 5f);
            break;
            case 4:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
        }

    }

    public void Choice_Ambulance()
    {
        switch (Correct)
        {
            case 0:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 1:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 2:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 6);
            Invoke("SceneChanger", 5f);
            break;
            case 3:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 4:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
        }

    }

    public void Choice_Phone()
    {
        switch (Correct)
        {
            case 0:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 1:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 6);
            Invoke("SceneChanger", 5f);
            break;
            case 2:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 3:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 4:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
        }

    }

    public void Choice_Door()
    {
        switch (Correct)
        {
            case 0:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 6);
            Invoke("SceneChanger", 5f);
            break;
            case 1:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 2:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 3:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
            case 4:
            Dialog.GetComponent<DialoguesManager>().SetDialogue(901, 5);
            break;
        }

    }

    void SceneChanger() //씬 전환 함수
    {
        SceneManager.LoadScene("Chapter0");
    }
}
