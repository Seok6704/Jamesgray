using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{
    Animator ele0, ele1, ele2, james;
    public GameObject Dialog;
    bool isClear;
    bool isChoose = false;
    int ran;

    void Start()
    {
        ele0 = GameObject.Find("Ele0").GetComponent<Animator>();
        ele1 = GameObject.Find("Ele1").GetComponent<Animator>();
        ele2 = GameObject.Find("Ele2").GetComponent<Animator>();
        james = GameObject.Find("James").GetComponent<Animator>();
        ran = Random.Range(0,3);
    }

    public void LeftElevatorClick()
    {
        if(isChoose) return;
        isChoose = true;
        james.SetTrigger("LeftClick");
        StartCoroutine(eleTrigger(ele0));
        if(ran == 0)
        {
            RobbyMoveSound();
            Invoke("SceneChanger", 3f);
        }
        else if(ran == 1)
        {
            isClear = true;
            FifthFloorMoveSound();
            Invoke("SceneChange", 3f);
        }
        else
        {
            B1FloorMoveSound();
            Invoke("SceneChange", 3f);
        }
    }

    public void RightElevatorClick()
    {
        if(isChoose) return;
        isChoose = true;
        james.SetTrigger("RightClick");
        StartCoroutine(eleTrigger(ele2));
        if(ran == 2)
        {
            RobbyMoveSound();
            Invoke("SceneChanger", 3f);
        }
        else if(ran ==0)
        {
            isClear = true;
            FifthFloorMoveSound();
            Invoke("SceneChange", 3f);
        }
        else
        {
            B1FloorMoveSound();
            Invoke("SceneChange", 3f);
        }
    }

    public void MiddleElevatorClick()
    {
        if(isChoose) return;
        isChoose = true;
        james.SetTrigger("MiddleClick");
        StartCoroutine(eleTrigger(ele1));
        if(ran == 1)
        {
            RobbyMoveSound();
            Invoke("SceneChanger", 3f);
        }
        else if(ran == 0)
        {
            B1FloorMoveSound();
            Invoke("SceneChange", 3f);
        }
        else
        {
            isClear = true;
            FifthFloorMoveSound();
            Invoke("SceneChange", 3f);
        }
    }

    IEnumerator eleTrigger(Animator anim)
    {
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("Open");
    }

    public void BtnE0Click()
    {
        if(ran == 0)
        {
            RobbySound();
        }
        else if(ran == 1)
        {
            FifthFloorSound();
        }
        else
        {
            B1FloorSound();
        }
    }
    public void BtnE1Click()
    {
        if(ran == 1)
        {
            RobbySound();
        }
        else if(ran == 0)
        {
            B1FloorSound();
        }
        else
        {
            FifthFloorSound();
        }
    }
    public void BtnE2Click()
    {
        if(ran == 2)
        {
            RobbySound();
        }
        else if(ran == 0)
        {
            FifthFloorSound();
        }
        else
        {
            B1FloorSound();
        }
    }

    void RobbySound()
    {
        Dialog.GetComponent<DialoguesManager>().SetDialogue(910, 1);
    }

    void FifthFloorSound()
    {
        Dialog.GetComponent<DialoguesManager>().SetDialogue(910, 3);
    }

    void B1FloorSound()
    {
        Dialog.GetComponent<DialoguesManager>().SetDialogue(910, 5);
    }

    void RobbyMoveSound()
    {
        isClear = false;
        Dialog.GetComponent<DialoguesManager>().SetDialogue(910, 2);
    }

    void FifthFloorMoveSound()
    {
        Dialog.GetComponent<DialoguesManager>().SetDialogue(910, 4);
    }

    void B1FloorMoveSound()
    {
        isClear = false;
        Dialog.GetComponent<DialoguesManager>().SetDialogue(910, 6);
    }

    void SceneChanger() //씬 전환 함수
    {
        SceneManager.UnloadSceneAsync(gameObject.scene);    //현재 씬 종료
        SceneManager.SetActiveScene(LoadingScene.preScene); //기억하고 있던 이전 씬을 액티브로 전환
        
        GameObject[] objects = SceneManager.GetActiveScene().GetRootGameObjects();

        for(int i = 0; i < objects.Length; i++)
        {
            if(objects[i].name == "SceneManager" || objects[i].name == "Scene Manager")
            {
                objects[i].GetComponent<SceneController>().AdditiveEnded(isClear);
                break;
            }
        }
    }

    void SceneChange()
    {
        if(isClear) SceneManager.LoadScene("Chapter2-5");
        else SceneManager.LoadScene("Chapter2-B1");
    }
}
