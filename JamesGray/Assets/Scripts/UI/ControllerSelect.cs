using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControllerSelect : MonoBehaviour
{
    RectTransform rect; //각자의 RectTransform Transform과 별 차이 없지만 UI는 RectTransform을 쓰는것이 좋다고한다.(IOS와 안드로이드간 실행시 차이가 없다고함 Rect로 해야지만.)

    public Button defaultBtn;   //시작점 버튼 설정

    [SerializeField]
    Button currentBtn;

    static CanvasClass canvas = null;   //Canvas 및 Canvas의 RectTransform을 가지는 클래스 및 Canvas 관련 자주 쓰이는 변수 선언, 싱글톤 디자인. 어차피 캔버스는 모두 공유하니까
    static SerialCOM con = null;    //SerialCOM은 플레이어 컨트롤러 스크립트에서 Awake에서 할당한것을 사용하므로 Awake 이후에 할당해야함.
    
    static Color color = Color.yellow;
    static float outlinePadding = 2.5f;
    

    void Start() 
    {
        this.enabled = false;    //시작할때에는 비활성화 상태이다.

        if(SettingManager.onVirtualPad)   //가상 패드가 켜진상태라면 사용하지 않음.
        {
            return;
        }

        if(ReferenceEquals(null, canvas))
        {
            canvas = new CanvasClass(GameObject.Find("Canvas").GetComponent<Canvas>());
        }
        if(ReferenceEquals(null, con))
        {
            con = SerialCOM.getInstance();  //this method must call after Awake
        }
        rect = GetComponent<RectTransform>();
        //Debug.Log(defaultBtn.GetComponent<RectTransform>().position);
    }

    void OnEnable() 
    {
        EnableSelect();
    }
    
    void OnDisable() 
    {
        
    }
    void EnableSelect()
    {
        currentBtn = defaultBtn;
    }

    void Update()   //입력을 받는다.
    {
        if(rect.position != canvas.rect.position)  //판넬이 canvas 중앙에 위치하지 않으면 Disable시켜버리기
        {
            this.enabled = false;
            return;
        }

        if((!ReferenceEquals(null, con) && con.SELECT) || Input.GetKeyDown(KeyCode.E))
        {
            currentBtn.onClick.Invoke();
        }
        else if((!ReferenceEquals(null, con) && con.UP) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetHighlight(currentBtn, color, 0f);
            currentBtn = GetNearestButton(WAY.UP);
            SetHighlight(currentBtn, color, outlinePadding);
        }
        else if((!ReferenceEquals(null, con) && con.DOWN) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetHighlight(currentBtn, color, 0f);
            currentBtn = GetNearestButton(WAY.DOWN);
            SetHighlight(currentBtn, color, outlinePadding);
        }
        else if((!ReferenceEquals(null, con) && con.RIGHT) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetHighlight(currentBtn, color, 0f);
            currentBtn = GetNearestButton(WAY.RIGHT);
            SetHighlight(currentBtn, color, outlinePadding);
        }
        else if((!ReferenceEquals(null, con) && con.LEFT) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetHighlight(currentBtn, color, 0f);
            currentBtn = GetNearestButton(WAY.LEFT);
            SetHighlight(currentBtn, color, outlinePadding);
        }
    }
    
    /// <summary>
    /// 현재 위치에서 입력된 방향으로 가장 가까운 버튼을 반환한다.
    /// </summary>
    /// <param name="way">방향 값</param>
    /// <returns>입력된 방향에서 가장 가까운 버튼을 반환한다. 만약 없다면 현재 버튼 반환</returns>
    Button GetNearestButton(WAY way)
    {
        int distance = int.MaxValue;    //가장 짧은 거리의 버튼을 구해야하므로 일단 최대값으로 설정
        Button resultBtn = currentBtn;  //처음에는 현재 버튼을 가진다. 아래 연산에서 아무런 버튼을 못찾으면 현재 버튼을 반환해야하므로...
        Button[] childs = gameObject.GetComponentsInChildren<Button>(); //이 스크립트가 있는 Panel 아래 버튼 모두 가져오기
        foreach (Button child in childs)
        {
            //if(IsVisable(child)&& way == CalcDir(resultBtn, child) && child.IsActive())    //버튼이 캔버스 내부에 있으면서 입력된 방향에 있어야하며 활성화 된 상태라면 후보로 선정
            if(child != currentBtn && IsVisable(child) && way == CalcDir(currentBtn, child))    //IsActive()는 할 필요가 없다고함. GetComponent에서 비활성화된 오브젝트는 제외된다고 함.
            {
                int temp = GetDistance(currentBtn, child);
                if(distance > temp) //만약 보다 더 짧은 거리의 버튼이라면
                {
                    resultBtn = child;
                    distance = temp;
                }
            }
        }
        return resultBtn;
    }

    /// <summary>
    /// 인자로 전달받은 버튼이 캔버스 뷰 내부에 존재하며 사용자에게 표시되는지 체크하는 역할
    /// </summary>
    /// <param name="btn"></param>
    /// <returns>캔버스 내부에 있으며 사용자에게 보인다면 True</returns>
    bool IsVisable(Button btn)
    {
        Vector3 pos = WorldToCanvasPoint(btn.GetComponent<RectTransform>());
        if(pos.x >= canvas.min.x && pos.x <= canvas.max.x && pos.y >= canvas.min.y && pos.y <= canvas.max.y)
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// origin을 기준으로 candidate가 어느 방향에 있는지(4방향)을 알려주는 역할
    /// </summary>
    /// <param name="origin">기준이 되는 버튼</param>
    /// <param name="candidate">어느 방향에 있는지 알고 싶은 버튼</param>
    /// <returns>4 방향 중 하나를 반환한다.</returns>
    WAY CalcDir(Button origin, Button candidate)
    {
        Vector3 dir = (candidate.GetComponent<RectTransform>().position - origin.GetComponent<RectTransform>().position).normalized; //방향은 위와 같이 구할 수 있음.
        if(dir.x >= canvas.dir.x) return WAY.RIGHT;
        else if(dir.x <= canvas.dir.x * -1) return WAY.LEFT;
        else if(dir.y >= canvas.dir.y) return WAY.UP;
        else if(dir.y <= canvas.dir.y * -1) return WAY.DOWN;
        else return WAY.SAME;
    }

    /// <summary>
    /// A와 B사이의 거리를 계산, 정확한 거리가 필요하지 않아 sqr 연산을 생략
    /// </summary>
    /// <param name="origin">기준 지점</param>
    /// <param name="candidate">비교 지점</param>
    /// <returns>sqr연산이 생략된 거리를 반환한다.</returns>
    int GetDistance(Button origin, Button candidate)
    {
        //float h = Mathf.Abs(A.y - B.y), w = Mathf.Abs(A.x - B.x);   //세로, 가로 구하기
        //return (int)((h * h) + (w * w));
        //return (int)(origin.GetComponent<RectTransform>().position - candidate.GetComponent<RectTransform>().position).sqrMagnitude;   //위 식이랑 같은 결과
        return (int)(origin.transform.position - candidate.transform.position).sqrMagnitude;
    }

    /// <summary>
    /// 주어진 RectTransform을 캔버스 포인트로 변환
    /// </summary>
    /// <param name="rect">월드 좌표값을 가지는 RectTransform</param>
    /// <returns>월드 좌표 - 캔버스 좌표 반환</returns>
    public Vector3 WorldToCanvasPoint(RectTransform rect)
    {
        return new Vector3(rect.position.x - canvas.rect.position.x, rect.position.y - canvas.rect.position.y, rect.position.z - canvas.rect.position.z);
    }

    /// <summary>
    /// 버튼의 강조표시를 한다.
    /// </summary>
    /// <param name="color">색</param>
    /// <param name="outlinePadding">색상 두께</param>
    void SetHighlight(Button btn, Color color, float outlinePadding)
    {
        TMP_Text text = btn.gameObject.GetComponentInChildren<TMP_Text>();
        if(!ReferenceEquals(null, text))
        {
            text.gameObject.SetActive(false);
            text.outlineColor = color;
            text.outlineWidth = outlinePadding;
            text.gameObject.SetActive(true);
        }
        
        Outline outline = btn.gameObject.GetComponentInChildren<Outline>();
        if(!ReferenceEquals(null, outline))
        {
            outline.effectColor = color;
            outline.effectDistance = new Vector2(outlinePadding * 10, outlinePadding * 10);
        }
    }

    void OnDestroy() 
    {
        if(!ReferenceEquals(null, canvas))
        {
            canvas = null;
        }
        if(!ReferenceEquals(null, con))
        {
            con = null;
        }
    }

    /// <summary>
    /// Canvas 및 Canvas의 RectTransform을 가지는 클래스 및 Canvas 관련 자주 쓰이는 변수 선언
    /// </summary>
    class CanvasClass
    {
        public Canvas canvas;
        public RectTransform rect;
        public Vector2 min, max, dir;  //min, max 는 캔버스 크기를 표현, dir는 버튼의 방향을 알아내기 위해 기준으로 사용할 값

        public CanvasClass(Canvas canvas)
        {
            this.canvas = canvas;
            rect = canvas.GetComponent<RectTransform>();

            max.x = rect.rect.width/2;
            max.y = rect.rect.height/2;
            min.x = max.x * -1;
            min.y = max.y * -1;

            dir = new Vector2(1,1).normalized;  // 길이가 1인 1,1로 향하는 방향 벡터
        }
    }

    public enum WAY
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
        SAME
    }
}
