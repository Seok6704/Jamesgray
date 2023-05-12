using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
        타일맵에 위치한 오브젝트들(NPC)의 위치를 타일맵의 그리드에 맞게 위치하게 만들어 주는 스크립트입니다.
        만약 타일맵에 들어맞게 위치하는게 싫다면 해당 스크립트를 제거해주세요.
*/

public class PositionManager : MonoBehaviour
{
    static Tilemap tilemap;
    private void Awake() 
    {
        if(tilemap == null) tilemap = GameObject.FindWithTag("Map").GetComponent<Tilemap>();    //tag가 map으로 지정된 오브젝트에서 타일맵 컴포넌트 불러오기

        Vector3 temp = tilemap.CellToWorld(tilemap.WorldToCell(this.transform.position));    //위치 변경
        temp.x += 0.5f;
        temp.y += 0.5f;

        this.transform.position = temp;
    }
}
