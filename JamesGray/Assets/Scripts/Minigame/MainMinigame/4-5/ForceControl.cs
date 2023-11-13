using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceControl : MonoBehaviour
{
    int count = 0;
    bool OneTime;
    bool mgStart;

    public void UpConunt()
    {
        if(QuestionGame.isClear)
        {
            count++;
            if(!OneTime && count >= 8)
            {
                GameObject.Find("Dad").GetComponent<Animator>().SetBool("DadAwake", true);
                OneTime = true;
            } 
        }
    }

    public void DownCount()
    {
        if(QuestionGame.isClear)
        {
            count--;
        }
    }

    public void OnForce()
    {
        GameObject.Find("NPC").transform.GetChild(2).gameObject.SetActive(true);
    }
}
