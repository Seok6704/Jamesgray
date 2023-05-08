using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

//플레이어의 이동에 따라 따라가는 카메라 구현
//pixel perfect camer : 19.5 : 9 비율로 계산할것
public class PlayerFollow : MonoBehaviour
{
    //public float camera_Speed;
    [Header("다이얼로그 위치 조정")]
    [Range(-15, 15)]
    public float x;
    [Range(-15, 15)]
    public float y;

    /*
    [Header("Camera Zoom In Speed")]
    [Range(0, 1)]
    public float zoomSpeed;
    [Header("Zoom Level")]
    [Range(0, 10)]
    public float zoomlv;

    float zoom;
    */
    bool dialogueOn = false;
    public GameObject player;

    //PixelPerfectCamera pixelCam;
    //int pixelResolutionX, pixelResolutionY;

    private void Awake() 
    {
        //pixelCam = this.GetComponent<PixelPerfectCamera>();
        //pixelResolutionX = pixelCam.refResolutionX;
        //pixelResolutionY = pixelCam.refResolutionY;
        //zoom = 0.0f;
    }

    private void Update() 
    {   
        sbyte mod = 0;      // -128 ~ 127
        Vector3 dir = player.transform.position - this.transform.position;  //방향 구하기

        if(dialogueOn)
        {
            mod = 1;
            //zoom = Mathf.Lerp(zoom, zoomlv, zoomSpeed);
        }

        Vector3 moveVector = new Vector3((dir.x + (x * mod)) * Time.deltaTime, (dir.y + (y * mod)) * Time.deltaTime, 0.0f);

        this.transform.Translate(moveVector);
        //pixelCam.refResolutionX = pixelResolutionX - ((int)(19.5 * zoom));
        //pixelCam.refResolutionY = pixelResolutionY - ((int)(9 * zoom));
    }

    public void SetFlag()   //이벤트로 호출되면 플레그를 반전, 다이얼로그에서 호출
    {
        dialogueOn = !dialogueOn;
    }
}
