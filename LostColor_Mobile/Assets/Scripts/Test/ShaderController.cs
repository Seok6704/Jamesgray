using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderController : MonoBehaviour
{
    public Material shadowMaterial;//
    private void OnRenderImage(RenderTexture src, RenderTexture dest) 
    {
        Graphics.Blit(src, dest, shadowMaterial);
    }

}
