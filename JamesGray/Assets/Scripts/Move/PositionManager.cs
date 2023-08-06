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
    public static Tilemap tilemap = null;
    public static Tilemap border = null;
    private void Awake() 
    {
        if(ReferenceEquals(tilemap, null)) tilemap = GameObject.FindWithTag("Map")?.GetComponent<Tilemap>();    //tag가 map으로 지정된 오브젝트에서 타일맵 컴포넌트 불러오기
        if(ReferenceEquals(border, null)) border = GameObject.FindWithTag("Border")?.GetComponent<Tilemap>();

        Vector3 temp = !ReferenceEquals(tilemap, null) ? tilemap.GetCellCenterWorld(tilemap.WorldToCell(this.transform.position)) : new Vector3(0f,0f,0f); //위치 변경

        this.transform.position = temp;
    }

    public Vector3Int GetCellPos()
    {
        if(ReferenceEquals(tilemap, null)) return default(Vector3Int);
        
        return tilemap.WorldToCell(this.transform.position);
    }

    /// <summary>
    /// 입력된 위치를 타일맵으로 변환하여 해당 위치로 이동, 세이브 로드시 사용
    /// </summary>
    public void SetPos(Vector3 pos)
    {
        transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(pos));
    }

    public static void ResetTilemap()  //Static 변수는 씬전환 후에도 남아있기 때문에 싱글톤 디자인 외에도 씬 전환시 초기화가 필요함.
    {
        tilemap = null; border = null;
    }

    void OnDestroy()    //씬 종료시 변수 초기화
    {
        ResetTilemap();
    }
}
