using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LoadingScene : MonoBehaviour
{
    static string nextScene;

    public UnityEvent loadingComplete;

    [SerializeField]
    Image loadingBar;
    AsyncOperation op;
    private void Start() 
    {
        //StartCoroutine(LoadSceneProcess());
        StartCoroutine(LoadingBar());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator WaitTouch()     //입력 대기
    {
        while(true)
        {
            if(Input.touchCount > 0 || Input.anyKeyDown) 
            {
                op.allowSceneActivation = true;
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator LoadingBar()        //좀 더 자연스러운 로딩 구현
    {
        float timer = 0f;
        op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while(!op.isDone)
        {
            loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, op.progress, 0.05f);
            yield return null;
            if(op.progress >= 0.9f)
            {
                timer += Time.unscaledDeltaTime;
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1f, timer);

                if(loadingBar.fillAmount >= 1f)
                {
                    loadingComplete.Invoke();
                    StartCoroutine(WaitTouch());
                    yield break;
                }
            }
        }
    }

    IEnumerator LoadSceneProcess()      //인터넷에서 참고한 코루틴
    {
        op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while(!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9f)
            {
                loadingBar.fillAmount = op.progress;
                //loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, op.progress, 0.1f);
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                loadingBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(loadingBar.fillAmount >= 1f)
                {
                    //op.allowSceneActivation = true;
                    loadingComplete.Invoke();
                    StartCoroutine(WaitTouch());
                    yield break;
                }
            }
        }
    }
}
