using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class Chapter3Top : MonoBehaviour
{
    public GameObject dad;
    public Image attack;
    public Image fade;
    GameObject Dialog;
    GameObject attackPanel;
    GameObject fadePanel;
    PlayerController_v3 pc;
    Transform jamesPos;
    Transform rosePos;
    Vector3 targetJ = new Vector3 (0, -3.74f, 0);
    Vector3 targetR = new Vector3 (1.06f, -3.97f, 0);
    bool flag = true;

    void Start()
    {
        Dialog = GameObject.Find("Panel_Dialog");
        attackPanel = GameObject.Find("Panel_Attack");
        fadePanel = GameObject.Find("Panel_Fade");
        pc = GameObject.Find("Player").GetComponent<PlayerController_v3>();
        rosePos = GameObject.Find("Rose").transform;
    }

    public void ShowDad()
    {
        if(CoreTutorial.isClear2 && flag)
        {
            jamesPos = GameObject.Find("Player").transform;
            pc.ChangeisOn();
            dad.SetActive(true);
            Dialog.GetComponent<DialoguesManager>().SetDialogue(802, 0);
            Dialog.GetComponent<UI_Mover>().SetPos2Parent();
            flag = false;
            StartCoroutine("AttackEffect");
        }
    }

    IEnumerator AttackEffect()
    {
        attack.color = new Color(attack.color.r, attack.color.g, attack.color.b, 0);
        attackPanel.GetComponent<UI_Mover>().SetPos2Parent();
        float fadeCount = 0;
        while(fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.02f);
            attack.color = new Color(attack.color.r, attack.color.g, attack.color.b, fadeCount);
        }
        attackPanel.GetComponent<UI_Mover>().Set2ReturnPos();
        Dialog.GetComponent<UI_Mover>().Set2ReturnPos();
        StartCoroutine("OnKnockBack");
    }

    IEnumerator OnKnockBack()
    {
        while(jamesPos.position.y != targetJ.y)
        {
            yield return new WaitForSeconds(0.01f);
            jamesPos.position = Vector3.MoveTowards(jamesPos.position, targetJ, 20f * Time.deltaTime);
            rosePos.position = Vector3.MoveTowards(rosePos.position, targetR, 20f * Time.deltaTime);
        }
        Dialog.GetComponent<DialoguesManager>().SetDialogue(800, 2);
        Dialog.GetComponent<UI_Mover>().SetPos2Parent();
        StartCoroutine("OnFadeOut");
    }

    IEnumerator OnFadeOut()
    {
        fadePanel.GetComponent<UI_Mover>().SetPos2Parent();
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0);
        float fadeCount = 0;
        while(fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.02f);
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fadeCount);
        }
        Invoke("InvokeFadein", 3f);
    }

    void InvokeFadein()
    {
        StartCoroutine("OnFadein");
    }

    IEnumerator OnFadein()
    {
        dad.SetActive(false);
        Dialog.GetComponent<DialoguesManager>().SetDialogue(800, 3);
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1);
        float fadeCount = 1;
        while(fadeCount > 0.0f)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.02f);
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fadeCount);
        }
        fadePanel.GetComponent<UI_Mover>().Set2ReturnPos();
    }
}
