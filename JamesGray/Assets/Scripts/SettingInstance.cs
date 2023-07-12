using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

//Setting 인스턴스 
//Static으로 선언하면 씬전환후에도 데이터가 남아있다고 해서 실험으로 만들어봄.

public class SettingInstance : MonoBehaviour
{
    public static int difficult;
    public TMP_Dropdown options; 

    public static SettingClass setting;

    List<string> optionList = new List<string>();

    string path;

    void Awaketemp() 
    {
        path = Application.persistentDataPath + "/Setting/";
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path += "sts.json";
        if(!File.Exists(path))
        {
            File.Create(path);      //파일이 존재하지 않는다면 새로 만들기
            setting = SetSettings();
            SaveSetting();
        }
        else                        //파일이 있다면 해당 파일 읽고 메모리에 올리기
        {
            string settingJson = File.ReadAllText(path);
            setting = JsonUtility.FromJson<SettingClass>(settingJson);
        }
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
        options.onValueChanged.AddListener(delegate {SetDropDown(options.value);});

        SetDropDown(0);
    }   
    
    void SetDropDown(int option)
    {
        Debug.Log("options changed : " + option);
        difficult = option;
    }

    void SaveSetting()
    {
        string settingData = JsonUtility.ToJson(setting);

        File.WriteAllText(path, settingData);
    }

    SettingClass SetSettings()
    {
        SettingClass newSetting = new SettingClass();
        
        newSetting.difficultOption = 0;
        newSetting.onVirtualPad = false;

        return newSetting;
    }


    [System.Serializable] 
    public class SettingClass
    {
        public int difficultOption;
        public bool onVirtualPad;

    }
}
