using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JOYSTICKTEST : MonoBehaviour
{
    public TMP_Text X, Y;
    public GameObject circle;
    
    SerialCOM serial;

    Vector3 pos;
    private void Awake() {
        pos = circle.transform.position;
        //serial = GetComponent<SerialCOM>();
        serial = new SerialCOM(9600, 7);
        Debug.Log(System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
    }
    private void Update() {
        //Debug.Log("X : " + Input.GetAxis("Horizontal") + "   Y : " + Input.GetAxis("Vertical"));
        //float x = Input.GetAxis("Horizontal"); float y = Input.GetAxis("Vertical");
        char input = serial.GetInput();
        Vector3 dir = Vector3.zero;

        switch(input)
        {
            case 'n':
                dir = Vector3.zero;
                break;
            case 'w':
                dir = Vector3.up;
                break;
            case 's':
                dir = Vector3.down;
                break;
            case 'a':
                dir = Vector3.left;
                break;
            case 'd':
                dir = Vector3.right;
                break;
        }


        //X.text = x.ToString();
        //Y.text = y.ToString();

        X.text = dir.x.ToString();
        Y.text = dir.y.ToString();


        circle.transform.position = new Vector3(pos.x + dir.x, pos.y + dir.y , pos.z);
    }
}
