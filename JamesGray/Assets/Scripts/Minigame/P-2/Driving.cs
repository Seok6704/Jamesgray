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
    int count; // 정답 횟수
    bool flag; // 타이머 시작 타이밍 용 변수
    bool if_flag; // if문 무한 반복 방지용 변수
    bool is_right; // 왼쪽, 오른쪽 정답 감지용 변수
    int ram; // 랜덤 변수
    GameObject left; // 왼쪽 화살표
    GameObject right; // 오른쪽 화살표

    void Start()
    {
        anim_H = GameObject.Find("Handle").GetComponent<Animator>();
        anim_B = GameObject.Find("Background").GetComponent<Animator>();
        left = GameObject.Find("Left");
        right = GameObject.Find("Right");
        left.SetActive(false);
        right.SetActive(false);
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

    public void Btn_L_Click() // 좌회전 버튼 함수
    {
        anim_H.SetTrigger("Btn_L_Click");
        anim_B.SetTrigger("Btn_L_Click");
        count = count + 1;
        left.SetActive(false);
        right.SetActive(false);
        if_flag = true;
        if(is_right)
        {
            Game_Fail();
            if_flag = false;
        }
    }

    public void Btn_R_Click() // 우회전 버튼 함수
    {
        anim_H.SetTrigger("Btn_R_Click");
        anim_B.SetTrigger("Btn_R_Click");
        count = count + 1;
        left.SetActive(false);
        right.SetActive(false);
        if_flag = true;
        if(is_right == false)
        {
            Game_Fail();
            if_flag = false;
        }
    }

    void Nav_On(int ramdom) // 네비게이션 출력 함수
    {
        if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
        if(ramdom == 0)
        {
            if(count != 2) left.SetActive(true);
            AudioClip clip = Resources.Load("Sounds/Minigame/P-2/Left") as AudioClip;
            audioSrc.PlayOneShot(clip);
            is_right = false;
        }
        if(ramdom == 1)
        {
            if(count != 2) right.SetActive(true);
            AudioClip clip = Resources.Load("Sounds/Minigame/P-2/Right") as AudioClip;
            audioSrc.PlayOneShot(clip);
            is_right = true;
        }
    }

    void Scene_Restart() // 미니게임 실패 후, 다시 시작 함수
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void On_flag() // 시작 버튼에 들어가는 함수, 타이머 초기화 및 오디오 겹치는거 방지
    {
        flag = true;
        if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
    }

    void way_reset() // 현재 목적지 선정 함수, 네비게이션 방향 설정 및 ram 변수 값 변경
    {
        is_right = false;
        Nav_On(ram);
        ram = Random.Range(0, 2);
        if_flag = false;
    }

    void Game_Fail() // 게임 실패 함수
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
