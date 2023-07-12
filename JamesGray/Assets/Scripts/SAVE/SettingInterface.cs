using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingInterface : MonoBehaviour
{
    public Toggle toggleVirtualPad;
    public Slider sliderVolume;
    public TMP_Dropdown dropdownDifficulty;
    
    SettingClass setting;
    

    private void Start() 
    {
        //SetDropDown();

        toggleVirtualPad.onValueChanged.AddListener(delegate {
            ToggleVPChanged();
        });    

        sliderVolume.onValueChanged.AddListener(delegate {
            SliderVolChanged();
        });

        dropdownDifficulty.onValueChanged.AddListener(delegate {
            DropDownChanged();
        });
    }

    public void PushChanges()
    {
        SettingManager.SetSettings(setting);
    }

    public void SetValues(SettingClass saved)
    {
        setting = saved;

        UpdateObj();
    }

    public SettingClass GetValues()
    {
        return setting;
    }

    void UpdateObj()
    {
        toggleVirtualPad.isOn = setting.onVirtualPad;
        sliderVolume.value = setting.volume;
        dropdownDifficulty.value = setting.difficultOption;
    }

    void SetDropDown()
    {
        List<string> opt = new List<string>();
        
        opt.Add("EASY");
        opt.Add("MEDIUM");
        opt.Add("HARD");

        dropdownDifficulty.ClearOptions();
        dropdownDifficulty.AddOptions(opt);
    }

    void ToggleVPChanged()
    {
        setting.onVirtualPad = toggleVirtualPad.isOn;
    }
    void SliderVolChanged()
    {
        setting.volume = sliderVolume.value;
    }
    void DropDownChanged()
    {
        setting.difficultOption = dropdownDifficulty.value;
    }

}
