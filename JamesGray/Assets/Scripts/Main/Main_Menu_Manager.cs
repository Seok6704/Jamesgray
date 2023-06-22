using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//메인메뉴의 경우에는 여러 오브젝트가 있어서 이 스크립트에서 이벤트로 간결하게 관리하고자 함.
//메인메뉴 이미지의 자식 오브젝트가 버튼밖에 없을때 정상적으로 동작.
public class Main_Menu_Manager : MonoBehaviour  
{
    //public UnityEvent Show_Menu, hide_Menu;

    [Header ("메인메뉴로 사용할 이미지")]
    public GameObject Main_Menu;
    //[Header ("메인메뉴가 이동할 위치")]
    //[Tooltip ("메인메뉴는 캔버스 밖 어딘가로 배치하고 이벤트 발생시 이동할 곳 입력")]
    //public Vector3 pos;
    [Header ("타이틀")]
    public GameObject title;
    //public GameObject subtitle;
    RectTransform rect;
    RectTransform canvas;
    Vector3 pos;
    void Start() 
    {
        rect = GetComponent<RectTransform>();
        canvas = rect.parent.GetComponent<RectTransform>();
        pos = rect.position;
        
        title.GetComponent<Img_Fading>().SetTransparent();
        //title.GetComponent<Text_Fading>().SetTransparent(); //타이틀 투명화
        //subtitle.GetComponent<Text_Fading>().SetTransparent();

        GetComponent<Img_Fading>().SetTransparent();        //자기 자신과 자식 버튼들의 글자를 투명하게 변경, 버튼은 기본적으로 투명한것으로 가정

        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<Text_Fading>().SetTransparent();
        }
    }
    public void ActiveEvent()
    {
        rect.position = canvas.position;
        //rect.position = new Vector3(rect.parent.position.x - 7, rect.parent.position.y);
        //Show_Menu.Invoke();
    }

    public void HideMenu()
    {
        //hide_Menu.Invoke();
        StartCoroutine(WaitHide());
        title.GetComponent<Img_Fading>().ShowDown();
        //title.GetComponent<Text_Fading>().ShowDown();
        //subtitle.GetComponent<Text_Fading>().ShowDown();
        GetComponent<Img_Fading>().ShowDown();

        for(int i = 0; i < transform.childCount; i++) 
        {
            transform.GetChild(i).GetChild(0).GetComponent<Text_Fading>().ShowDown();
        }
    }

    public void ShowMenu()
    {
        rect.position = canvas.position;
        title.GetComponent<Img_Fading>().ShowUp();
        //title.GetComponent<Text_Fading>().ShowUp();
        //subtitle.GetComponent<Text_Fading>().ShowUp();
        GetComponent<Img_Fading>().ShowUp();

        for(int i = 0; i < transform.childCount; i++) 
        {
            transform.GetChild(i).GetChild(0).GetComponent<Text_Fading>().ShowUp();
        }
    }

    IEnumerator WaitHide()
    {
        for(int i = 0; i < 300; i++) 
        {
            yield return null;
        }
        rect.position = pos;
    }
}
