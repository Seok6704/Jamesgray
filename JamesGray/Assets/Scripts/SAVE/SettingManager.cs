using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static bool onVirtualPad
    {
        get { return setting.onVirtualPad; }
        set { setting.onVirtualPad = value; }
    }
    static SettingClass setting = new SettingClass();

    public SettingInterface settingInterface;

    string path;

    void Awake() 
    {
        path = Application.persistentDataPath + "/Setting/";
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path += "sts.json";
        if(!File.Exists(path))
        {
            FileStream temp = File.Create(path);      //파일이 존재하지 않는다면 새로 만들기
            temp.Close();
            setting = DefaultSettings();
            SaveSetting();
        }
        else                        //파일이 있다면 해당 파일 읽고 메모리에 올리기
        {
            string settingJson = File.ReadAllText(path);
            setting = JsonUtility.FromJson<SettingClass>(settingJson);
        }

        settingInterface.SetValues(setting);
    }
    /// <summary>
    ///  현재 설정 저장
    /// </sumarry>
    public void SaveSetting()
    {
        string settingData = JsonUtility.ToJson(setting);

        File.WriteAllText(path, settingData);
    }
    /// <summary>
    ///  설정을 기본값으로 초기화
    /// </sumarry>
    public void ResetSetting()
    {   
        setting = DefaultSettings();
    }

    public static void SetSettings(SettingClass changed)
    {
        if(setting == null)
            setting = DefaultSettings();
        
        setting = changed;
    }

    static SettingClass DefaultSettings()
    {
        SettingClass newSetting = new SettingClass();
        
        newSetting.volume = 1.0f;
        newSetting.difficultOption = 0;
        newSetting.onVirtualPad = false;

        return newSetting;
    }
}

[System.Serializable] 
public class SettingClass
{
    public float volume;
    public int difficultOption;
    public bool onVirtualPad;
}
