using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CrossGame : MonoBehaviour
{
    public Animator anim;
    public AudioSource audioSrc;
    public GameObject Dialog;
    public bool Move;
    float time; // 시간 변수
    bool flag;
    bool M_Stop;
    Vector3 pos;

    void Awake()
    {
        anim = GameObject.Find("Walker").GetComponent<Animator>();    
        flag = false;
        Move = false;
        M_Stop = false;
    }

    void Update()
    {
        if(flag)
        {
            time += Time.deltaTime;
            pos = this.transform.position; // 현재 Walker 위치 담는 변수
            if(pos.x >= 2140) // 도착 지점 도달 시
            { 
                flag = false;
                Dialog.GetComponent<DialoguesManager>().SetDialogue(903, 6);
            }

            if( (time > 5 && time < 8) || (time > 13 && time < 15) ||  (time > 24 && time < 26) || (time > 30 && time < 31) || (time > 39 && time < 41) || (time > 49 && time < 51)) // 음악이 멈추는 시간
            {
                M_Stop = true;
            }
            else M_Stop = false;

            if ( time > 60) // 타임 오버
            {
                flag = false;
                Dialog.GetComponent<DialoguesManager>().SetDialogue(903, 7);
            }

            if(Move && pos.x <= 2150 && !M_Stop) // 버튼이 클릭되고 있고, Walker가 도착점에 도착하지 않았을 경우, 위치 이동.
            {
                transform.Translate(new Vector3(0.2f, 0, 0));
                anim.SetBool("Btn_R_Click", true);
            }

            else if( Move && pos.x <= 2150 && M_Stop ) // 버튼이 클릭되고 있고, 음악이 멈춰있는 도중일 경우
            {
                transform.Translate(new Vector3(0.2f, 0, 0));
                anim.SetBool("Btn_R_Click", true);
                flag = false;
                Dialog.GetComponent<DialoguesManager>().SetDialogue(903, 5);
            }
        }
        
    }

    public void Btn_R_Click_Down()
    {
        Move = true;
    }

    public void Btn_R_Click_UP()
    {
        Move = false;
        anim.SetBool("Btn_R_Click", false);
    }

    public void Start_Btn_Click()
    {
        if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
        AudioClip clip = Resources.Load("Sounds/Minigame/1-S-1/Music") as AudioClip;
        audioSrc.PlayOneShot(clip);
        flag = true;
        GameObject.Find("Start").SetActive(false);
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
                objects[i].GetComponent<SceneController>().AdditiveEnded();
                break;
            }
        }
    }

    void Scenechange()
    {
        SceneManager.LoadScene("Chapter0");
    }
}
