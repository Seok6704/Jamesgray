using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string dif;
        if(SettingInstance.difficult == 0) 
        {
            dif = "EASY";
        }
        else if(SettingInstance.difficult == 1)
        {
            dif = "MEDIUM";
        }
        else
        {
            dif = "HARD";
        }
        Debug.Log("현재 난이도는 " + dif + " 입니다.");
    }

}
