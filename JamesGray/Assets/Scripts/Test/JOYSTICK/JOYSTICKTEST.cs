using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JOYSTICKTEST : MonoBehaviour
{
    public TMP_Text X, Y;
    public GameObject circle;

    Vector3 pos;
    private void Awake() {
        pos = circle.transform.position;
        Debug.Log(System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
    }
    private void Update() {
        //Debug.Log("X : " + Input.GetAxis("Horizontal") + "   Y : " + Input.GetAxis("Vertical"));
        float x = Input.GetAxis("Horizontal"); float y = Input.GetAxis("Vertical");
        X.text = x.ToString();
        Y.text = y.ToString();

        circle.transform.position = new Vector3(pos.x + x, pos.y + y , pos.z);
    }
}
