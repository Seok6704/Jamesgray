using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class BluetoothController
{
    private static BluetoothController instance = null; //싱글톤 디자인
    
    private AndroidJavaObject activityContext = null;   //안드로이드 context
    private AndroidJavaObject javaClassInstance = null;
    private AndroidJavaClass javaClass = null;

    bool BLE_Connection = false;
    bool debug_Once = false;

    readonly string[] PERMISSIONS = {
        "android.permission.ACCESS_FINE_LOCATION",
        "android.permission.ACCESS_COARSE_LOCATION",
        "android.permission.BLUETOOTH",
        "android.permission.BLUETOOTH_ADMIN",
        "android.permission.BLUETOOTH_CONNECT",
        "android.permission.BLUETOOTH_SCAN",
        "android.permission.ACCESS_BACKGROUND_LOCATION"
    };

    public BluetoothController()
    {
        if(!ReferenceEquals(null, instance))
        {   
            Debug.Log("Bluetooth Instacne is Already available\n Use instance method instead.");   //이미 객체가 존재한다면
            return;
        }

        //instance = this;

        //using문을 사용하여 메모리에서 필요없어진 시점에 제거
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) //안드로이드 플러그인에 포함된 UnityPlayer라는 객체 접근
        {
            activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
        }

        using(javaClass = new AndroidJavaClass("com.nan.myunitytest.myUnityTest")) //내가 만든 객체 접근
        {
            javaClassInstance = javaClass.CallStatic<AndroidJavaObject>("instance");    //플러그인에 선언된 static 메소드중 instance 메소드 호출
            
            javaClassInstance.Call("setContext", activityContext);
        }

        PermissionCallbacks callbacks = new PermissionCallbacks();  //블루투스 권한을 위해 콜백 선언. 여러번 거절당하거나 하면 그 뒤로 제거하고 다시 해도 동작안되는 경우가 있는거 같은데 왜그런지 모르겠음

        callbacks.PermissionDenied += PermissionCallbacks_PermissionDenied;
        callbacks.PermissionGranted += PermissionCallbacks_PermissionGranted;
        callbacks.PermissionDeniedAndDontAskAgain += PermissionCallbacks_PermissionDeniedAndDontAskAgain;
    
        for(int i = 0; i < PERMISSIONS.Length; i++)
        {
            if(!Permission.HasUserAuthorizedPermission(PERMISSIONS[i]))
            {
                System.Object msg = PERMISSIONS[i] + " need Permission.";
                javaClassInstance.Call("SendLog", msg);
            }
        }

        Permission.RequestUserPermissions(PERMISSIONS, callbacks);

        BLE_Connection = false;
        debug_Once = false;
    }

    public static BluetoothController GetInstance()
    {
        if(ReferenceEquals(instance, null))
        {
            //Debug.Log("Instacne is not available. \n Use Constructor first.");  //인스턴스가 아직 생성안되었을때
            //return null;
            instance = new BluetoothController();
        }
        return instance;
    }


    /// <summary>
    /// 플러그인으로 부터 데이터를 전달받을 오브젝트 이름 설정
    /// </summary>
    /// <param name="objName">전달 받을 수신지 오브젝트 이름</param>
    public void SetReceiverName(string objName)
    {
        System.Object obj = objName;
        javaClassInstance.Call("SetReceiverName", obj);
    }
    
    /// <summary>
    /// BLE 디바이스 검색 및 성공 시 연결하기
    /// </summary>
    public void ScanBLE()
    {
        javaClassInstance.Call("ScanDevices");
    }

    /// <summary>
    /// 안드로이드에서 Toast(짧은 시간 동안 유지되는 메세지) 띄우기. 
    /// </summary>
    /// <param name="msg">메세지</param>
    public void MakeToast(string msg)
    {
        System.Object obj = msg;
        javaClassInstance.Call("makeToast", obj);
    }

    public void ReadCharacteristic(int num)
    {
        System.Object obj = num.ToString();
        javaClassInstance.Call("ReadCharacteristic", obj);
    }

    void SetLog(string msg)
    {
        Debug.Log("From Java Debug Log - " + msg);
    }


    void OutPutLog(string msg)
    {
        Debug.Log("From Java - " + msg);
    }

    void SetBLEConnection(string msg)
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

    
    ///////콜백///////
    
    internal void PermissionCallbacks_PermissionDeniedAndDontAskAgain(string permissionName)
    {
        System.Object msg = "UNITY - This is the Final Permission Ask. You will be no longer able to use Controller. Reinstall suggest.";
        javaClassInstance.Call("SendLog", msg);
    }
    internal void PermissionCallbacks_PermissionGranted(string permissionName)
    {
        System.Object msg = "UNITY - " + permissionName + " Granted.";
        javaClassInstance.Call("SendLog", msg);
    }

    internal void PermissionCallbacks_PermissionDenied(string permissionName)
    {
        System.Object msg = "UNITY - " + permissionName + " Not Granted!";
        javaClassInstance.Call("SendLog", msg);
    }
}

interface IBLE
{
    void setLog(string msg);
    void OutPutLog(string msg);
    void SetBLEConnection(string msg);

}
