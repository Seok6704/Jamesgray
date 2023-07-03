using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Driving : MonoBehaviour
{
    public Animator anim_H; //핸들 애니메이션 변수
    public Animator anim_B; // 배경 애니메이션 변수

    void Start()
    {
        anim_H = GameObject.Find("Handle").GetComponent<Animator>();
        anim_B = GameObject.Find("Background").GetComponent<Animator>();
    }

    public void Btn_L_Click()
    {
        anim_H.SetTrigger("Btn_L_Click");
        anim_B.SetTrigger("Btn_L_Click");
    }

    public void Btn_R_Click()
    {
        anim_H.SetTrigger("Btn_R_Click");
        anim_B.SetTrigger("Btn_R_Click");
    }
}
