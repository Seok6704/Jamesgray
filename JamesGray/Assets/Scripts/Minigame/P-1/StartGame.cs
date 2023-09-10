using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject Dialog;
    Prologue prologue;
    bool isClear;
    bool isChoose = false;

    void Start()
    {
        prologue = GameObject.Find("Player").GetComponent<Prologue>();
    }

    public void BtnYesClick()
    {
        if(isChoose) return;
        isChoose = true;
        isClear = true;
        Dialog.GetComponent<DialoguesManager>().SetDialogue(850, 1);
        prologue.isClear = true;
        Invoke("SceneChanger", 2f);
    }

    public void BtnNoClick()
    {
        if(isChoose) return;
        isChoose = true;
        isClear = false;
        Dialog.GetComponent<DialoguesManager>().SetDialogue(850, 2);
        Invoke("SceneChanger", 2f);
    }

    void SceneChanger() //씬 전환 함수
    {
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
