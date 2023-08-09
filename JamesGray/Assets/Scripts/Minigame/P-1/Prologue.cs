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
    bool flag = true;
    public bool isClear = false;
    bool fade;

    void Start()
    {
        kid = GameObject.Find("Rose");
        Dialog = GameObject.Find("Panel_Dialog");
        fadePanel = GameObject.Find("Panel_Minigame");
    }

    void Update()
    {
        pos = kid.transform.position;
        if( pos.y >= -2.5 && flag ) 
        {
            kid.GetComponent<NPCManager>().OnAction();
            Dialog.GetComponent<UI_Mover>().SetPos2Parent();
            flag = false;
        }
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
        Dialog.GetComponent<DialoguesManager>().SetDialogue(899, 1);
        Dialog.GetComponent<UI_Mover>().SetPos2Parent();
    }

}
