using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public Image img;
    GameObject panel;
    public UnityEngine.Events.UnityEvent fadeDone;

    public void OnStart()
    {
        panel = GameObject.Find("Panel_Fade");
        panel.GetComponent<UI_Mover>().SetPos2Parent();
        StartCoroutine("OnFadeOut");
    }
    
    IEnumerator OnFadeOut()
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        float fadeCount = 0;
        while(fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.02f);
            img.color = new Color(img.color.r, img.color.g, img.color.b, fadeCount);
        }
        panel.GetComponent<UI_Mover>().Set2ReturnPos();
        fadeDone.Invoke();
    }
}
