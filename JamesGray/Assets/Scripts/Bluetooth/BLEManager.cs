using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BLEManager : MonoBehaviour, IBLE
{
    private BluetoothController BLE = null;

    public bool BLE_Connection;
    public bool BLE_Ready2Read;
    public bool BLE_Scan;
    
    private void Start() 
    {
        //if(SettingManager.useController)
        //{
        //   return;
        //}
        if (Application.platform != RuntimePlatform.Android) return;
        BLE = BluetoothController.GetInstance();
        BLE_Connection = false;
        BLE_Ready2Read = false;
        BLE_Scan = false;
        
        BLE.SetReceiverName(gameObject.name);   //JAVA로 부터 메세지를 받을 객체
        
        //ScanBLE();
    }

    /// <summary>
    /// 블루투스에게 데이터 요청하기. Characteristic이 여러개인 경우 num으로 지정해야함.
    /// </summary>
    /// <param name="num">Characteristic Index</param>
    public void Read(int num)   //num = 2로 일단 해보기
    {
        if(!BLE_Connection || !BLE_Ready2Read)
            return;
            
        BLE.ReadCharacteristic(num);
    }
    
    /// <summary>
    /// 컨트롤러 데이터가 들어오는 곳
    /// </summary>
    public void OutPutLog(string msg)
    {
        Debug.Log("From Java - " + msg);
    }

    public void SetBLEConnection(string msg)
    {
        if(msg == "TRUE")
        {
            BLE_Connection = true;
        }
        else
        {
            BLE_Connection = false;
        }
    }

    public void SetReady2Read(string msg)
    {
        if(msg == "TRUE")
        {
            BLE_Ready2Read = true;
        }
        else
        {
            BLE_Ready2Read = false;
        }
    }
    /// <summary>
    /// 디버그 로그가 들어오는 곳
    /// </summary>
    public void setLog(string msg)
    {
        Debug.Log("From Java Debug Log - " + msg);
    }

    public void ScanBLE()
    {
        BLE_Scan = !BLE_Scan;
        BLE.ScanBLE();
    }
    
}
