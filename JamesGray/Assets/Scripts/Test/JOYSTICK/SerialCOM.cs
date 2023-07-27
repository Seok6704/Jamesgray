using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

//https://velog.io/@anthem53/Unity-Serial-%ED%86%B5%EC%8B%A0-%EB%A9%94%EB%AA%A8 참고

/*
    chatgpt 한테 물어본결과
    블루투스는 시리얼 통신이 맞지만, 안드로이드에서는 COM이 할당되지 않는다고 함. 기기 명이나 MAC을 확인하라고함. SPP도 참고해보라함.
    일단은 유선 시리얼 통신을 가정하고 구현해보겠음.
*/
public class SerialCOM : MonoBehaviour
{
    public int baudrate = 9600;
    public int COMNum;
    SerialPort sp;
    // Start is called before the first frame update
    void Start()
    {
        //string[] COMS = SerialPort.GetPortNames();

        //for(int i = 0; i < COMS.Length; i++)
        //{
         //   Debug.Log(COMS[i]);
        //}
        sp = new SerialPort("COM" + COMNum, baudrate);

        sp.Open();
        sp.ReadTimeout = 100;   //무한루프 방지
        sp.WriteTimeout = 100;
        //Debug.Log(sp.IsOpen);
    }

    private void Update() 
    {
        if(sp.IsOpen)
        {
            Debug.Log(sp.ReadExisting());
        }    
    }

    private void OnDestroy() {
        sp.Close();
    }
}
