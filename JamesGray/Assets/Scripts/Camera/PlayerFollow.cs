using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어의 이동에 따라 따라가는 카메라 구현
public class PlayerFollow : MonoBehaviour
{
    public float camera_Speed;
    public Vector2 dialogue_mod;
    bool dialogueOn = false;
    public GameObject player;

    private void Update() 
    {   
        sbyte mod = 0;      // -128 ~ 127
        Vector3 dir = player.transform.position - this.transform.position;
        if(dialogueOn)
        {
            mod = 1;
        }
        Vector3 moveVector = new Vector3((dir.x * camera_Speed + (dialogue_mod.x * mod)) * Time.deltaTime, (dir.y * camera_Speed + (dialogue_mod.y * mod)) * Time.deltaTime, 0.0f);

        this.transform.Translate(moveVector);
    }

    public void SetFlag()   //이벤트로 호출되면 플레그를 반전, 다이얼로그에서 호출
    {
        dialogueOn = !dialogueOn;
    }
}
