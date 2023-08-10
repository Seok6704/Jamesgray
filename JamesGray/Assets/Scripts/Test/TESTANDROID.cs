using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TESTANDROID : MonoBehaviour
{
    // 텍스트 출력 확인용.
    [SerializeField] TMP_Text t;

    // 제작한 안드로이드 플러그인 캐싱용.
    private AndroidJavaObject activityContext = null;
    private AndroidJavaClass javaClass = null;
    private AndroidJavaObject javaClassInstance = null;


    void Start()
    {

        // context설정을 위한 현재 유니티의 Activity캐싱.
        // context는 안드로이드 Activity 상태값 이라고 생각하면 될듯.
        // using문은 안에 중괄호가 끝날 때 메모리에서 확실하게 내리기 위함이라고 함.
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

        }

        // 작성한 자바 코드의 맨위에 있는 패키지명 + .클래스 이름
        using (javaClass = new AndroidJavaClass("com.mandarange.myunitytest.myUnityTest"))
        {
            if (javaClass != null)
            {
                // 이렇게 인스턴스로 안하면 작동 안되는거 확인함.
                // 왜 안되는건지 아시는 분 있으시면 댓글 달아주세요ㅠㅠ
                javaClassInstance = javaClass.CallStatic<AndroidJavaObject>("instance");

                // 자바 코드쪽에 Context를 전달하여 설정.
                javaClassInstance.Call("setContext", activityContext);
            }

        }

        // 안드로이드 코드 실행하기.
        callJava();

    }

    // 안드로이드 코드 실행하기.
    void callJava()
    {
        //-----------------메세지 함수 실행 요청.-------------------------------

        System.Object[] objs = new System.Object[2];

        // 메세지를 받을 오브젝트 이름.
        objs[0] = "M";

        // 오브젝트에 포함된 스크립트들 중 OutPutLog이라는 함수를 실행해달라고 전달할 용도.
        objs[1] = "OutPutLog";

        // 안드로이드 플러그인에서 TestLog함수를 실행하고, 인수로 objs를 전달해줌.
        // 이 과정에서 유니티의 데이터도 안드로이드로 넘길 수 있음.
        javaClassInstance.Call("TestLog", objs);

        //-------------------토스트 위젯 띄우기 요청-----------------------------

        javaClassInstance.Call("ShowToast");

    }





    // 안드로이드 스튜디오에서 sendMessage에 인수가 있으므로 이 함수에도 같은 형식의 인수가 있어야함.
    // 안드로이드 쪽에서 실행하는것이므로 따로 유니티 로직에는 포함될 필요가 없음.
    // 이 예제 코드의 경우 (유니티)Start함수 -> (안드로이드)TestLog함수 -> (유니티)OutPutLog함수 순으로 실행 됨.
    void OutPutLog(string msg)
    {
        t.text = msg;

        Debug.Log(msg);
    }


}