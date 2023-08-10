using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
    SceneController 스크립트는 LoadingScene 스크립트를 불러와서 로딩 씬으로 전환 후 다음 씬으로 넘어가는 동작을 할 수 있도록 해주는 스크립트입니다.
    로딩 씬으로 전환하는 이유는 메모리 GC의 동작 순서 때문입니다. 자세한 내용은 검색 혹은 질문해주세요.
    
    Function : 
                public void LoadNextScene(string nextSceneName, float waitTime = 1f, bool isAdditive = false) //로딩 씬을 호추하기 전에 일정시간 대기하도록 하는 함수.
                waitTime은 초 단위(아마도) 기본값은 1초로 설정되어 있음.
                isAdditive 는 씬을 Additive로 불러올지 여부 기본적으로는 Async로 불러와짐

                예시, 만약 어떤 씬에서 챕터 0을 불러오고 싶다면 어떤 씬의 종료 부분에서 다음코드를 작성한다. (어떠한 객체가 SceneController를 컴포넌트로 가지고 있어야할 것이다. 아마도)
                    (SceneController를 가진 어떤 객체).GetComponent<SceneController>().LoadNextScene("Chapter0");
*/
public class SceneController : MonoBehaviour
{   
    public delegate void OnMiniGameEnd(bool isSuccessful);
    OnMiniGameEnd onMGE;
    //public UnityEvent End;  //Additive 씬이 종료되었을때 이벤트, 다이얼로그에서 기존에 있던 다이얼로그를 지우기 위해 사용

    //이벤트 시스템과 리스너는 한 씬에 두개 있으면 오류가 발생하므로 비활성화 해야하므로 additive로 씬을 호출할경우 비활성화해야함
    public GameObject audioListner;
    public GameObject eventSys;
    public DialoguesManager dial;

    public void LoadNextScene(string nextSceneName, float waitTime = 1f, bool isAdditive = false)
    {
        if(!isAdditive)
            StartCoroutine(Wait(nextSceneName, waitTime));
        else
            StartCoroutine(AdditivelyLoad(nextSceneName, waitTime));
    }
    private void Start() 
    {
        //델리게이트 체인 등록
        if(dial != null)
        {
           onMGE = dial.OnMiniGameEnd;
        }
    }
    /// <summary>
    /// 메인 메뉴 불러오는 함수
    /// </summary>
    public void LoadMainMenu()
    {
        LoadNextScene("MainScene");
    }
    public void WaitToLoad()        //챕터 0만 불러오는 함수이므로 다른 함수를 사용하도록 하자
    {
        StartCoroutine(Wait("Prologue", 1f));
    }

    public void AdditiveEnded(bool result)
    {
        SetEnable(true);
        onMGE(result);
        //End.Invoke();
    }

    /// <summary>
    /// 버튼으로 호출하기 쉽게 파라미터가 1개 뿐인 씬 호출 함수, additively
    /// </summary>
    public void SimpleLoadScene(string nextSceneName)
    {
        StartCoroutine(AdditivelyLoad(nextSceneName, 0f));
    }

    IEnumerator Wait(string nextSceneName, float waitTime)
    {
        /*for(int i = 0; i < 1000; i++)      
        {
            yield return null;
        }*/
        yield return new WaitForSeconds(waitTime); //바로 씬 전환 대신 일정 시간 대기한 후 로딩씬으로 넘어가도록 하는 반복문
        LoadingScene.LoadScene(nextSceneName);
    }

    IEnumerator AdditivelyLoad(string nextSceneName, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SetEnable(false);
        LoadingScene.LoadAdditive(nextSceneName);
    }

    void SetEnable(bool enable) //additive로 씬을 호출했을때, 오류를 발생시킬 여지가 있는 컴포넌트 비활성화 및 재 활성화
    {
        audioListner.GetComponent<AudioListener>().enabled = enable;
        eventSys.GetComponent<UnityEngine.EventSystems.EventSystem>().enabled = enable;
        eventSys.GetComponent<UnityEngine.EventSystems.StandaloneInputModule>().enabled = enable;
    }
}
