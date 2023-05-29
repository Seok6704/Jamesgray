using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GetCellPos : MonoBehaviour
{
    public Tilemap border;
    public Tilemap map;

    private void Start() 
    {
        //PathFinding.AStar aStar = new PathFinding.AStar(ref PositionManager.tilemap, ref border, new Vector2Int(0,0), new Vector2Int(-8, -10));
        
    }

    private void Update() 
    {
        Vector3Int temp;
        if(Input.GetMouseButtonDown(0))
        {
            temp = border.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Debug.Log("tileMap " + PositionManager.tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            Debug.Log("Border " + temp);
            Debug.Log(border.GetSprite(temp)?.name);
            Debug.Log("world " + border.GetCellCenterWorld(temp));
            
            transform.position = map.GetCellCenterWorld(temp);
        }
    }
}
