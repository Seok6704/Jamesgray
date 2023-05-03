using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Image_Grayer : MonoBehaviour
{
    public Image img;

    Color color;

    void Start() {
        color = img.color;
        color = new Color((color.r + color.g + color.b)/3,(color.r + color.g + color.b)/3,(color.r + color.g + color.b)/3);
        img.color = color;
    }
}
