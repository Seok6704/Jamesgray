using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DataStructure;

    ////////////////////////////////////////////////////////////////////////////////////////
    //  다이얼로그 시스템
    ////////////////////////////////////////////////////////////////////////////////////////

    // JSON 파일 관련 작성법은 Wiki 참조

public class DialoguesManager : MonoBehaviour
{
    public UnityEvent DialogueOn, DialogueOff;  //다이얼로그 on/off 이벤트
    public UnityEvent DialogueNext, DialoguePrevious; //다이얼로그 대화 이동 이벤트 변수, 다이얼로그 진행에 따른 애니메이션 변화에 사용(이석현 삽입)
    JSONManager dialogues;          //현재 챕터 전체 대화문을 담고있는 객체

    public PlayerStatus playerStatus;   //플레이어 인벤토리 및 기타 관리하는 스크립트

    [Header("Scene 매니저")]
    public GameObject sceneManager;

    [Header("GameObject which has TMPro")]  //대화문을 보여줄 TMPro 및 NPC 이름 출력 TMPro
    public TMP_Text tmp_NpcName;
    public TMP_Text tmp_Dialogue;

    [Header("VideoManager")]
    public UnityEngine.Video.VideoPlayer video;

    [Header("Audio Sorce")] //대화 음성 출력 오브젝트
    public AudioSource audioSrc;

    public Button choiceTemplete;   //선택지 템플릿, Prefab으로 만들어야하나 일단 이걸로
    public GameObject choiceGrid;   //선택지가 모이게 될 그리드 그룹 레이아웃 

    public InventoryManager inv;

    DEQ<DialogueNode> nextQ; Stack<DialogueNode> previousStack; //각각 이후 대화문, 이전 대화문
    DialogueNode buffer;  //버퍼, 위 큐 사이에 중간 역할 즉 현재 출력하고 있는 것을 의미
    bool isPrintDone;   //출력이 끝났는지 체크하는 변수 Sync용 변수

    VideoManager videoManager;  //비디오를 출력하기 위한 객체
    int currentID, currentLineID;   //현재 대화 ID
    NPCManager currentNPC;  //현재 대화하고 있는 NPC

    List<string> DataA, DataB;  //선택지 데이터를 저장하는 리스트
    sbyte btnNum; //클릭된 버튼 기억하기

    GameObject nextBtn = null;

    //bool isTypeDone;    //버퍼에 있는 문장 출력 완료 여부

    [SerializeField]
    string sceneName = "";

    void Start()
    {
        SceneManager.SetActiveScene(gameObject.scene);  //이 스크립트가 속해있는 씬을 Active씬으로 지정

        videoManager = new VideoManager(video, audioSrc);

        sceneName = SceneManager.GetActiveScene().name;
        dialogues = new JSONManager(sceneName);    //대화문 로드

        previousStack = new Stack<DialogueNode>();
        nextQ = new DEQ<DialogueNode>();
        buffer = null;

        isPrintDone = true;

        currentID = -1; currentLineID = -1;
        currentNPC = null;

        DataA = new List<string>();
        DataB = new List<string>();

        btnNum = -1;

        inv = GameObject.FindAnyObjectByType<InventoryManager>();   //인벤토리 매니저 찾기

        //isTypeDone = true;

        tmp_Dialogue.GetComponent<TextOutputManager>().typeDone.AddListener(SetPrintDone);   //이벤트 등록
        tmp_Dialogue.GetComponent<TextOutputManager>().typeStart.AddListener(SetPrintNotDone);

        foreach(Button btn in GetComponentsInChildren<Button>())    //자식들 중 버튼 오브젝트 전부 호출
        {
            if(btn.gameObject.name == "Btn_Next")   //다음 버튼 찾기
            {
                nextBtn = btn.gameObject;
                break;
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    // 다이얼로그 시작 함수
    ////////////////////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// 다이얼로그가 켜질때, 초기화 함수, 큐에 데이터를 넣는다. NPC가 불러올때는 NPCManager 객체를 넣어야함
    /// </summary>
    public void SetDialogue(int id, int lineID, NPCManager npc = null)
    {
        DialogueOn.Invoke();    //다이얼로그 창 열기
        currentNPC = npc;
        SetNewDialogue(id, lineID);
    }
    /// <summary>
    /// 새 다이얼로그를 불러오는 함수, 기존의 데이터가 있든 없든 다 지워버리고 데이터를 입력한다.
    /// </summary>
    void SetNewDialogue(int id, int lineID)
    {
        string[] contents;
        if(dialogues.GetStoryLineLength(id) > lineID && lineID >= 0)   //lineID가 범위를 벗어나지 않는지 검사 만약 벗어났다면 사전에 지정된 디폴트 대사를 호출
        {
            contents = dialogues.GetContents(id, lineID);
        }
        else
        {
            contents = new string[] {dialogues.GetDefaultLine(id)};
        }

        if(contents.Length == 0) 
        {
            return;
        }

        //ResetVariable();
        previousStack.Clear();
        nextQ.Clear();

        currentID = id; currentLineID = lineID;
        

        tmp_NpcName.text = dialogues.GetName(id);

        for(int i = 0; i < contents.Length; i++)
        {
            nextQ.RearEnqueue(new DialogueNode(contents[i], id, lineID, i));
        }

        buffer = nextQ.FrontDequeue();
        ShowDialogue();
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    // 버퍼에 있는 대화문 출력 함수
    ////////////////////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// 현재 버퍼에 있는 것을 출력 및 영상 재생
    /// </summary>
    void ShowDialogue()
    {
        if(ReferenceEquals(null, buffer))   //더이상 출력할 것이 없다면 종료
        {
            EndDialogue();
            return;
        }

        //PlayAudio(buffer.id, buffer.lineID, buffer.index);
        PlayVideo(sceneName, buffer.id, buffer.lineID, buffer.index);
        PrintDialogue();
        
    }

    void PrintDialogue()
    {
        tmp_NpcName.text = dialogues.GetName(buffer.id);    //매 대사마다 NPC 이름 체크
        ClearDialogue();
        tmp_Dialogue.GetComponent<TextOutputManager>().Typing(CheckContent()); //현재 버퍼에 있는 것을 출력
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    //  선택지 관련 함수
    ////////////////////////////////////////////////////////////////////////////////////////


    /*
    명령문 구조 : "[명령종류] [출력할대사] ..."
    - 예시 -
                "[CHOICE] [선택지 출력예시, 1 번 - 미니게임 1-1 실행, 2 번 - 종료, 3 번 - 코덱스] [1.네] [MINIGAME] [1-1] [] [] , [2.아니오] [END] [] [] [] , [3.궁금한거물어보기] [CODEX] [0] [] []"

                3번째 []부터는 5개의 []가 하나의 선택지로 취급, [][][][][] 가 몇개 있냐에 따라 선택지 갯수가 달라진다.
                [출력할 대사] [명령문] [데이터] [내부데이터] [내부데이터] 형식
                MINIGAME 다음에는 해당 미니게임 씬 이름이 필요, 내부데이터는 첫번째가 성공시, 두번째가 실패시의 값을 가짐 , 불러오고 싶은 다이얼로그 storyline값이 필요
                END 다음에는 데이터 불필요 
                CODEX 다음에는 출력하고자 하는 대화문의 내용의 codexIndex가 필요 (codex는 명령문에 의해 codex 데이터를 받아올수있지만, 영상 및 음성 파일은 다른 스토리라인과 겹칠수있으므로 겹치지 않도록 1000 + codex의 값을 하는 폴더에 위치해주세요.)

                "[SET] [출력 대사] [storyline]"
                3개의 [] 로 이루어져있으며, 마지막 [] 에는 변경할 NPC의 storyline값이 들어와야한다.
                그 외 명령문은 추가 예정


                "[CLOSE] []"
                두개의 [] 로 이루어져있으며, 만나자마자 다이얼로그를 종료한다. 두번째는 빈칸이다.

                "[GET] [<아이템이름> 아이템을 얻었다!] [<Item ID>]"

                "[CHAPTER] [" "] [<이동할 챕터 씬 이름>]"
                다른 챕터로 이동하고자 할때 사용, 해당 명령문을 만나면 바로 이동합니다.

                "[OTHER] [출력할 문장] [NPC ID] [LineID] [index]"  
                다른 NPC의 대사 (영상)을 재생하고 싶을때 사용. 영상 경로를 제공해주어야 하기 때문에 위와 같이 인자값이 필요하다.

                "[HINT] [힌트를 얻었다!] [<힌트 이름>] [<힌트 내용>]"
                예시) "[HINT] ["힌트를 얻었다!"] [잃어버린 주민등록증] [제임스 그레이\n12345-6789012]"
                주의! 중복 힌트 수령을 막기위해 반드시 한번만 호출되는 구조로 만들어야함! storyLine을 무조건 변경하는것을 추천

                "[NEVEREND] [출력할 문장]"
                이 명령어를 만나면 다음 버튼이 비활성화되어 다음문장으로 가거나 종료를 할 수 없습니다.
    */

    /// <summary>
    /// 선택지 여부 검사 및 문자열과 명령문을 분리하는 함수
    /// </summary>
    string CheckContent()   //검사의 편리성을 위해 가장 처음 나오는 문자가 [ 라면 명령문이 포함된 문장으로 인식하고 그 외는 일반 문장으로 스킵하도록 함.
    {
        if(buffer.line[0] != '[') return buffer.line;   //명령문이 아니라면 전체 대사 리턴  
        int i;
        List<string> command = new List<string>();
        string temp = "";
        bool check = false;
        for(i = 0; i < buffer.line.Length; i++)
        {
            if(buffer.line[i] == ']') 
            {
                command.Add(temp);
                temp = "";
                check = false;
            }
            if(check) temp += buffer.line[i];
            if(buffer.line[i] == '[') check = true;
        }

        temp = command[1]; //1번 인덱스의 내용은 지워질것이기 때문에 temp에 임시 저장하여 반환

        if(command[0] == "CHOICE")  //선택지 명령문일 경우
        {
            command.RemoveRange(0,2);   //명령문과 대사 제거
            SetChoice(command);
        }
        else if(command[0] == "SET")    //storyline 값 올리기라면
        {
            SetNPCData(int.Parse(command[2]));
            return temp;    //SET 에 경우에는 버퍼를 비우는것이 필요하지는 않으므로 함수를 종료... 그리고 이렇게 해야 다이얼로그 출력 중 다음 버튼 눌렀을때 다이얼로그 종료가 되지 않음
        }
        else if(command[0] == "CLOSE")
        {
            EndDialogue();
            return "";
        }
        else if(command[0] == "GET")
        {
            playerStatus.AddItem(int.Parse(command[2]));
        }
        else if(command[0] == "CHAPTER")
        {
            //LoadScene(command[2], float.Parse(command[1]), false);
            LoadScene(command[2], 0f, false);   //바로 씬전환
        }
        else if(command[0] == "OTHER")  //대화문에서 다른 NPC 대사가 출력할 수 있도록 하기. 현재 버퍼를 대체하는 방식으로 동작
        {
            buffer = new DialogueNode(temp, int.Parse(command[2]), int.Parse(command[3]), int.Parse(command[4]));
            tmp_NpcName.text = dialogues.GetName(buffer.id);    //새로운 NPC로 이름 변경
            PlayVideo(sceneName, buffer.id, buffer.lineID, buffer.index);   //새로운 버퍼로 영상 재생
            return temp;
        }
        else if(command[0] == "HINT")   //인벤토리에 추가
        {
            inv.AddPage(command[2], command[3], "힌트");
        }
        else if(command[0] == "NEVEREND")
        {
            nextBtn.SetActive(false);
            return temp;    //pre나 again으로 문장이 재호출되어도 명령어를 유지하기
        }

        ClearPre(); //명령문일 경우 Pre와 버퍼를 비운다. 선택지로 다이얼로그의 분기가 생기는데 뒤로 돌아가면 꼬일수도있기때문.
        buffer = null;

        return temp;
    }

    void SetChoice(List<string> list)
    {   
        for(int i = 0; i < list.Count; i += 5)
        {
            GameObject btn = Instantiate(choiceTemplete).gameObject;
            btn.transform.SetParent(choiceGrid.transform);
            btn.transform.GetChild(0).GetComponent<TMP_Text>().text = list[i];   //선택지 내용
            btn.GetComponent<ButtonData>().SetData(list[i + 1], list[i + 2], (sbyte)i);
            DataA.Add(list[i + 3]);
            DataB.Add(list[i + 4]);
        }
    }

    void DestroyChoice()
    {
        for(int i = 0; i < choiceGrid.transform.childCount; i++)
        {
            Destroy(choiceGrid.transform.GetChild(i).gameObject);
        }
        DataA.Clear();
        DataB.Clear();
    }

    /// <summary>
    /// 기존 대화문에 codex에 있는 대화문을 추가한다.
    /// </summary>
    void SetCodex(int codex)
    {
        string[] codexContents = dialogues.GetCodexLine(currentID, codex);
        Stack<DialogueNode> temp = new Stack<DialogueNode>();

        for(int i = 0; i < codexContents.Length; i++)
        {
            DialogueNode newNode = new DialogueNode(codexContents[i], currentID, 1000 + codex, i);
            temp.Push(newNode);
        }

        while(temp.Count > 0)
        {
            nextQ.FrontEnqueue(temp.Pop());
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    //  초기화 함수
    ////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 다이얼로그에 출력되어 있는 이전 데이터를 제거하는 함수
    /// </summary>
    void ClearDialogue()
    {
        DestroyChoice();
        tmp_Dialogue.GetComponent<TextOutputManager>().StopTyping();    //만약 아직 타이핑 중인데 넘기기 동작이면 멈추기
        tmp_Dialogue.GetComponent<TextOutputManager>().ClearText();
        nextBtn.SetActive(true);    //매 문장바다 true로 고정, NEVEREND 명령어 만날때는 false로 바뀌므로...
    }
    /// <summary>
    /// 초기화 함수
    /// </summary>
    void ResetVariable()
    {
        buffer = null;
        previousStack.Clear();
        nextQ.Clear();

        currentID = -1; currentLineID = -1;
        currentNPC = null;

        btnNum = -1;

        isPrintDone = true;

        DestroyChoice();
    }
    void ClearPre()//대화문을 뒤로 옮기는것이 문제가 될수도있다면 호출하여 원인을 제거
    {
        previousStack.Clear();
    }

    void EndDialogue()
    {
        DialogueOff.Invoke();
        if(audioSrc.isPlaying) audioSrc.Stop();
        if(videoManager.GetStatus()) videoManager.StopVideo();
        ResetVariable();
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    // 영상 및 음성 출력 함수
    ////////////////////////////////////////////////////////////////////////////////////////

    void PlayAudio(int id, int lineID, int index)
    {
        if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
        string path = Application.dataPath + "/Resources/Sounds/NPC/" + id.ToString() + '/' + lineID.ToString() + '/' + index.ToString();
        if(!System.IO.File.Exists(path + ".mp3") && !System.IO.File.Exists(path + ".wav"))
        {
            return;
        }

        AudioClip clip = Resources.Load("Sounds/NPC/" + id.ToString() + '/' + lineID.ToString() + '/' + index.ToString()) as AudioClip;

        if(clip != null)
            audioSrc.PlayOneShot(clip);
        
    }

    void PlayVideo(string sceneName, int id, int lineID, int index)
    {
        if(videoManager.GetStatus())
        {
            videoManager.StopVideo();
        }
        videoManager.PlayVideo(sceneName, id, lineID, index);
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    // 버튼 상호 작용 관련 함수
    ////////////////////////////////////////////////////////////////////////////////////////

    public void ShowAgain()
    {
        ShowDialogue();
    }
    public void ShowPrevious()
    {
        SetBuffer2Pre();
        ShowDialogue();
        DialoguePrevious.Invoke();
    }
    public void ShowNext()
    {
        if(isPrintDone)  //문장 출력이 완료되었을때에만 다음 버퍼로 넘어가기 
        {
            SetBuffer2Next();
            ShowDialogue();
            DialogueNext.Invoke();
        }
        else
        {
            tmp_Dialogue.GetComponent<TextOutputManager>().ASAPrint();
        }
    }
    
    /// <summary>
    /// 다이얼로그 문장 출력이 완료되면 이 함수가 이벤트로 호출되어 Flag를 초기화한다.
    /// </summary>
    public void SetPrintDone()
    {
        isPrintDone = true;
    }

    /// <summary>
    /// 다이얼로그 문장 출력이 시작되면 호출되어 Flag를 초기화한다.
    /// </summary>
    void SetPrintNotDone()
    {
        isPrintDone = false;
    }

    /// <summary>
    /// 선택지 버튼으로 부터 데이터를 받아오는 함수
    /// </summary>
    public void GetFromButton(string command, string data, sbyte num) //명령어, 데이터, 버튼이 몇번째 버튼인지
    {
        if(command == "END")
        {
            EndDialogue();
        }
        else if(command == "MINIGAME")
        {
            btnNum = num;
            LoadScene(data);
        }
        else if(command == "CODEX") //일단 쓰지말것, 디버깅이 아직 안되어있는 상태
        {
            SetCodex(int.Parse(data));
        }
        else if(command == "CHAPTER")
        {
            LoadScene(data, 0.1f ,false);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////
    // 대화문 버퍼 이동 관련 함수, 버튼 함수들로 부터 불려진다.
    ////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 다음 문장을 버퍼에 넣는다
    /// </summary>
    void SetBuffer2Next()
    {
        if(nextQ.GetCount() == 0) 
        {
            buffer = null;
            return;
        }

        if(!ReferenceEquals(null, buffer))
            previousStack.Push(buffer);

        buffer = nextQ.FrontDequeue();
    }
    /// <summary>
    /// 이전 문장을 버퍼에 넣는다
    /// </summary>
    void SetBuffer2Pre()
    {
        if(previousStack.Count == 0) return;

        if(buffer != null)
            nextQ.FrontEnqueue(buffer);

        buffer = previousStack.Pop();
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    // 미니게임 종료시 결과를 받아오는 함수
    ////////////////////////////////////////////////////////////////////////////////////////

    public void OnMiniGameEnd(bool isSuccessful)
    {
        Debug.Log(isSuccessful);
        if(isSuccessful)
        {
            SetNewDialogue(currentID, int.Parse(DataA[btnNum]));
        }
        else
        {   
            SetNewDialogue(currentID, int.Parse(DataB[btnNum]));
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    // NPC관련 함수
    ////////////////////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// npc의 스토리라인 변수를 변경하는 함수
    /// </summary>
    void SetNPCData(int num)
    {
        if(ReferenceEquals(null, currentNPC))
            return;
        
        currentNPC.i_Story = num;
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    //  씬 관련 함수
    ////////////////////////////////////////////////////////////////////////////////////////

    void LoadScene(string sceneName, float waitTime = 0.1f, bool isAdditive = true)
    {
        sceneManager.GetComponent<SceneController>().LoadNextScene(sceneName, waitTime, isAdditive);
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    // 대화 이벤트의 한 문장마다의 데이터를 담는 노드 클래스
    ////////////////////////////////////////////////////////////////////////////////////////

    class DialogueNode
    {
        public string line;
        public int id, lineID, index;

        public DialogueNode(string line, int id, int lineID, int index)
        {
            this.line = line;
            this.id = id; this.lineID = lineID; this.index = index;
        }
    }
}
