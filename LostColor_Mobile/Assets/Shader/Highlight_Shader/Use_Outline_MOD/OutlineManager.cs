using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutlineManager : MonoBehaviour
{
    
    public Color effectColor;
    bool b_on_select;
    Outline outline;
    Color defaultColor;
    private void Start() {
        b_on_select = false;
        outline = GetComponent<Outline>();
        //defaultColor = outline.effectColor;
        effectColor = new Color(effectColor.r, effectColor.g, effectColor.b, 1.0f);
        defaultColor = new Color(effectColor.r, effectColor.g, effectColor.b, 0.0f);
    }

    public void SelectThis(){
        b_on_select = true;
    }

    public void UnSelectThis(){
        b_on_select = false;
    }

    public void ToggleSelect(){
        b_on_select = !b_on_select;
    }

    private void Update() {
        if(b_on_select){
            outline.effectColor = effectColor;
        }
        else
            outline.effectColor = defaultColor;
    }
}
