using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorChoice : MonoBehaviour
{
    Animator james;
    public AudioSource audioSrc;
    public GameObject Dialog;
    int ram;
    bool isClear;
    bool isChoose;

    void Start()
    {
        james = GameObject.Find("James").GetComponent<Animator>();
        ram = Random.Range(0,4);
    }

    public void BtnDoor0()
    {
        if(!isChoose)
        {
            isChoose = true;
            james.SetTrigger("Door0Click");
            if(ram == 0)
            {
                isClear = true;
                Invoke("SceneChange", 2f);
            }
            else Invoke("SceneChange", 2f);
        }
    }

    public void BtnDoor1()
    {
        if(!isChoose)
        {
            isChoose = true;
            james.SetTrigger("Door1Click");
            if(ram == 1)
            {
                isClear = true;
                Invoke("SceneChange", 2f);
            }
            else Invoke("SceneChange", 2f);
        }
    }

    public void BtnDoor2()
    {
        if(!isChoose)
        {
            isChoose = true;
            james.SetTrigger("Door2Click");
            if(ram == 2)
            {
                isClear = true;
                Invoke("SceneChange", 2f);
            }
            else Invoke("SceneChange", 2f);
        }
    }

    public void BtnDoor3()
    {
        if(!isChoose)
        {
            isChoose = true;
            james.SetTrigger("Door3Click");
            if(ram == 3)
            {
                isClear = true;
                Invoke("SceneChange", 2f);
            }
            else Invoke("SceneChange", 2f);
        }
    }

    public void BtnDoorSound0()
    {
        if(!isChoose)
        {
            switch(ram)
            {
                case 0:
                    HelpSound();
                    break;
                case 1:
                    WorkerSound();
                    break;
                case 2:
                    SleepSound();
                    break;
                case 3:
                    EmergencySound();
                    break;
            }
        }
    }

    public void BtnDoorSound1()
    {
        if(!isChoose)
        {
            switch(ram)
            {
                case 0:
                    EmergencySound();
                    break;
                case 1:
                    HelpSound();
                    break;
                case 2:
                    WorkerSound();
                    break;
                case 3:
                    SleepSound();
                    break;
            }
        }
    }

    public void BtnDoorSound2()
    {
        if(!isChoose)
        {
            switch(ram)
            {
                case 0:
                    SleepSound();
                    break;
                case 1:
                    EmergencySound();
                    break;
                case 2:
                    HelpSound(); 
                    break;
                case 3:
                    WorkerSound();
                    break;
            }
        }
    }

    public void BtnDoorSound3()
    {
        if(!isChoose)
        {
            switch(ram)
            {
                case 0:
                    WorkerSound();
                    break;
                case 1:
                    SleepSound();
                    break;
                case 2:
                    EmergencySound();
                    break;
                case 3:
                    HelpSound();
                    break;
            }
        }
    }

    void HelpSound()
    {
        //아이가 도와달라고 외치는 소리 재생
        Debug.Log("HELP!");
    }

    void WorkerSound()
    {
        //타워 직원들의 잡담 소리 재생
        Debug.Log("BLAHBLAH~!");
    }

    void SleepSound()
    {
        // 코골면서 자는 사람의 소리 재생
        Debug.Log("SLEEEEEPPPPPPP~~~");
    }

    void EmergencySound()
    {
        // 기계 고장음과 다급한 목소리 재생
        Debug.Log("NO!!!!!!!");
    }

    public void BtnStartClick()
    {
         if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
    }

    void SceneChange()
    {
        if(isClear) SceneManager.LoadScene("Chapter2-Rose");
        else
        {
            Dialog.GetComponent<DialoguesManager>().SetDialogue(907, 0);
            Dialog.GetComponent<UI_Mover>().SetPos2Parent();
            Invoke("Restart", 4f);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene("2-2");
    }

}
