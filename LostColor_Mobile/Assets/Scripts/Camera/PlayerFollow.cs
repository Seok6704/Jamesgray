using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어의 이동에 따라 따라가는 카메라 구현
public class PlayerFollow : MonoBehaviour
{
    public float camera_Speed;

    public GameObject player;

    private void Update() 
    {
        Vector3 dir = player.transform.position - this.transform.position;

        Vector3 moveVector = new Vector3(dir.x * camera_Speed * Time.deltaTime, dir.y * camera_Speed * Time.deltaTime, 0.0f);

        this.transform.Translate(moveVector);
    }
}
