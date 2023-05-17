using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GetCellPos : MonoBehaviour
{
    public Tilemap border;

    private void Update() 
    {
        Vector3Int temp;
        if(Input.GetMouseButtonDown(0))
        {
            temp = border.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Debug.Log("tileMap " + PositionManager.tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            Debug.Log("Border " + temp);
            //Debug.Log(border.GetSprite(temp));
            Debug.Log("world " + border.GetCellCenterWorld(temp));
            
        }
    }
}
