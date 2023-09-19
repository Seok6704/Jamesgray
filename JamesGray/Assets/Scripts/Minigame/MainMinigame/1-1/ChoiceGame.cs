using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChoiceGame : MonoBehaviour
{
    int Correct;
    int Fail; // 실패 횟수 기록 변수
    public AudioSource audioSrc;
    public GameObject Dialog;
    bool isClear; //성공여부
    void Start()
    {
        Correct = Random.Range(0,5);
        Fail = 0;
        isClear = false; //성공여부
    }

    public void Choice_Car()
    {
        if(Correct == 4)
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 2);
            isClear = true;
            Invoke("SceneChanger", 5f);
        }
        else
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 1);
            Fail = Fail + 1;
            if(Fail > 2)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 3);
                Invoke("SceneChanger", 5f);
            }
        }
    }
    

    public void Choice_Subway()
    {
        if(Correct == 3)
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 2);
            isClear = true;
            Invoke("SceneChanger", 5f);
        }
        else
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 1);
            Fail = Fail + 1;
            if(Fail > 2)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 3);
                Invoke("SceneChanger", 5f);
            }
        }
    }

    public void Choice_Ambulance()
    {
        if(Correct == 2)
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 2);
            isClear = true;
            Invoke("SceneChanger", 5f);
        }
        else
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 1);
            Fail = Fail + 1;
            if(Fail > 2)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 3);
                Invoke("SceneChanger", 5f);
            }
        }
    }

    public void Choice_Phone()
    {
        if(Correct == 1)
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 2);
            isClear = true;
            Invoke("SceneChanger", 5f);
        }
        else
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 1);
            Fail = Fail + 1;
            if(Fail > 2)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 3);
                Invoke("SceneChanger", 5f);
            }
        }
    }

    public void Choice_Door()
    {
        if(Correct == 0)
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 2);
            isClear = true;
            Invoke("SceneChanger", 5f);
        }
        else
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 1);
            Fail = Fail + 1;
            if(Fail > 2)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 3);
                Invoke("SceneChanger", 5f);
            }
        }
    }

    public void Btn_Quest_Click()
    {
        if(Correct == 0)
        {
            if(audioSrc.isPlaying)
            {
               audioSrc.Stop();
            }
            AudioClip clip = Resources.Load("Sounds/Minigame/1-1/Knock") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
        if(Correct == 1)
        {
            if(audioSrc.isPlaying)
            {
               audioSrc.Stop();
            }
            AudioClip clip = Resources.Load("Sounds/Minigame/1-1/Alarm") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
        if(Correct == 2)
        {
            if(audioSrc.isPlaying)
            {
               audioSrc.Stop();
            }
            AudioClip clip = Resources.Load("Sounds/Minigame/1-1/Ambulance") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
        if(Correct == 3)
        {
            if(audioSrc.isPlaying)
            {
               audioSrc.Stop();
            }
            AudioClip clip = Resources.Load("Sounds/Minigame/1-1/Subway") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
        if(Correct == 4)
        {
            if(audioSrc.isPlaying)
            {
               audioSrc.Stop();
            }
            AudioClip clip = Resources.Load("Sounds/Minigame/1-1/Car") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
    }

    void SceneChanger() //씬 전환 함수
    {
        //SceneManager.LoadScene("Chapter0");
        SceneManager.UnloadSceneAsync(gameObject.scene);    //현재 씬 종료
        SceneManager.SetActiveScene(LoadingScene.preScene); //기억하고 있던 이전 씬을 액티브로 전환
        
        GameObject[] objects = SceneManager.GetActiveScene().GetRootGameObjects();

        for(int i = 0; i < objects.Length; i++) //scene 매니저 찾기
        {
            if(objects[i].name == "SceneManager" || objects[i].name == "Scene Manager")
            {
                objects[i].GetComponent<SceneController>().AdditiveEnded(isClear);
                break;
            }
        }
    }
}
