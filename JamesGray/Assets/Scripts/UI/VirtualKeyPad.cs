using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualKeyPad : MonoBehaviour
{
    public bool UP {get; private set;} = false;
    public bool DOWN {get; private set;} = false;
    public bool RIGHT {get; private set;} = false;
    public bool LEFT {get; private set;} = false;
    public bool ACTION {get; private set;} = false;

    void Awake()
    {
        this.gameObject.SetActive(SettingManager.onVirtualPad);   //설정이 꺼져있으면 비활성화
    }

    public void UPPressed()
    {
        UP = true;
    }

    public void UPReleased()
    {
        UP = false;
    }
    public void DOWNPressed()
    {
        DOWN = true;
    }

    public void DOWNReleased()
    {
        DOWN = false;
    }
    public void RIGHTPressed()
    {
        RIGHT = true;
    }

    public void RIGHTReleased()
    {
        RIGHT = false;
    }
    public void LEFTPressed()
    {
        LEFT = true;
    }

    public void LEFTReleased()
    {
        LEFT = false;
    }

    public void ACTIONPressed()
    {
        ACTION = true;
    }

    public void ACTIONReleased()
    {
        ACTION = false;
    }
}
