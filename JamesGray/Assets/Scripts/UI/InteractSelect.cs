using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*

    !!! 사용법 -> Gitlab/Wiki 에 사용법을 적어놓았습니다. !!!


    이 스크립트는 씬의 아무곳에 위치하면 동작합니다. 아웃라인 두께, 색상, 캔버스 오브젝트를 설정해주고 난뒤, 이벤트 등으로 UpdateNode() 함수를 실행하면
    현재 시점의 캔버스 화면내부에 있는 모든 버튼을 키보드로 선택할수있도록 구성합니다.
    하이라이트 기능을 사용하기 위해서는 버튼 객체가 Outline 컴포넌트를 가지고 있거나 버튼객체의 첫번째 자식 오브젝트가 TMPro Text 컴포넌트를 가지고 있어야합니다. (혹은 둘다) 만약 둘다 없다면 아무런 반응도 보이지 않을것입니다.(비록 선택은 되고있더라도)
    
    자세한 로직은 주석으로 적기 어렵기 때문에, 간단하게 설명하도록 하겠습니다.
    1. 씬내부의 모든 버튼 객체를 불러옵니다. - SetButtons()
    2. 모든 버튼 객체를 대상으로 현재 캔버스 시점에 존재하는지 검사합니다. - SetNodeMap()
    3. 캔버스 시점에 존재하는 모든 버튼 객체를 대상으로 4 방향 (상하좌우)에 대해 가장 가까운 객체를 참조하도록 합니다. 

    아직 검증된 동작은 아니지만, 실시간으로 노드간 연결을 업데이트 하고 싶다면 Update() 함수 내부에 UpdateNode() 함수를 실행합니다.
    다만, 제대로 동작할지는 모르겠습니다. 시간 복잡도가 N^2 의 형태로 간선을 연결하기때문에, 상당히 느리지만, 버튼의 수가 그렇게 많지 않기때문에 큰 부담은 없을것이라 생각됩니다.

    주의사항, 만약 캔버스 화면이 업데이트되어 버튼들의 위치가 변경된다면, Update해야 정상적으로 동작합니다.
*/

/// <summary>
/// 캔버스 뷰에서 상호작용 가능한 버튼들을 방향키로 선택 및 하이라이트
/// </summary>

public class InteractSelect : MonoBehaviour
{
    public Color color; //하이라이트로 사용할 색상
    [Range(0,5)]
    public float outlinePadding;    //아웃라인 두께

    [SerializeField]
    List<GameObject> buttons;
    ButtonNode defaultObj, currentObj;
    //private GameObject[] buttons;    //상호작용할 버튼들
    public Canvas canvas;           
    RectTransform rect;
    Vector2 min, max, dir;               //캔버스기준 최소 최대 좌표

    private void Start() 
    {
        rect = canvas.GetComponent<RectTransform>();
        max.x = rect.rect.width/2;
        max.y = rect.rect.height/2;
        min.x = max.x * -1;
        min.y = max.y * -1;

        dir = new Vector2(1,1).normalized;  // 길이가 1인 1,1로 향하는 방향 벡터
        SetButtons();
    }
    public void UpdateNode()    //현재 시점의 캔버스의 버튼 요소들을 키보드로 선택할수있도록 설정하는 실행함수
    {
        SetNodeMap();
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            ResetOutline();
            currentObj = currentObj.GetNearby(0);
            SetOutline();
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            ResetOutline();
            currentObj = currentObj.GetNearby(1);
            SetOutline();
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ResetOutline();
            currentObj = currentObj.GetNearby(2);
            SetOutline();
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ResetOutline();
            currentObj = currentObj.GetNearby(3);
            SetOutline();
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            currentObj.me.GetComponent<Button>().onClick.Invoke();
        }
    }

    void SetOutline()
    {
        if(currentObj.me.transform.childCount != 0)
        {
            currentObj.me.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().outlineWidth = outlinePadding;
            currentObj.me.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().outlineColor = color;
        }
        Outline outline = null;
        currentObj.me.TryGetComponent<Outline>(out outline);
        if(outline)
        {
            outline.effectColor = color;
            outline.effectDistance = new Vector2(outlinePadding * 10, outlinePadding * 10);
        }
    }
    void ResetOutline()
    {
        if(currentObj.me.transform.childCount != 0)
        {
            currentObj.me.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().outlineWidth = 0f;
        }
        Outline outline = null;
        currentObj.me.TryGetComponent<Outline>(out outline);
        if(outline)
        {
            outline.effectDistance = new Vector2(0f, 0f);
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

    void SetButtons()   //모든 오브젝트를 불러온 뒤, 버튼인지 체크
    {
        GameObject[] allObj = FindObjectsOfType<GameObject>();
        for(int i = 0; i < allObj.Length; i++)
        {
            Button temp;
            if(allObj[i].TryGetComponent<Button>(out temp))
            {
                buttons.Add(allObj[i]);
            }
        }
    }

    void SetNodeMap()
    {
        int i;
        Vector2 priority = min;
        List<ButtonNode> onCanvas = new List<ButtonNode>(); //캔버스 뷰에 있는 오브젝트를 저장할 임시 리스트
        for(i = 0; i < buttons.Count; i++)  //씬에 존재하는 모든 버튼에 대해 캔버스시점에 존재하는지 검사
        {
            Vector3 temp = WorldToCanvasPoint(buttons[i].transform.position);
            if(isInCanvas(temp))
            {
                onCanvas.Add(new ButtonNode(buttons[i]));
                if(temp.x > priority.x || (temp.x == priority.x && temp.y < priority.y))    //가장 위에 있거나, 만약 가장 위에 있는 오브젝트가 또 있다면 가장 왼쪽에 있는 오브젝트 선택
                {
                    priority.x = temp.x; priority.y = temp.y;
                    defaultObj = onCanvas[onCanvas.Count-1];    //마지막 노드가 최신 노드이므로
                }
            }
        }
        if(onCanvas.Count == 0) return;
        
        currentObj = defaultObj;

        for(i = 0; i < onCanvas.Count; i++) //모든 노드에 대해서 가장 가까운 곳에 대해 간선 연결
        {
            for(int j = 0; j < onCanvas.Count; j++)
            {
                if(j != i)      //자기 자신은 스킵
                {
                    int nDir = GetDirByFour(onCanvas[j].me.transform.position - onCanvas[i].me.transform.position);
                    int distance = GetDistance(onCanvas[i].me.transform.position, onCanvas[j].me.transform.position);
                    if(onCanvas[i].distance[nDir] > distance)
                    {
                        onCanvas[i].SetNearby(onCanvas[j], nDir, distance);
                    }
                }   
            }
        }
    }

    int GetDistance(Vector3 A, Vector3 B)   //피타고라스를 사용하여 A와 B사이 거리를 구하기, 성능을 위해 루트 연산은 스킵
    {
        //float h = Mathf.Abs(A.y - B.y), w = Mathf.Abs(A.x - B.x);   //세로, 가로 구하기
        //return (int)((h * h) + (w * w));
        return (int)(A - B).sqrMagnitude;   //위 식이랑 같은 결과
    }
    int GetDirByFour(Vector3 dir)   //벡터 방향은  A - B로 구할수있다.
    {
        dir = dir.normalized;
        if(dir.x >= this.dir.x) return 3;   //right
        else if(dir.x <= this.dir.x * -1) return 2; //left
        else if(dir.y >= this.dir.y) return 0; //up
        else if(dir.y <= this.dir.y * -1) return 1; // down
        else return 1;  //temp
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
    }
}
