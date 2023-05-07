using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ColotChange : MonoBehaviour
{

    public TMP_Text T;
    byte r = 255, g = 0, b = 0;
    int count, rgb = 0;
    bool flag = true;

    void Update()
    {
        //byte r = (byte)UnityEngine.Random.Range(0, 255);    //랜덤값을 사용해서 랜덤한 색상 표시
        //byte g = (byte)UnityEngine.Random.Range(0, 255); 
        //byte b = (byte)UnityEngine.Random.Range(0, 255);
        //T.color = new Color32(r, g, b, 255);
        

        //15씩 값을 증가하여 색 표현
        T.color = new Color32(r, g, b, 255);
        if (flag) count += 5;       //15는 좀 부자연스러운 것 같음
        else count -= 5;

        if (rgb == 0) g = (byte)count;
        else if (rgb == 1) r = (byte)count;
        else if (rgb == 2) b = (byte)count;

        if(count >= 255 || count <= 0)
		{
            flag = !flag;
            rgb++;
            if (rgb == 3) rgb = 0;
		}
        
    }

}
