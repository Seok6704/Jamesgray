using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DadFightGame : MonoBehaviour
{
    public GameObject Dialog;
    bool left, right, up, down; // 패턴 회피용 변수
    int ran;
    bool phase2;
    int dadHp = 10;
    int myHp = 3;

    public void OnStart()
    {
        DadAttack();
    }

    public void DadAttack()
    {
        if(myHp <= 0)
        {
            Debug.Log("게임 오버");
            return;
        }
        if(dadHp <= 5) phase2 = true;
        ran = Random.Range(0, 4);
        switch (ran)
        {
            case 0:
                AttackVoice(ran);
                break;
            case 1:
                AttackVoice(ran);
                break;
            case 2:
                AttackVoice(ran);
                break;
            case 3:
                AttackVoice(ran);
                break;
        }
    }

    void AttackVoice(int num)
    {
        Dialog.GetComponent<DialoguesManager>().SetDialogue(803, num);
        Debug.Log("문제 : " + num);
        BtnActive();
        if(!phase2) StartCoroutine(RoseVoice(num));
        StartCoroutine(OnAttackEffect(num));
    }

    IEnumerator RoseVoice(int num)
    {
        Dialog.GetComponent<DialoguesManager>().SetDialogue(800, num);
        yield break;
    }

    IEnumerator OnAttackEffect(int num)
    {
        yield return new WaitForSecondsRealtime(2f);
        BtnDeActive();
        GameObject.Find("Attack").transform.GetChild(num).gameObject.SetActive(true);
        GameObject.Find("Attack").transform.GetChild(num).gameObject.GetComponent<Animator>().SetTrigger("OnAttack");
    }

    void BtnActive()
    {
        GameObject.Find("Panel_Minigame").transform.GetChild(1).gameObject.SetActive(true);
        left = false;
        right = false;
        up = false;
        down = false;
    }

    void BtnDeActive()
    {
        GameObject.Find("Panel_Minigame").transform.GetChild(1).gameObject.SetActive(false);
    }

    public void isDamage()
    {
        switch (ran)
        {
            case 0:
                if(down) dadHp--;
                else myHp--;
                break;
            case 1:
                if(up) dadHp--;
                else myHp--;
                break;
            case 2:
                if(left) dadHp--;
                else myHp--;
                break;
            case 3:
                if(right) dadHp--;
                else myHp--;
                break;
        }
        Debug.Log("아버지 체력 : " + dadHp);
        Debug.Log("내 체력 : " + myHp);
    }

    public void BtnClick()
    {
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Left":
                left = true;
                BtnDeActive();
                break;
            case "Up":
                up = true;
                BtnDeActive();
                break;
            case "Right":
                right = true;
                BtnDeActive();
                break;
            case "Down":
                down = true;
                BtnDeActive();
                break;
        }
    }
}
