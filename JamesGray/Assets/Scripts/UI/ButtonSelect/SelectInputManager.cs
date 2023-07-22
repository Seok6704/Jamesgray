using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectInputManager : MonoBehaviour
{
    public Color color; //하이라이트로 사용할 색상
    [Range(0,5)]
    public float outlinePadding;    //아웃라인 두께

    RectTransform rect;
    Vector2 min, max, dir;               //캔버스기준 최소 최대 좌표

    ButtonNode currentObj;

    List<ButtonNode> buttons;

    bool updateFlag;

    Canvas canvas;

    void Awake() 
    {
        buttons = new List<ButtonNode>();
        currentObj = null;
        updateFlag = false;
        canvas = FindAnyObjectByType<Canvas>();
    }

    void Start()
    {
        rect = canvas.GetComponent<RectTransform>();
        max.x = rect.rect.width/2;
        max.y = rect.rect.height/2;
        min.x = max.x * -1;
        min.y = max.y * -1;

        dir = new Vector2(1,1).normalized;  // 길이가 1인 1,1로 향하는 방향 벡터
    }

    public void AddNewButton(GameObject btn)
    {
        ButtonNode newNode = new ButtonNode(btn);
        buttons.Add(newNode);
        updateFlag = true;
    }

    void Update() 
    {
        if(currentObj == null)
        {
            return;
        }
        else if(updateFlag) //리스트의 값이 변경되었으므로 업데이트하기
        {
            updateFlag = false;
            UpdateNodeMap();
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            currentObj.ResetOutline();
            currentObj = currentObj.GetNearby(0);
            currentObj.SetOutline(color, outlinePadding);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            currentObj.ResetOutline();
            currentObj = currentObj.GetNearby(1);
            currentObj.SetOutline(color, outlinePadding);
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            currentObj.ResetOutline();
            currentObj = currentObj.GetNearby(2);
            currentObj.SetOutline(color, outlinePadding);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            currentObj.ResetOutline();
            currentObj = currentObj.GetNearby(3);
            currentObj.SetOutline(color, outlinePadding);
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            currentObj.me.GetComponent<Button>().onClick.Invoke();
        }
    }

    /// <summary>
    /// 리스트에 있는 버튼들 간선으로 연결하기
    /// </summary>
    void UpdateNodeMap()
    {
        int i;
        Vector3 tempA, tempB;
        Vector2 priority = min;
        List<ButtonNode> nodeList = new List<ButtonNode>(); //리스트중 캔버스 내부에 있는 버튼만 모이는 리스트
        bool firstUpdate = false;
        
        if(currentObj == null || (currentObj != null && (!isInCanvas(WorldToCanvasPoint(currentObj.me.transform.position)) || !currentObj.me.activeSelf)))  //첫 업데이트라서 currentObj가 null일때, 혹은 currentObj가 캔버스뷰 밖일때,
            firstUpdate = true;

        for(i = buttons.Count - 1; i >= 0; i--) //거꾸로 순회시 리스트 중간에 원소가 삭제되어도 안전하다고함
        {
            tempA = WorldToCanvasPoint(buttons[i].me.transform.position);
            if(isInCanvas(tempA))
            {
                nodeList.Add(buttons[i]);
                if(firstUpdate && (tempA.x > priority.x || (tempA.x == priority.x && tempA.y < priority.y)))    //만약 첫 업데이트라 currentObj가 null이라면 가장 왼쪽 위 오브젝트를 current로 할당
                {
                    priority.x = tempA.x; priority.y = tempA.y;
                    currentObj = buttons[i];
                }
            }
        }
        
        for(i = nodeList.Count - 1; i >= 0; i--)
        {
            for(int j = nodeList.Count - 1; j >= 0; j--)
            {
                if(j != i)      //자기 자신은 스킵
                {
                    tempA = nodeList[j].me.transform.position; tempB = nodeList[i].me.transform.position;

                    int nDir = GetDirByFour(tempA - tempB);
                    int distance = GetDistance(tempB, tempA);
                    if(nodeList[i].distance[nDir] > distance)
                    {
                        nodeList[i].SetNearby(nodeList[j], nDir, distance);
                    }
                }   
            }
        }
    }

    public Vector3 WorldToCanvasPoint(Vector3 pos) //월드 좌표를 canvas 좌표로 변경
    {
        return new Vector3(pos.x - canvas.transform.position.x, pos.y - canvas.transform.position.y, pos.z - canvas.transform.position.z);
    }

    public bool isInCanvas(Vector3 canvasView) //캔버스 시점 내부에 존재하는지 검사
    {
        //Vector3 canvasView = WorldToCanvasPoint(pos);
        if(canvasView.x >= min.x && canvasView.x <= max.x && canvasView.y >= min.y && canvasView.y <= max.y)
            return true;

        else return false;
    }

    /// <summary>
    /// 피타고라스의 정리를 이용한 A to B의 거리 구하기, 성능을 위해 루트 연산 생략
    /// </summary>
    int GetDistance(Vector3 A, Vector3 B)
    {
        //float h = Mathf.Abs(A.y - B.y), w = Mathf.Abs(A.x - B.x);   //세로, 가로 구하기
        //return (int)((h * h) + (w * w));
        return (int)(A - B).sqrMagnitude;   //위 식이랑 같은 결과
    }

    /// <summary>
    /// 몇번째 방향인지 알려주는 함수
    /// </summary>
    /// <param name="dir"> A - B로 구한 방향 벡터 </param>
    int GetDirByFour(Vector3 dir)   //벡터 방향은  A - B로 구할수있다.
    {
        dir = dir.normalized;
        if(dir.x >= this.dir.x) return 3;   //right
        else if(dir.x <= this.dir.x * -1) return 2; //left
        else if(dir.y >= this.dir.y) return 0; //up
        else if(dir.y <= this.dir.y * -1) return 1; // down
        else return 1;  //temp
    }

    public void SetEnabled(bool enable)
    {
        if(SettingManager.onVirtualPad) //만약 설정에서 가상 패드가 켜져 있다면 그냥 비활성화 하기.
            enable = false;

        this.enabled = enable;
    }

    public void SetEnalbe()
    {
        SetEnabled(true);
    }
    public void SetDisable()
    {
        SetEnabled(false);
    }

    class ButtonNode
    {
        sbyte nWay = 4;     //4 방향
        ButtonNode[] nearbyNode;    //0 : up, 1 : down, 2 : left, 3 : right
        public int[] distance; //각 노드간 거리
        public GameObject me;

        public ButtonNode(GameObject me)
        {
            this.me = me;
            nearbyNode = new ButtonNode[nWay];
            distance = new int[nWay];
            for(sbyte i = 0; i < nWay; i++)
            {
                distance[i] = int.MaxValue;
                nearbyNode[i] = null;
            }
        }

        public ButtonNode GetNearby(int way)    //0 : up, 1 : down, 2 : left, 3 : right
        {
            if(nearbyNode[way] == null) return this;
            return nearbyNode[way];
        }
        public ButtonNode GetUP()
        {
            return GetNearby(0);
        }
        public ButtonNode GetDOWN()
        {
            return GetNearby(1);
        }
        public ButtonNode GetLEFT()
        {
            return GetNearby(2);
        }
        public ButtonNode GetRIGHT()
        {
            return GetNearby(3);
        }

        public void SetNearby(ButtonNode node, int way, int distance)
        {
            nearbyNode[way] = node;
            this.distance[way] = distance;
        }

        public void SetUP(ButtonNode node, int distance)
        {
            SetNearby(node, 0, distance);
        }
        public void SetDOWN(ButtonNode node, int distance)
        {
            SetNearby(node, 1, distance);
        }
        public void SetLEFT(ButtonNode node, int distance)
        {
            SetNearby(node, 2, distance);
        }
        public void SetRIGHT(ButtonNode node, int distance)
        {
            SetNearby(node, 3, distance);
        }

        public void SetOutline(Color color, float outlinePadding)
        {
            if(me.transform.childCount != 0)
            {
                TMPro.TMP_Text text = null;
                me.transform.GetChild(0).TryGetComponent<TMPro.TMP_Text>(out text);
                if(text)
                {
                    text.gameObject.SetActive(false);
                    text.outlineColor = color;
                    text.outlineWidth = outlinePadding;
                    text.gameObject.SetActive(true);
                }
            }
            Outline outline = null;
            me.TryGetComponent<Outline>(out outline);
            if(outline)
            {
                outline.effectColor = color;
                outline.effectDistance = new Vector2(outlinePadding * 10, outlinePadding * 10);
            }
        }
        public void ResetOutline()
        {
            if(me.transform.childCount != 0)
            {
                TMPro.TMP_Text text = null;
                me.transform.GetChild(0).TryGetComponent<TMPro.TMP_Text>(out text);
                if(text != null)
                {
                    text.gameObject.SetActive(false);
                    text.outlineWidth = 0.001f;
                    text.outlineWidth = 0f;
                    text.gameObject.SetActive(true);
                }
            }
            Outline outline = null;
            me.TryGetComponent<Outline>(out outline);
            if(outline != null)
            {
                outline.effectDistance = new Vector2(0f, 0f);
            }
        }
    }
}
