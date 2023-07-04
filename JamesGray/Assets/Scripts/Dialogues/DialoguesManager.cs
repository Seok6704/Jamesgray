using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DataStructure;

public class DialoguesManager : MonoBehaviour
{
    public UnityEvent DialogueOn, DialogueOff;
    JSONManager dialogues;
    [Header("Scene 매니저")]
    public GameObject sceneManager;
    [Header("GameObject which has TMPro")]
    public TMP_Text tmp_NpcName;
    public TMP_Text tmp_Dialogue;
    [Header("VideoManager")]
    public UnityEngine.Video.VideoPlayer video;
    /*[Header("NPC Image")]
    public Image npcImage;*/

    [Header("Audio Sorce")] //대화 음성 출력 오브젝트
    public AudioSource audioSrc;

    public Button choiceTemplete;
    public GameObject choiceGrid;

    DEQ<DialogueNode> nextQ; Stack<DialogueNode> previousStack; //각각 이후 대화문, 이전 대화문
    DialogueNode buffer;  //버퍼, 위 큐 사이에 중간 역할 즉 현재 출력하고 있는 것을 의미
    bool isPrintDone;   //출력이 끝났는지 체크하는 변수

    VideoManager videoManager;
    int currentID, currentLineID;
    NPCManager currentNPC;

    List<string> DataA, DataB;
    sbyte btnNum; //클릭된 버튼 기억하기

    void Start()
    {
        SceneManager.SetActiveScene(gameObject.scene);  //이 스크립트가 속해있는 씬을 Active씬으로 지정

        videoManager = new VideoManager(video);
        dialogues = new JSONManager(SceneManager.GetActiveScene().name);

        previousStack = new Stack<DialogueNode>();
        nextQ = new DEQ<DialogueNode>();
        buffer = null;

        isPrintDone = false;

        currentID = -1; currentLineID = -1;
        currentNPC = null;

        DataA = new List<string>();
        DataB = new List<string>();

        btnNum = -1;
    }
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
    /// <summary>
    /// npc의 스토리라인 변수를 변경하는 함수
    /// </summary>
    void SetNPCData(int num)
    {
        if(currentNPC == null)
            return;
        
        currentNPC.i_Story = num;
    }
    /// <summary>
    /// 현재 버퍼에 있는 것을 출력 및 영상 재생
    /// </summary>
    void ShowDialogue()
    {
        if(buffer == null)   //더이상 출력할 것이 없다면 종료
        {
            EndDialogue();
            return;
        }
        isPrintDone = false;
        PlayAudio(buffer.id, buffer.lineID, buffer.index);
        //PlayVideo(buffer.id, buffer.lineID, buffer.index);
        PrintDialogue();
    }

    void PrintDialogue()
    {
        ClearDialogue();
        tmp_Dialogue.GetComponent<TextOutputManager>().Typing(CheckContent()); //현재 버퍼에 있는 것을 출력
    }
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

        previousStack.Push(buffer);
        buffer = nextQ.FrontDequeue();
    }
    /// <summary>
    /// 이전 문장을 버퍼에 넣는다
    /// </summary>
    void SetBuffer2Pre()
    {
        if(previousStack.Count == 0) return;

        nextQ.FrontEnqueue(buffer);
        buffer = previousStack.Pop();
    }

    /// <summary>
    /// 다이얼로그에 출력되어 있는 이전 데이터를 제거하는 함수
    /// </summary>
    void ClearDialogue()
    {
        DestroyChoice();
        tmp_Dialogue.GetComponent<TextOutputManager>().StopTyping();    //만약 아직 타이핑 중인데 넘기기 동작이면 멈추기
        tmp_Dialogue.GetComponent<TextOutputManager>().ClearText();
    }
    /// <summary>
    /// 초기화 함수
    /// </summary>
    void ResetVariable()
    {
        buffer = null;
        previousStack.Clear();
        nextQ.Clear();

        isPrintDone = false;

        currentID = -1; currentLineID = -1;
        currentNPC = null;

        btnNum = -1;

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

        ClearPre(); //명령문일 경우 Pre와 버퍼를 비운다. 선택지로 다이얼로그의 분기가 생기는데 뒤로 돌아가면 꼬일수도있기때문.

        temp = command[1]; //1번 인덱스의 내용은 지워질것이기 때문에 temp에 임시 저장하여 반환

        if(command[0] == "CHOICE")  //선택지 명령문일 경우
        {
            command.RemoveRange(0,2);   //명령문과 대사 제거
            SetChoice(command);
        }
        else if(command[0] == "SET")    //storyline 값 올리기라면
        {
            SetNPCData(int.Parse(command[2]));
        }
        else if(command[0] == "CLOSE")
        {
            EndDialogue();
        }

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

    void PlayVideo(int id, int lineID, int index)
    {
        if(videoManager.GetStatus())
        {
            videoManager.StopVideo();
        }
        videoManager.PlayVideo(id, lineID, index);
    }
    void LoadScene(string sceneName)
    {
        sceneManager.GetComponent<SceneController>().LoadNextScene(sceneName, 0.1f, true);
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

    public void ShowAgain()
    {
        ShowDialogue();
    }
    public void ShowPrevious()
    {
        SetBuffer2Pre();
        ShowDialogue();
    }
    public void ShowNext()
    {
        SetBuffer2Next();
        ShowDialogue();
    }

    public void SetPrintDone()
    {
        isPrintDone = true;
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
    }

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
