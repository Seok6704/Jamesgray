using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//배경화면 이미지를 회색으로 변경하는 스크립트
//사용법 : NewGrayController 스크립트는 NewGrayShader.shader, GrayImageMaterial 과 결합하여 작성하였기에 다 같이 사용해야 정상적으로 작동함.
//회색으로 변경할 이미지에 GrayImageMaterial과 스크립트를 적용, 스크립트는 Awake함수에서 자동으로 Material을 지정할 것임.
//그리고 이벤트 등을 사용하여 public함수를 불러서 사용하면 됨.
//
//만약, 씬뷰에서는 정상적으로 보이는데, 게임뷰에서는 검정색으로 변한다면 캔버스 렌더모드를 ScreenSpace - camera로 변경해볼것.
//
//쉐이더를 건드려 보고 싶다면 아래 링크의 글들을 읽고 따라해볼것.
//https://celestialbody.tistory.com/5

public class NewGrayController : MonoBehaviour
{
    Material material;

    [Range(0,1)]
    public float f_grayStrength = 0.0f; //Debug 용

    [Tooltip("그레이로 변환하는 속도 조절")]
    [Range(0,1)]
    public float f_graySpeed;     //0.006

    private void Awake()    //자동으로 해당 객체의 머티리얼을 가져오도록 함.
    {
        material = Instantiate(GetComponent<Image>().material); //Instantiate함수로 복사하지 않으면 쉐이더가 영구적으로 변경되는 문제가 있어 복사하여 사용.
        GetComponent<Image>().material = material;
    }   

    public void SetGray()
    {
        StartCoroutine(IncreaseGray());
    }
    public void SetColor()
    {
        StartCoroutine(DecreaseGray());
    }

    IEnumerator IncreaseGray()
    {
        for(; f_grayStrength <= 1.0f; f_grayStrength += f_graySpeed) {
            if(f_grayStrength >= 1.0f) break;

            material.SetFloat("_GrayStrength", f_grayStrength);

            yield return null;
        }
        f_grayStrength = 1.0f;

        material.SetFloat("_GrayStrength", f_grayStrength);
    }

    IEnumerator DecreaseGray()
    {
        for(; f_grayStrength >= 0; f_grayStrength -= f_graySpeed) {
            if(f_grayStrength <= 0) break;

            material.SetFloat("_GrayStrength", f_grayStrength);

            yield return null;
        }
        f_grayStrength = 0.0f;

        material.SetFloat("_GrayStrength", f_grayStrength);
    }
}
