using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WalkingGame : MonoBehaviour
{
    public int Correct;
    public int Fail;
    public int Round; //게임 라운드 변수
    public GameObject Dialog;
    public AudioSource audioSrc;
    public Animator anim;

    void Awake()
    {
        Correct = GameObject.Find("Start_Button").GetComponent<NPCManager>().i_Story; //Correct 변수에 Start_Button 오브젝트의 NPCManager 스크립트 안에 i_Stroy 변수 값 저장
        if(Correct == 4) Correct = 3;
        Fail = 0;
        Round = 0;
        anim = GameObject.Find("James").GetComponent<Animator>();
    }

    public void Btn_R_S_Click()
    {
        if(Correct == 0 && Round == 0 || Correct == 3 && Round == 1 || Correct == 2 && Round == 2)
        {
            Correct_Sound();
        }
        else
        {
            Incorrect_Sound();
        }
    }

    public void Btn_L_S_Click()
    {
        if(Correct == 1 && Round == 0 || Correct == 2 && Round == 1 || Correct == 3 && Round == 2)
        {
            Correct_Sound();
        }
        else
        {
            Incorrect_Sound();
        }
    }

    public void Btn_U_S_Click()
    {
        if(Correct == 2 && Round == 0 || Correct == 1 && Round == 1 || Correct == 1 && Round == 2)
        {
            Correct_Sound();
        }
        else
        {
            Incorrect_Sound();
        }
    }

    public void Btn_D_S_Click()
    {
        if(Correct == 3 && Round == 0 || Correct == 0 && Round == 1 || Correct == 0 && Round == 2)
        {
            Correct_Sound();
        }
        else
        {
            Incorrect_Sound();
        }
    }

    public void Btn_R_Click()
    {
        if(Correct == 0 && Round == 0 || Correct == 3 && Round == 1 || Correct == 2 && Round == 2)
        {
            anim.SetTrigger("Btn_R_Click");
            Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 8);
            Round = Round + 1;
            if(Round == 3)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 6);
                Invoke("SceneChanger", 8f);
            }
        }
        else
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 5);
            Fail = Fail + 1;
            if(Fail > 2)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 7);
                Invoke("SceneChanger", 5f);
            }            
        }
    }

    public void Btn_L_Click()
    {
        if(Correct == 1 && Round == 0 || Correct == 2 && Round == 1 || Correct == 3 && Round == 2)
        {
            anim.SetTrigger("Btn_L_Click");
            Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 8);
            Round = Round + 1;
            if(Round == 3)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 6);
                Invoke("SceneChanger", 8f);
            }
        }
        else
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 5);
            Fail = Fail + 1;
            if(Fail > 2)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 7);
                Invoke("SceneChanger", 5f);
            }  
        }
    }

    public void Btn_U_Click()
    {
        if(Correct == 2 && Round == 0 || Correct == 1 && Round == 1 || Correct == 1 && Round == 2)
        {
            anim.SetTrigger("Btn_U_Click");
            Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 8);
            Round = Round + 1;
            if(Round == 3)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 6);
                Invoke("SceneChanger", 8f);
            }
        }
        else
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 5);
            Fail = Fail + 1;
            if(Fail > 2)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 7);
                Invoke("SceneChanger", 5f);
            }  
        }
    }

    public void Btn_D_Click()
    {
        if(Correct == 3 && Round == 0 || Correct == 0 && Round == 1 || Correct == 0 && Round == 2)
        {
            anim.SetTrigger("Btn_D_Click");
            Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 8);
            Round = Round + 1;
            if(Round == 3)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 6);
                Invoke("SceneChanger", 8f);
            }
        }
        else
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 5);
            Fail = Fail + 1;
            if(Fail > 2)
            {
                Dialog.GetComponent<DialoguesManager>().SetDialogue(902, 7);
                Invoke("SceneChanger", 5f);
            }  
        }
    }

    void Correct_Sound()
    {
        if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
        AudioClip clip = Resources.Load("Sounds/Minigame/1-2/Correct") as AudioClip;
        audioSrc.PlayOneShot(clip);
    }

    void Incorrect_Sound()
    {
        if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
        AudioClip clip = Resources.Load("Sounds/Minigame/1-2/Incorrect") as AudioClip;
        audioSrc.PlayOneShot(clip);
    }

    void SceneChanger() //씬 전환 함수
    {
        //SceneManager.LoadScene("Chapter0");
        SceneManager.UnloadSceneAsync(gameObject.scene);    //현재 씬 종료
        SceneManager.SetActiveScene(LoadingScene.preScene); //기억하고 있던 이전 씬을 액티브로 전환
        
        GameObject[] objects = SceneManager.GetActiveScene().GetRootGameObjects();

        for(int i = 0; i < objects.Length; i++)
        {
            if(objects[i].name == "SceneManager" || objects[i].name == "Scene Manager")
            {
                objects[i].GetComponent<SceneController>().AdditiveEnded();
                break;
            }
        }
    }
}
