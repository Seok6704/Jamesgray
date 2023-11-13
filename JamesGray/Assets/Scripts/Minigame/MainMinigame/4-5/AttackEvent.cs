using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent attackDone;
    bool isHit;

    public void AttackDone()
    {
        if(!isHit) 
        {
            DadFightGame.dadHp--;
            DadFightGame.nDadHp.value = DadFightGame.dadHp;
        }
        attackDone.Invoke();
        this.gameObject.SetActive(false);
        isHit = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isHit = true;
        DadFightGame.isDamage();
    }
}
