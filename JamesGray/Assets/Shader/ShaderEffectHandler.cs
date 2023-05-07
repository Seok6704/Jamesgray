using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderEffectHandler : MonoBehaviour
{
    public Material shadowMaterial;

    [Range(0,1)]
    public float shadowThreshold; //Debug 용
    [Range(0,1)]
    public float shadowSpeed;

    void Start() {
        shadowThreshold = 0.0f;
    }
    public void MakeGrayer(){
        StartCoroutine(SetGray());
    }

    public void MakeColor(){
        StartCoroutine(SetColor());
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if(shadowThreshold == 0) 
        {
            Graphics.Blit(src,dest);
            return;
        }
        shadowMaterial.SetFloat("_ShadowThreshold", shadowThreshold);
        Graphics.Blit(src, dest, shadowMaterial);
   }

    IEnumerator SetGray(){
        for(; shadowThreshold <= 1.0f; shadowThreshold += shadowSpeed) {
            if(shadowThreshold >= 1.0f) break;
            yield return null;
        }

        shadowThreshold = 1.0f;
    }

    public void ClearGray(){    //바로 그레이 쉐이더 꺼버리기
        shadowThreshold = 0.0f;
    }

    IEnumerator SetColor(){     //gray 쉐이더 이펙트 서서히 줄이기
        for(; shadowThreshold >= 0.0f; shadowThreshold -= shadowSpeed) {
            if(shadowThreshold <= 0.0f) break;
            yield return null;
        }
    
        shadowThreshold = 0.0f;
    }

}
