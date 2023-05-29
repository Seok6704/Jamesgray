using System.Collections;
using System.Collections.Generic;
using DataStructure;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathFinding
{
    /*A* 알고리즘*/
    public class AStar
    {
        public LinkedList<Vector2Int> Close = new LinkedList<Vector2Int>();
        Vector2Int[] way = {new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1)};
        
        public AStar(ref Tilemap map, ref Tilemap colider, Vector2Int start, Vector2Int end)
        {
            CalcPath(ref map, ref colider, start, end);
            //CalcPath(ref PositionManager.tilemap, ref PositionManager.border, start, end);
        }

        void CalcPath(ref Tilemap map, ref Tilemap colider, Vector2Int start, Vector2Int end)
        {
            PriorityQueue<ANode> Open = new PriorityQueue<ANode>();
            LinkedList<Vector2Int> OpenList = new LinkedList<Vector2Int>();

            ANode startNode = new ANode();
            startNode.pos = start;
            startNode.g = 0; startNode.h = CalcHeuristic(start, end);
            Open.Enqueue(startNode, startNode.f);
            OpenList.AddFirst(startNode.pos);

            while(Open.count > 0)
            {
                ANode temp = Open.Dequeue();
                OpenList.Remove(OpenList.Find(temp.pos));

                if(Close.Contains(temp.pos)) //이미 방문했다면 스킵
                    continue;

                Close.AddFirst(temp.pos);

                if(temp.pos == end) break;  //목적지 도착시 종료

                for(int i = 0; i < way.Length; i++)
                {
                    if(!CheckTile(ref colider, temp.pos + way[i]))  //벽이라면 스킵
                        continue;    
                    if(Close.Contains(temp.pos + way[i]))   //방문했다면 스킵
                        continue;
                    if(OpenList.Contains(temp.pos + way[i]))
                        continue;
                    ANode iter = new ANode();
                    iter.pos = temp.pos + way[i];
                    iter.g = temp.g + 1; iter.h = CalcHeuristic(iter.pos, end);
                    Open.Enqueue(iter, iter.f);
                    OpenList.AddFirst(iter.pos);
                }
            }
        }
        bool CheckTile(ref Tilemap colider, Vector2Int tilePos)
        {
            //string coliderName = colider.GetSprite(new Vector3Int(tilePos.x, tilePos.y, 0)).name;

            if(colider.GetSprite(new Vector3Int(tilePos.x, tilePos.y, 0)) != null)
            {
                return false;
            }

            return true;
        }

        int CalcHeuristic(Vector2Int start, Vector2Int end)
        {
            int a = Mathf.Abs(start.x - end.x);
            int b = Mathf.Abs(start.y - end.y);
            //return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
            return (a * a) + (b * b);
        }

        class ANode
        {
            public int g, h;       //g : 출발 노드에서 현재 노드까지 도달하기 위한 최단 비용 , h : 현재 노드에서 목표 노드까지 예상 이동 비용 (휴리스틱 거리 측정값)
            public Vector2Int pos;
            public int f { get { return g + h; } } //휴리스틱 비용 함수 f = g + h
        }
    }
}
