using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BLEManager : MonoBehaviour, IBLE
{
    private BluetoothController BLE = null;

    public bool BLE_Connection;
    
    private void Start() 
    {
        if(SettingManager.useController)
        {
            return;
        }

        BLE = BluetoothController.GetInstance();
        BLE_Connection = false;
        
        BLE.SetReceiverName(gameObject.name);
        
        BLE.ScanBLE();
    }

    /// <summary>
    /// 블루투스에게 데이터 요청하기. Characteristic이 여러개인 경우 num으로 지정해야함.
    /// </summary>
    /// <param name="num">Characteristic Index</param>
    public void Read(int num)
    {
        if(!BLE_Connection)
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
    /// <summary>
    /// 디버그 로그가 들어오는 곳
    /// </summary>
    public void setLog(string msg)
    {
        Debug.Log("From Java Debug Log - " + msg);
    }
    
}
