using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    Animator ele0, ele1, ele2, james;
    public AudioSource audioSrc;
    int ran;

    void Start()
    {
        ele0 = GameObject.Find("Elevator0").GetComponent<Animator>();
        ele1 = GameObject.Find("Elevator1").GetComponent<Animator>();
        ele2 = GameObject.Find("Elevator2").GetComponent<Animator>();
        james = GameObject.Find("James").GetComponent<Animator>();
        ran = Random.Range(0,3);
    }

    public void LeftElevatorClick()
    {
        james.SetTrigger("LeftClick");
        StartCoroutine(eleTrigger(ele0));
    }

    public void RightElevatorClick()
    {
        james.SetTrigger("RightClick");
        StartCoroutine(eleTrigger(ele2));
    }

    public void MiddleElevatorClick()
    {
        james.SetTrigger("MiddleClick");
        StartCoroutine(eleTrigger(ele1));
    }

    IEnumerator eleTrigger(Animator anim)
    {
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("Open");
    }

    public void BtnE0Click()
    {
        if(ran == 0)
        {
            if(audioSrc.isPlaying)
            {
                audioSrc.Stop();
            }
            AudioClip clip = Resources.Load("Sounds/Minigame/2-1/Correct") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
        else if(ran == 1)
        {
            if(audioSrc.isPlaying)
            {
                audioSrc.Stop();
            }
            AudioClip clip = Resources.Load("Sounds/Minigame/2-1/Incorrect1") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
        else
        {
            if(audioSrc.isPlaying)
            {
                audioSrc.Stop();
            }
            AudioClip clip = Resources.Load("Sounds/Minigame/2-1/Incorrect2") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
    }
    public void BtnE1Click()
    {
        if(ran == 1)
        {
            if(audioSrc.isPlaying)
            {
                audioSrc.Stop();
            }
            AudioClip clip = Resources.Load("Sounds/Minigame/2-1/Correct") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
        else if(ran == 0)
        {
            if(audioSrc.isPlaying)
            {
                audioSrc.Stop();
            }
            AudioClip clip = Resources.Load("Sounds/Minigame/2-1/Incorrect2") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
        else
        {
            if(audioSrc.isPlaying)
            {
                audioSrc.Stop();
            }
            AudioClip clip = Resources.Load("Sounds/Minigame/2-1/Incorrect1") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
    }
    public void BtnE2Click()
    {
        if(ran == 2)
        {
            if(audioSrc.isPlaying)
            {
                audioSrc.Stop();
            }
            AudioClip clip = Resources.Load("Sounds/Minigame/2-1/Correct") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
        else if(ran == 0)
        {
            if(audioSrc.isPlaying)
            {
                audioSrc.Stop();
            }
            AudioClip clip = Resources.Load("Sounds/Minigame/2-1/Incorrect1") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
        else
        {
            if(audioSrc.isPlaying)
            {
                audioSrc.Stop();
            }
            AudioClip clip = Resources.Load("Sounds/Minigame/2-1/Incorrect2") as AudioClip;
            audioSrc.PlayOneShot(clip);
        }
    }
}
