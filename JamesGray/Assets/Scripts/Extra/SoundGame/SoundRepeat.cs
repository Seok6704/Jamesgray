using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRepeat : MonoBehaviour
{   
    public AudioSource audioSource;
    public AudioClip popSound;

    public float secondBetweenSound;
    public bool activeOnStart;

    private void Start() {

        if(activeOnStart)
        {
            //StartCoroutine(PlaySoundConstantly());
            audioSource.PlayOneShot(popSound);
        }
    }
    public void StartMusic() 
    {
        audioSource.PlayOneShot(popSound);
    }

    public void StopMusic() 
    {
        audioSource.Stop();
    }

    IEnumerator PlaySoundConstantly() 
    {
        while(true) 
        {
            audioSource.PlayOneShot(popSound);
            yield return new WaitForSeconds(secondBetweenSound);
        }
    }
}
