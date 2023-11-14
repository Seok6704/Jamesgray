using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ChapterFade : MonoBehaviour
{
    public TextMeshProUGUI text;
    GameObject panel;
    PlayerController_v3 pc;
    public UnityEngine.Events.UnityEvent fadeDone;
    bool isFirst = true;

    void Start()
    {
        isFirst = !(SAVEManager.saveSceneName == SceneManager.GetActiveScene().name);   //현재 씬과 세이브 파일의 씬 이름이 같다면 false로 오프닝을 스킵
        panel = GameObject.Find("Panel_Chapter");
        pc = GameObject.Find("Player").GetComponent<PlayerController_v3>();
        
        if(!isFirst) 
        {
            StartCoroutine("OnFadeOut");
            return;
        }
        pc.ChangeisOn();
        StartCoroutine("OnFadein");
    }
    
    IEnumerator OnFadein()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        float fadeCount = 0;
        while(fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.02f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, fadeCount);
        }
        StartCoroutine("OnFadeOut");
    }

    IEnumerator OnFadeOut()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        float fadeCount = 1;
        while(fadeCount > 0.0f)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.02f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, fadeCount);
        }
        panel.GetComponent<UI_Mover>().Set2ReturnPos();
        fadeDone.Invoke();
    }
}
