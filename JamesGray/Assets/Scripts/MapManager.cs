using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
    A* 알고리즘을 사용하여 경로를 찾아주는 스크립트
*/

public class MapManager
{
    //public Tilemap tileMap; //플레이 맵
    //Vector2Int[] moveVec = new Vector2Int[]{new Vector2Int(0,1), new Vector2Int(0,-1), new Vector2Int(-1,0), new Vector2Int(1,0)};  //0 : 위, 1 : 아래, 2 : 왼쪽, 3 : 오른쪽



}

class TileNode  //각 타일 데이터 저장
{
    bool isAccessible;      //해당 타일이 접근가능한지
    bool[] availableWay;    //해당 타일에서 이동가능한 방향 0 : 위, 1 : 아래, 2 : 왼쪽, 3 : 오른쪽

    TileNode()
    {
        isAccessible = false;
        availableWay = new bool[]{false, false, false, false};
    
    }
}
