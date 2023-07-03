using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonData : MonoBehaviour
{
    string command;
    string data;

    public void SetData(string com, string data)
    {
        command = com;
        this.data = data;
    }

    public void Onclick()
    {
        GameObject.FindWithTag("Dialogue").GetComponent<DialoguesManager>().GetFromButton(command, data);
    }
}
