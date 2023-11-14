using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DadFightGame : MonoBehaviour
{
    public GameObject Dialog;
    int ran;
    bool phase2;
    public static int dadHp;
    static int myHp;
    public static Slider nDadHp, nMyHp;
    PlayerController_v3 pc;

    void Start()
    {
        dadHp = 10;
        myHp = 5;
        nDadHp = GameObject.Find("DHpBar").GetComponent<Slider>();
        nMyHp = GameObject.Find("JHpBar").GetComponent<Slider>();
        pc = GameObject.Find("Player").GetComponent<PlayerController_v3>();
    }

    public void OnStart()
    {
        DadAttack();
    }

    public void DadAttack()
    {
        if(myHp <= 0)
        {
            pc.ChangeisOn();
            ClearAndFail.GameFail();
            Invoke("OnReset", 2f);
            return;
        }
        if(dadHp <= 5) phase2 = true;
        if(dadHp <= 0)
        {
            pc.ChangeisOn();
            ClearAndFail.GameClear();
            Invoke("EndGame", 2f);
        }
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
        Dialog.GetComponent<DialoguesManager>().SetDialogue(804, num);
        Debug.Log("문제 : " + num);
        if(!phase2) StartCoroutine(RoseVoice(num));
        StartCoroutine(OnAttackEffect(num));
    }

    IEnumerator RoseVoice(int num)
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("로즈 대사");
        Dialog.GetComponent<DialoguesManager>().SetDialogue(800, num);
    }

    IEnumerator OnAttackEffect(int num)
    {
        yield return new WaitForSecondsRealtime(6f);
        GameObject.Find("Attack").transform.GetChild(num).gameObject.SetActive(true);
    }

    public static void isDamage()
    {
        myHp--;
        nMyHp.value = myHp;
    }

    void OnReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void EndGame()
    {
        SceneManager.LoadScene("4-5(Clear)");
    }
}
