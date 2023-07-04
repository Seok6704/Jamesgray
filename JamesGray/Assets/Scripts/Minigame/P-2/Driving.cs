using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Driving : MonoBehaviour
{
    public Animator anim_H; //핸들 애니메이션 변수
    public Animator anim_B; // 배경 애니메이션 변수
    public bool isClear; // 미니게임 클리어 여부 체크
    public AudioSource audioSrc;
    float time;
    int count;
    bool flag;
    bool if_flag;
    bool is_right;
    int ram; // 랜덤 변수

    void Start()
    {
        anim_H = GameObject.Find("Handle").GetComponent<Animator>();
        anim_B = GameObject.Find("Background").GetComponent<Animator>();
        count = 0;
        ram = Random.Range(0,2);
        flag = false;
        if_flag = true;
    }

    void Update()
    {
        if(flag) time += Time.deltaTime;
        if(time >= 3 && count == 0 && if_flag)
        {
            way_reset();
        }
        if(time >=10 && count == 0 && if_flag == false)
        {
            Game_Fail();
        }
        if(time >= 10 && count == 1 && if_flag)
        {
            way_reset();
        }
        if(time >=17 && count == 1 && if_flag == false)
        {
            Game_Fail();
        }
        if(time >= 17 && count == 2 && if_flag)
        {
            way_reset();
        }
        if(count == 3 && if_flag)
        {
            if_flag = false;
            if(audioSrc.isPlaying)
            {
                audioSrc.Stop();
            }
            AudioClip clip = Resources.Load("Sounds/Minigame/P-2/Goal") as AudioClip;
            audioSrc.PlayOneShot(clip);
            Invoke("scene_change", 6f);
        }
    }

    public void Btn_L_Click()
    {
        anim_H.SetTrigger("Btn_L_Click");
        anim_B.SetTrigger("Btn_L_Click");
        count = count + 1;
        if_flag = true;
        if(is_right)
        {
            Game_Fail();
            if_flag = false;
        }
    }

    public void Btn_R_Click()
    {
        anim_H.SetTrigger("Btn_R_Click");
        anim_B.SetTrigger("Btn_R_Click");
        count = count + 1;
        if_flag = true;
        if(is_right == false)
        {
            Game_Fail();
            if_flag = false;
        }
    }

    void Nav_On(int ramdom)
    {
        if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
        if(ramdom == 0)
        {
            AudioClip clip = Resources.Load("Sounds/Minigame/P-2/Left") as AudioClip;
            audioSrc.PlayOneShot(clip);
            is_right = false;
        }
        if(ramdom == 1)
        {
            AudioClip clip = Resources.Load("Sounds/Minigame/P-2/Right") as AudioClip;
            audioSrc.PlayOneShot(clip);
            is_right = true;
        }
    }

    void Scene_Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void On_flag()
    {
        flag = true;
    }

    void way_reset()
    {
        is_right = false;
        Nav_On(ram);
        ram = Random.Range(0, 2);
        if_flag = false;
    }

    void Game_Fail()
    {
        if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
        AudioClip clip = Resources.Load("Sounds/Minigame/P-2/Fail") as AudioClip;
        audioSrc.PlayOneShot(clip);
        count = 10;
        Invoke("Scene_Restart", 5f);
    }

    void scene_change()
    {
        SceneManager.LoadScene("Chapter0");
    }
}
