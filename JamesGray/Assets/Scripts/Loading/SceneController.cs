using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public void WaitToLoad()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait() 
    {
        for(int i = 0; i < 1000; i++)
        {
            yield return null;
        }    
        LoadingScene.LoadScene("Chapter0");
    }
}
