using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Setting 인스턴스 
//Static으로 선언하면 씬전환후에도 데이터가 남아있다고 해서 실험으로 만들어봄.

public class SettingInstance : MonoBehaviour
{
    public static int difficult;
    public TMP_Dropdown options; 

    List<string> optionList = new List<string>();

    void Awake() 
    {
        
    }

    void Start() 
    {
        //options = this.GetComponent<TMP_Dropdown>();    
        options.ClearOptions();

        optionList.Add("EASY");
        optionList.Add("MEDIUM");
        optionList.Add("HARD");
        
        options.AddOptions(optionList);

        options.value = 0;
        options.onValueChanged.AddListener(delegate {setDropDown(options.value);});

        setDropDown(0);
    }   
    
    void setDropDown(int option)
    {
        Debug.Log("options changed : " + option);
        difficult = option;
    }
}
