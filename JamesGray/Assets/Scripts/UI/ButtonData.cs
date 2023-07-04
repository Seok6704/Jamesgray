using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonData : MonoBehaviour
{
    string command;
    string data;

    sbyte num;

    public void SetData(string com, string data, sbyte num)
    {
        command = com;
        this.data = data;
        this.num = num;
    }

    public void Onclick()
    {
        GameObject.FindWithTag("Dialogue").GetComponent<DialoguesManager>().GetFromButton(command, data, num);
    }
}
