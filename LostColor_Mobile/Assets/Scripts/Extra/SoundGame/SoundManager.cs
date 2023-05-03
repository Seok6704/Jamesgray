using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] audioSourceArray;  //시계방향으로 배열에 저장 Ex) 0 = Front, 1 = Right
    public AudioClip audioClipPop;  //POP Sound
    public static SoundManager instance;
    public TMP_Text score;

    bool hardMode = false;

    int i_SoundGame_Answer, i_Player_Anwer, i_Score = 0;
    
    private void Awake() 
    {
        i_SoundGame_Answer = -1;
        i_Player_Anwer = -1;

        score.text = i_Score.ToString();

        if(SoundManager.instance == null)
        {
            SoundManager.instance = this;

            audioSourceArray = new AudioSource[transform.childCount];

            for(int i = 0; i < transform.childCount; i++) 
            {
                audioSourceArray[i] = transform.GetChild(i).GetComponent<AudioSource>();
            }
        }
    }

    public void setMode() 
    {
        hardMode = !hardMode;
    }
    void PlaySound_atIndex(int audioSrc_idx) 
    {
        audioSourceArray[audioSrc_idx].PlayOneShot(audioClipPop);
    }
    public void OnClickFront() {
        //PlaySound_atIndex(0);
        i_Player_Anwer = 0;
    }
    public void OnClickRight() {
        //PlaySound_atIndex(1);
        i_Player_Anwer = 1;
    }
    public void OnClickBack() {
        //PlaySound_atIndex(2);
        i_Player_Anwer = 2;
    }
    public void OnClickLeft() {
        //PlaySound_atIndex(3);
        i_Player_Anwer = 3;
    }

    public void PlayPopSound()      //debug 용
    {
        //audioSource.PlayOneShot(audioClipPop);
        StartCoroutine(PlaySound_RandomDirection());
    }

    void CheckAnswer() 
    {
        if(i_Player_Anwer != i_SoundGame_Answer) 
        {
            Debug.Log("Gaem Over...");
            gameObject.SetActive(false);
        }
        else
        {
            i_Score += 10;
            score.text = i_Score.ToString();
        }
    }

    IEnumerator PlaySound_RandomDirection() 
    {
        while(true) 
        {
            int i_random = Random.Range(0, audioSourceArray.Length); 
            i_SoundGame_Answer = i_random;
            
            audioSourceArray[i_random].PlayOneShot(audioClipPop);
            while(true) 
            {
                yield return new WaitForSeconds(2f);
                if(!hardMode)
                {
                    audioSourceArray[i_random].PlayOneShot(audioClipPop);
                }
    
                if(i_Player_Anwer != -1)
                {
                    CheckAnswer();
                    break;
                }
            }
            
        }
    }

}
