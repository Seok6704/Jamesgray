using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Prologue : MonoBehaviour
{
    public Image fadeimg;
    GameObject kid;
    GameObject Dialog;
    GameObject fadePanel;
    Vector3 pos;
    public bool isClear = false;
    bool fade;

    void Start()
    {
        kid = GameObject.Find("Player");
        Dialog = GameObject.Find("Panel_Dialog");
        fadePanel = GameObject.Find("Panel_Minigame");
    }

    public void OnDialogue()
    {
        if(isClear)
        {
            Invoke("RoseDialogue", 0.2f);
            isClear = false;
            fade = true;
        }
        else return;
    }

    public void OnFadeOut()
    {
        if(fade)
        {
            fade = false;
            StartCoroutine("FadeOutRoutine");
        }
        else return;
    }

    IEnumerator FadeOutRoutine()
    {
        fadePanel.GetComponent<UI_Mover>().SetPos2Parent();
        fadeimg.color = new Color(0, 0, 0, 0);
        float fadeCount = 0;
        while(fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.07f);
            fadeimg.color = new Color(0, 0, 0, fadeCount);
        }
        SceneManager.LoadScene("P-2");
    }

    void RoseDialogue()
    {
        Dialog.GetComponent<DialoguesManager>().SetDialogue(800, 1);
        Dialog.GetComponent<UI_Mover>().SetPos2Parent();
    }

}
