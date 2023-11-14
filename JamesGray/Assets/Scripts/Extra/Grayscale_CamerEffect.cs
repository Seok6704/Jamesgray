using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grayscale_CamerEffect : MonoBehaviour
{
    Material CameraMaterial;
    public float grayScale = 0.0f;
    float appliedTime = 1.0f;

    void Start(){
        CameraMaterial = new Material(Shader.Find("Custom/GrayScale"));
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest) { //후처리 효과, src(현재 화면)을 dst로 교체
        CameraMaterial.SetFloat("_Grayscale", grayScale);
        Graphics.Blit(src, dest, CameraMaterial);    
    }

    public void Camera_Effect_Gray(){
        StartCoroutine(EffectGray());
    }

    IEnumerator EffectGray(){
        float elapsedTime = 0.0f;
        
        while(elapsedTime < appliedTime){
            elapsedTime += Time.deltaTime;
            grayScale = elapsedTime / appliedTime;
            yield return null;
        }
    }
}
