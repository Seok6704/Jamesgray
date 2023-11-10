using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChapterFade : MonoBehaviour
{
    public TextMeshProUGUI text;
    GameObject panel;
    PlayerController_v3 pc;
    public UnityEngine.Events.UnityEvent fadeDone;

    void Start()
    {
        panel = GameObject.Find("Panel_Chapter");
        pc = GameObject.Find("Player").GetComponent<PlayerController_v3>();
        pc.ChangeisOn();
        StartCoroutine("OnFadeOut");
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
        StartCoroutine("OnFadein");
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
