using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingControl : MonoBehaviour
{
    int num = 0;
    public void CoroutineStart()
    {
        StartCoroutine("StartEnding");
    }

    IEnumerator StartEnding()
    {
        while(true)
        {
            GameObject.Find("Panel_End").transform.GetChild(num).gameObject.SetActive(true);
            yield return new WaitForSeconds(4f);
            if(num == 3) break;
            GameObject.Find("Panel_End").transform.GetChild(num).gameObject.SetActive(false);
            num++;
        }
        GameObject.Find("Panel_End3").GetComponent<FadeOut>().OnStart();
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("Chapter4-1_Color");
    }
}
