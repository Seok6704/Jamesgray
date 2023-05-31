using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class ShowPath : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public GameObject waypoint;
    public Tilemap map;
    public Tilemap border;

    public GameObject Up, Down, Right, Left;

    private void Start() 
    {   
        Vector3Int startPos = start.GetComponent<PositionManager>().GetCellPos();
        Vector3Int endPos = end.GetComponent<PositionManager>().GetCellPos();

        PathFinding.AStar path = new PathFinding.AStar(ref map, ref border, new Vector2Int(startPos.x, startPos.y), new Vector2Int(endPos.x, endPos.y));
        int count = 0;
        while(path.Close.Count > 0)
        {
            Vector3 pos = map.GetCellCenterWorld((Vector3Int)path.Close.First.Value);
            GameObject temp = Instantiate(waypoint,pos, Quaternion.identity);
            temp.GetComponent<TMP_Text>().text = count.ToString();

            SetArrow(path.Close.First.Value, path.ClosePre.First.Value);

            path.ClosePre.RemoveFirst();
            path.Close.RemoveFirst();
            count++;
        }
    }

    void SetArrow(Vector2Int current, Vector2Int pre)
    {
        Vector3 pos = map.GetCellCenterWorld((Vector3Int)current);
        Vector2Int temp = current - pre;
        if(temp == Vector2Int.down)
        {
            Instantiate(Up, pos, Quaternion.Euler(0,0,0));
        }
        else if(temp == Vector2Int.up)
        {
            //Instantiate(Down, pos, Quaternion.identity);
            Instantiate(Up, pos, Quaternion.Euler(0,0,180));
        }
        else if(temp == Vector2Int.left)
        {
            //Instantiate(Left, pos, Quaternion.identity);
            Instantiate(Up, pos, Quaternion.Euler(0,0,-90));
        }
        else if(temp == Vector2Int.right)
        {
            //Instantiate(Right, pos, Quaternion.identity);
            Instantiate(Up, pos, Quaternion.Euler(0,0,90));
        }
    }
}
