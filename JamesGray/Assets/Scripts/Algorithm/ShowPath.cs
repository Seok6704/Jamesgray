using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShowPath : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public GameObject waypoint;
    public Tilemap map;
    public Tilemap border;

    private void Start() 
    {   
        Vector3Int startPos = start.GetComponent<PositionManager>().GetCellPos();
        Vector3Int endPos = end.GetComponent<PositionManager>().GetCellPos();

        PathFinding.AStar path = new PathFinding.AStar(ref PositionManager.tilemap, ref PositionManager.border, new Vector2Int(startPos.x, startPos.y), new Vector2Int(endPos.x, endPos.y));

        while(path.Close.Count > 0)
        {
            Vector3 pos = map.GetCellCenterWorld((Vector3Int)path.Close.First.Value);
            Instantiate(waypoint,pos, Quaternion.identity);
            path.Close.RemoveFirst();
        }
    }
}
