using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
씬 시작 시, 플레이어 위치를 원하는 곳에 배치하기 위한 스크립트입니다.
*/

public class PlayerPositionManager : MonoBehaviour
{
    public Transform player;

    void Awake()
    {
        player.transform.position = new Vector3(-2, -14, 0);     
    }
}
