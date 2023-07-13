using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Panel마다 붙여서 자식들 중 상호작용가능한 버튼들을 간선으로 연결하여 컨트롤러로 조작할 수 있게함
/// </summary>
public class ControlPadInteract : MonoBehaviour
{
    public Color color; //하이라이트로 사용할 색상
    [Range(0,5)]
    public float outlinePadding;    //아웃라인 두께
    [Header("시작시 활성화")]
    public bool onStart;

    [SerializeField]
    List<Button> buttons;
    ButtonNode defaultObj, currentObj;

    RectTransform rect;
    Vector2 min, max, dir;
    void Start()
    {
        defaultObj = null;
        currentObj = null;
        buttons = new List<Button>();

        rect = this.GetComponent<RectTransform>();  //Panel의 View 특정하기
        max.x = rect.rect.width/2;
        max.y = rect.rect.height/2; 
        min.x = max.x * -1;
        min.y = max.y * -1;

        dir = new Vector2(1,1).normalized;  // 길이가 1인 좌표 1,1로 향하는 방향 벡터

       UpdateChain(onStart); //기본적으로 이 스크립트는 비활성화 만약 onStart가 참이라면 시작시 활성화
    }

    void Update()   //일단 임시로 키보드로 조작 가능하게 설정
    {
        if(currentObj == null) return;
        
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

    /// <summary>
    /// 전부 초기화 후 새롭게 노드맵 설정
    /// </summary>
    public void UpdateNode()
    {
        FindButtons();
        SetNodeMap();
    }

    public void SetEnabled(bool enable)
    {
        if(SettingManager.onVirtualPad) //만약 설정에서 가상 패드가 켜져 있다면 그냥 비활성화 하기.
            enable = false;

        this.enabled = enable;
    }
    /// <summary>
    /// 업데이트와 활성화를 동시에... 만약 비활성화상태라면 초기화를... 활성화 상태라면 업데이트를 한다. 사용의 편의성을 위해 한가지 함수에 몰아넣었다.
    /// </summary>
    public void UpdateChain(bool enable)    //이 함수를 사용하길 권장...
    {
        if(SettingManager.onVirtualPad) //만약 설정에서 가상 패드가 켜져 있다면 그냥 비활성화 하기.
            enable = false;
        
        Debug.Log(enable + "!!!");
        if(enable)
        {
            this.enabled = enable;
            UpdateNode();
        }
        else        //초기화
        {
            this.enabled = enable;
            defaultObj = null; currentObj = null;
            buttons.Clear();
        }
        
    }

    public void SetEnalbe()
    {
        SetEnabled(true);
    }
    public void SetDisable()
    {
        SetEnabled(false);
    }

    /// <summary>
    /// 자식들 중 Panel View 내부의 버튼들을 찾아내서 리스트에 등록하기
    /// </summary>
    void FindButtons()
    {   
        if(buttons.Count > 0)   //리스트가 차있다면 비우기
        {
            buttons.Clear();
        }

        Button[] btns = this.GetComponentsInChildren<Button>();   //자식들 중 버튼 가진 오브젝트 전부 가져오기

        foreach (Button child in btns)  //이들 중 비활성화 이거나 Panel View 외부의 버튼은 제외하여 리스트에 추가
        {
            if(child.IsActive() && CheckPanelView(child.transform.position))
            {
                buttons.Add(child);
            }
        }
    }
    /// <summary>
    /// 리스트에 있는 버튼들 간선으로 연결하기
    /// </summary>
    void SetNodeMap()
    {
        int i;
        Vector2 priority = min;
        List<ButtonNode> nodeList = new List<ButtonNode>(); //노드를 저장할 임시 리스트
        
        for(i = 0; i < buttons.Count; i++)
        {
            Vector3 temp = WorldToPanelPoint(buttons[i].transform.position);
            ButtonNode newNode = new ButtonNode(buttons[i].gameObject);
            nodeList.Add(newNode);
            if(temp.x > priority.x || (temp.x == priority.x && temp.y < priority.y))    //가장 위에 있거나, 만약 가장 위에 있는 오브젝트가 또 있다면 가장 왼쪽에 있는 오브젝트 선택
            {
                    priority.x = temp.x; priority.y = temp.y;
                    defaultObj = newNode;    //마지막 노드가 최신 노드이므로
            }
        }
        if(nodeList.Count <= 0) return;

        currentObj = defaultObj;

        for(i = 0; i < nodeList.Count; i++) //모든 노드에 대해서 가장 가까운 곳에 대해 간선 연결
        {
            for(int j = 0; j < nodeList.Count; j++)
            {
                if(j != i)      //자기 자신은 스킵
                {
                    int nDir = GetDirByFour(nodeList[j].me.transform.position - nodeList[i].me.transform.position);
                    int distance = GetDistance(nodeList[i].me.transform.position, nodeList[j].me.transform.position);
                    if(nodeList[i].distance[nDir] > distance)
                    {
                        nodeList[i].SetNearby(nodeList[j], nDir, distance);
                    }
                }   
            }
        }
    }

    void SetOutline()
    {
        if(currentObj.me.transform.childCount != 0)
        {
            TMPro.TMP_Text text = null;
            currentObj.me.transform.GetChild(0).TryGetComponent<TMPro.TMP_Text>(out text);
            if(text)
            {
                text.gameObject.SetActive(false);   //구글링 결과 이렇게 감싸야지 강제로 업데이트 되면서 변경사항을 적용한다고함.
                text.outlineColor = color;
                text.outlineWidth = outlinePadding;
                text.gameObject.SetActive(true);    //강제 업데이트
            }
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
            TMPro.TMP_Text text = null;
            currentObj.me.transform.GetChild(0).TryGetComponent<TMPro.TMP_Text>(out text);
            if(text != null)
            {
                text.gameObject.SetActive(false);   //강제 업데이트
                text.outlineWidth = 0.001f;
                text.outlineWidth = 0f;
                text.gameObject.SetActive(true);    //강제 업데이트
            }
        }
        Outline outline = null;
        currentObj.me.TryGetComponent<Outline>(out outline);
        if(outline != null)
        {
            outline.effectDistance = new Vector2(0f, 0f);
        }
    }

    /// <summary>
    /// Panel View에 속하는 지 검사
    /// </summary>
    bool CheckPanelView(Vector3 worldPos)
    {
        //Vector3 canvasView = worldPos - this.transform.position;
        Vector3 panelView = WorldToPanelPoint(worldPos);

        if(panelView.x >= min.x && panelView.x <= max.x && panelView.y >= min.y && panelView.y <= max.y)
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

    public Vector3 WorldToPanelPoint(Vector3 pos) //월드 좌표를 canvas 좌표로 변경
    {
        return new Vector3(pos.x - this.transform.position.x, pos.y - this.transform.position.y, pos.z - this.transform.position.z);
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
