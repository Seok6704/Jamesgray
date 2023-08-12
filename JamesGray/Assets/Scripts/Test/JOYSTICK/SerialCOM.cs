using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

//https://velog.io/@anthem53/Unity-Serial-%ED%86%B5%EC%8B%A0-%EB%A9%94%EB%AA%A8 참고
//https://parksh3641.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-C-%EB%B8%94%EB%A3%A8%ED%88%AC%EC%8A%A4-%ED%86%B5%EC%8B%A0-%EA%B0%84%EB%8B%A8-%EA%B5%AC%ED%98%84%ED%95%98%EA%B8%B0
//https://m.blog.naver.com/PostView.naver?isHttpsRedirect=true&blogId=chodadoo&logNo=220960049198
//https://you-rang.tistory.com/214
//https://www.google.com/search?q=%EC%95%84%EB%91%90%EC%9D%B4%EB%85%B8%EC%97%90%EC%84%9C+%EC%9C%A0%EB%8B%88%ED%8B%B0&sourceid=chrome&ie=UTF-8
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
    int waitFrame = 50;
    char latest;

    void Start()
    {
        string[] COMS = SerialPort.GetPortNames();
        for(int i = 0; i < COMS.Length; i++)
        {
            Debug.Log(COMS[i]);
        }

     /*   string[] COMS = SerialPort.GetPortNames();

        for(int i = 0; i < COMS.Length; i++)
        {
            Debug.Log(COMS[i]);
        }
        
        sp = new SerialPort("COM" + COMNum, baudrate);

        Thread.Sleep(250);

        sp.ReadTimeout = 100;   //무한루프 방지
        sp.WriteTimeout = 100;

        Debug.Log(baudrate);

        sp.Open();
        //Debug.Log(sp.IsOpen);*/
        latest = 'n';
        SetSerial();
    }

    void SetSerial()
    {
        sp = new SerialPort("COM" + COMNum, baudrate, Parity.None, 8, StopBits.One);

        sp.Open();

        sp.DtrEnable = true;
        sp.ReadTimeout = 5;    

        Debug.Log("COM : " + COMNum + "   ,   BaudRate : " + baudrate);
    }

    public char GetInput()
    {
        char input;
        string streamInput = "";
        if(sp.IsOpen)
        {
            try
            {
                //Debug.Log(sp.ReadByte());
                //Debug.Log(sp.ReadExisting());
                streamInput = sp.ReadExisting();
                //Debug.Log(streamInput);
            }
            catch (System.TimeoutException e)   //ReadExisting이기 때문에 타임아웃 예외가 발생하지 않음...
            {
                Debug.Log(e);
                throw;
            }
        }
        else if(!sp.IsOpen)
        {
            Debug.Log("Connecting...");
            SetSerial();
        }

        if(streamInput.Length > 1)
        {
            input = streamInput[streamInput.Length - 1];    //가장 최신 입력을 가져오기
        }
        else if(streamInput.Length == 1)
        {
            input = streamInput[0];
        }
        else
        {
            input = latest;
        }

        latest = input;
        return input;
    }

    private void OnDestroy() {
        if(sp.IsOpen)
            sp.Close();
        Thread.Sleep(250);
        sp = null;
    }
}
