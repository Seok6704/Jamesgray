using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Trickery : MonoBehaviour
{
    public Animator anim_C1; //컵 1 애니메이션
    public Animator anim_C2; //컵 2 애니메이션
    public Animator anim_C3; //컵 3 애니메이션
    public AudioSource audioSrc;
    public GameObject Dialog;
    GameObject Cat, Btn_S_1, Btn_S_2, Btn_S_3; // 고양이, 소리듣기 1, 2, 3 버튼 변수
    int ram; // 랜덤 변수
    int round = 0; // 라운드 체크
    int Score = 0; // 맞춘 횟수
    int fail = 0; // 틀린 횟수
    bool flag = false; // if문 반복 방지용 플래그
    bool isClear; // 승리 변수
    //int[] pos = new int[] { 1645, 1170, 695 }; // 고양이 위치 변수 각각 컵3 컵2 컵1 위치값
    List<int> pos = new List<int>();
    Vector3 C1_pos, C2_pos, C3_pos; // 컵 위치 변수
    bool isStart;

    void Start()
    {
        anim_C1 = GameObject.Find("Cup_1").GetComponent<Animator>();
        anim_C2 = GameObject.Find("Cup_2").GetComponent<Animator>();
        anim_C3 = GameObject.Find("Cup_3").GetComponent<Animator>();
        Btn_S_1 = GameObject.Find("Btn_S_1");
        Btn_S_2 = GameObject.Find("Btn_S_2");
        Btn_S_3 = GameObject.Find("Btn_S_3");
        Cat = GameObject.Find("Cat");
        ram = UnityEngine.Random.Range(0,3);
        Cat.SetActive(false);
    }

    public void OnStart()
    {
        C1_pos = GameObject.Find("Cup_1").transform.position;
        C2_pos = GameObject.Find("Cup_2").transform.position;
        C3_pos = GameObject.Find("Cup_3").transform.position;
        pos.Add((int)C3_pos.x);
        pos.Add((int)C2_pos.x);
        pos.Add((int)C1_pos.x);
        isStart = true;
    }

    void Update()
    {
        if(!isStart) return;
        if( Score > 2 && flag == false)
        {
            flag = true;
            isClear = true;
            round = 0;
            if(audioSrc.isPlaying)
            {
                audioSrc.Stop();
            }
            Dialog.GetComponent<DialoguesManager>().SetDialogue(903, 2);
            isClear = true;
            ClearAndFail.GameClear();
            Invoke("SceneChanger", 5f);
        }
        if (fail > 1 && flag == false)
        {
            flag = true;
            isClear = false;
            round = 0;
            if(audioSrc.isPlaying)
            {
                audioSrc.Stop();
            }
            Dialog.GetComponent<DialoguesManager>().SetDialogue(903, 1);
            isClear = false;
            ClearAndFail.GameFail();
            Invoke("SceneChanger", 5f);
        }
        if(pos[ram] == C3_pos.x && round >= 1 && flag == true)
        {
            Cat.transform.position = C3_pos;
        }
        if(pos[ram] == C2_pos.x && round >= 1 && flag == true)
        {
            Cat.transform.position = C2_pos;
        }
        if(pos[ram] == C1_pos.x && round >= 1 && flag == true)
        {
            Cat.transform.position =C1_pos;
        }
        if(round >= 1 && flag == false)
        {
            flag = true;
            Invoke("Open_Cup", 3f);
            Invoke("Shuffle", 6f);
        }
    }

    void Shuffle() //컵 섞기
    {
        Cat.SetActive(false);
        anim_C1.SetTrigger("Cup_Shuffle");
        anim_C2.SetTrigger("Cup_Shuffle");
        anim_C3.SetTrigger("Cup_Shuffle");
        round = round + 1;
        ram = UnityEngine.Random.Range(0,3);
        Invoke("Btn_Active",2f);
    }

    void Open_Cup() // 컵 전체 공개
    {
        Cat.SetActive(true);
        Btn_DeActive();   
        anim_C1.SetTrigger("Cup_Open");
        anim_C2.SetTrigger("Cup_Open");
        anim_C3.SetTrigger("Cup_Open");
    }

    public void Choice_Cup_1() // 플레이어가 고른 컵 열기
    {
        Cat.SetActive(true);
        anim_C1.SetTrigger("Cup_Open");
        if(Cat.transform.position.x == C1_pos.x) Score = Score + 1;
        else fail = fail + 1;
        flag = false; 
    }
    public void Choice_Cup_2()
    {
        Cat.SetActive(true);
        anim_C2.SetTrigger("Cup_Open");
        if(Cat.transform.position.x == C2_pos.x) Score = Score + 1;
        else fail = fail + 1; 
        flag = false; 
    }
    public void Choice_Cup_3()
    {
        Cat.SetActive(true);
        anim_C3.SetTrigger("Cup_Open");
        if(Cat.transform.position.x == C3_pos.x) Score = Score + 1;
        else fail = fail + 1; 
        flag = false; 
    }

    public void Btn_Start_Click() // 첫 시작 버튼 클릭
    {
        flag = true;
        Invoke("Open_Cup", 0.5f);
        Invoke("Shuffle", 3f);
        if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
    }

    public void Btn_S_1_Click() // 컵 1 소리듣기
    {
        if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
        if(Cat.transform.position.x == C1_pos.x)
        {
            AudioClip clip = Resources.Load("Sounds/Minigame/1-S-2/Cat") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
        else
        {
            AudioClip clip = Resources.Load("Sounds/Minigame/1-S-2/Dog") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
    }
    public void Btn_S_2_Click() // 컵 2 소리듣기
    {
        if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
        if(Cat.transform.position.x == C2_pos.x)
        {
            AudioClip clip = Resources.Load("Sounds/Minigame/1-S-2/Cat") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
        else
        {
            AudioClip clip = Resources.Load("Sounds/Minigame/1-S-2/Dog") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
    }
    public void Btn_S_3_Click() // 컵 3 소리듣기
    {
        if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
        if(Cat.transform.position.x == C3_pos.x)
        {
            AudioClip clip = Resources.Load("Sounds/Minigame/1-S-2/Cat") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
        else
        {
            AudioClip clip = Resources.Load("Sounds/Minigame/1-S-2/Dog") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
    }

    void Btn_Active() // 소리듣기 버튼 활성화
    {
        Btn_S_1.SetActive(true);
        Btn_S_2.SetActive(true);
        Btn_S_3.SetActive(true);
    }
    void Btn_DeActive() // 소리듣기 버튼 비활성화
    {
        Btn_S_1.SetActive(false);
        Btn_S_2.SetActive(false);
        Btn_S_3.SetActive(false);
    }

    void SceneChanger() //씬 전환 함수
    {
        SceneManager.UnloadSceneAsync(gameObject.scene);    //현재 씬 종료
        SceneManager.SetActiveScene(LoadingScene.preScene); //기억하고 있던 이전 씬을 액티브로 전환
        
        GameObject[] objects = SceneManager.GetActiveScene().GetRootGameObjects();

        for(int i = 0; i < objects.Length; i++)
        {
            if(objects[i].name == "SceneManager" || objects[i].name == "Scene Manager")
            {
                objects[i].GetComponent<SceneController>().AdditiveEnded(isClear);
                break;
            }
        }
    }

}
