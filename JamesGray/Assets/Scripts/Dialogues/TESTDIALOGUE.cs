using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;
using DataStructure;

public class DialoguesManager_v2 : MonoBehaviour
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

    [Header("선택지 버튼")]
    public GameObject[] buttons;    //다이얼로그 창에서 사용될 선택지 버튼
    [Header("옵션 버튼")]
    public GameObject[] options;    //다이얼로그 창에서 다시듣기, 이전 등 버튼

    public Button choiceTemplete;
    public GameObject choiceGrid;

    
    //string spritePath;
    string audioPath;
    string nextSceneName;
    //int index; // 다이얼로그 인덱스

    bool flag, again, next, previous, isChoice, recursion;
    int recursionIndex;

    VideoManager videoManager;

    private void Start() 
    {
        SceneManager.SetActiveScene(gameObject.scene);  //이 스크립트가 속해있는 씬을 Active씬으로 지정
        
        videoManager = new VideoManager(video); //비디오 플레이 설정

        dialogues = new JSONManager(SceneManager.GetActiveScene().name);
        flag = false;
        again = false;
        next = false;
        previous = false;
        isChoice = false;
        recursion = false;

        recursionIndex = -1;
    }


    public void SetDialogue(int id, int lineID)     //이 클래스의 진입 부분...
    {
        DialogueOn.Invoke();
        tmp_NpcName.text = dialogues.GetName(id);
        //content = dialogues.GetContent(id, lineID);

        //spritePath = dialogues.GetSpritePath();
        //spritePath += id.ToString() + '/';

        audioPath = dialogues.GetAudioPath();
        audioPath += id.ToString() + '/' + lineID.ToString() + '/';

        nextSceneName = "";

        StartCoroutine(LoadTyping(id, lineID));
    }


    IEnumerator WaitKey()                       // 이 클래스의 마지막 부분...
    {     //마지막 문장을 사라지지 않게 대기하게 해주는 코루틴 함수
        while(true) 
        {
            if(Input.GetKeyDown(KeyCode.Space) || next) break;
            yield return null;
        }
        //gameObject.SetActive(false);
        //LoadScene(i_dialogNum);
        DialogueOff.Invoke();
        ResetVar();
    }

    /*
        반복문을 통해 대화문의 처음부터 끝까지 출력하도록 하는 함수.
    */
    IEnumerator LoadTyping(int id, int lineID) 
    {
        int index = 0;
        while(index < dialogues.GetContentLength(id, lineID))
        {
            if(previous)
            {
                index = index >= 1 ? index - 1 : 0; //혹시라도 음수로 가는 것을 방지
            }
            if(recursion)
            {
                yield return StartCoroutine(LoadTyping(id, recursionIndex));    //재귀호출
            }
            string singleLine = dialogues.GetContent(id, lineID, index);

            tmp_Dialogue.GetComponent<TextOutputManager>().StopTyping();    //만약 아직 타이핑 중인데 넘기기 동작이면 멈추기
            tmp_Dialogue.GetComponent<TextOutputManager>().ClearText();

            
            /*if(singleLine[0] == '[')    //문장의 시작이 "[" 일 경우, 씬을 로드한다. []안에는 다음 씬 이름이 들어와야한다.
            {
                PlayAudio(id, lineID, index);
                //PlayVideo(id, lineID, index);
                SetChoice(singleLine);
                isChoice = true;
                break;
            }*/
            
            flag = false;
            again = false;
            previous = false;
            next = false;
            isChoice = false;
            recursion = false;

            /*
                출력 시작 부 - 1
            */
            PlayAudio(id, lineID, index);   //음성 출력
            //PlayVideo(id, lineID, index);
            tmp_Dialogue.GetComponent<TextOutputManager>().Typing(singleLine);

            //if(index == dialogues.GetContentLength(id, lineID) - 1 && dialogues.GetChoiceLength(id, lineID) != 0) //마지막 문장일때, 선택지 여부 검사
            //{
            //   isChoice = true;
                //SetChoice(id, lineID);
            //}

            yield return new WaitForSeconds(0.3f); // 이전에 이미 눌렀던 스페이스바가 그대로 유지되는 것을 막기위해 대기
            /*
                대기 부 - 2
            */
            while(!again)     //터치시까지 대기 또는 다시 재생 변수가 활성화 될때까지 대기
            {
                if((Input.GetKeyDown(KeyCode.Space) || next) && !isChoice)
                {
                    index++;
                    break;
                }
                yield return null;
            }
            DestroyChoice();
        }
        //if(!isChoice)
        StartCoroutine(WaitKey());    
    }   

    public void SetFlag()       //이벤트로 불러와질 함수    flag는 다이얼로그 텍스트의 문자 출력을 동기화하기 위해 선언되었습니다.  
    {
        flag = true;
    }

    void PlayAudio(int id, int lineID, int index)
    {
        if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
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

    IEnumerator GetAudioClip(int id, int lineID, int index)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + Application.dataPath + "/Sounds/NPC/" + id.ToString() + '/' + lineID.ToString() + '/' + index.ToString() + ".wav", AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }

    void ResetVar()     //초기화 함수. 혹시나 남아있을 큐를 비우고, flag를 원위치하도록 합니다.
    {   
        int i;
        if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
        if(videoManager.GetStatus() && false) //임시로 비활성화
        {
            videoManager.StopVideo();
        }
        flag = false;
        next = false;
        previous = false;
        isChoice = false;
        recursion = false;
        recursionIndex = -1;

        for(i = 0; i < options.Length; i++)    //옵션 버튼 활성화
        {
            options[i].SetActive(true);    
        }

        for(i = 0; i < buttons.Length; i++)     //선택지 비활성화
        {
            buttons[i].SetActive(false);
        }
    }

   /*void SetImage(string emotion) //emotion을 통해 스프라이트 변경하는 함수
    {
        byte[] byteTexture = System.IO.File.ReadAllBytes(spritePath + emotion + ".png");
        Texture2D texture = new Texture2D(0,0);
        texture.LoadImage(byteTexture);

        Rect rect = new Rect(0, 0, texture.width, texture.height);
        npcImage.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }*/

    void SetChoice(string singleLine)   //일단은 예/아니오 만 출력하게... 나중에 선택지 내용도 바꿀수있도록 수정 예정
    {
        int i = 1; //반복문에 사용될 인덱스
        char c = singleLine[i];

        while(c != ']') //[] 안에 들어있는 문장 추출
        {
            nextSceneName += c;
            c = singleLine[++i];
        }
        singleLine = singleLine.Substring(i + 1);   //씬 이름 제거

        tmp_Dialogue.GetComponent<TextOutputManager>().Typing(singleLine);
        flag = false;

        for(i = 0; i < options.Length; i++) //일단은 오류를 막기위해 옵션 버튼 비활성화
        {
            options[i].SetActive(false);    
        }

        for(i = 0; i < buttons.Length; i++) //선택지 활성화
        {
            buttons[i].SetActive(true);
        }
    }

    /*void SetChoice(int id, int lineID)
    {
        for(int i = 0; i < dialogues.GetChoiceLength(id, lineID); i++)
                {
                    string choiceLine = dialogues.GetChoice(id, lineID, i);
                    List<string> opt = new List<string>();
                    string temp = "";
                    bool check = false;
                    for(int j = 0; j < choiceLine.Length; j++)  //문자열에서 []로 감싸져있는 값만 추출하기, [선택지내용] [모드] [파라미터] 형식으로 입력되어야함
                    {
                        if(choiceLine[j] == ']') 
                        {
                            opt.Add(temp);
                            temp = "";
                            check = false;
                        }
                        if(check) temp += choiceLine[j];
                        if(choiceLine[j] == '[') check = true;
                    }

                    GameObject btn = Instantiate(choiceTemplete).gameObject;
                    btn.transform.SetParent(choiceGrid.transform);
                    btn.transform.GetChild(0).GetComponent<TMP_Text>().text = opt[0];   //선택지 내용
                    btn.GetComponent<Button>().onClick.AddListener(() => {GameObject.FindWithTag("Dialogue").GetComponent<DialoguesManager>().GetFromButton(opt[1], opt[2]);});
        }
    }*/
    void DestroyChoice()
    {
        for(int i = 0; i < choiceGrid.transform.childCount; i++)
        {
            Destroy(choiceGrid.transform.GetChild(i).gameObject);
        }
    }
    
    /*버튼 관련 함수*/////////////////////////////////////
    public void ShowPrevious()  // 이벤트로 불러와지며 이전 대화를 보여준다.
    {
        previous = true;
        ShowAgain();        //이전것을 다시 보여주기
    }
    public void ShowAgain() //다시 보여주기
    {
        again = true;
    }
    public void ShowNext()  //다음 대화문 보여주기
    {
        next = true;
    }
    ///////////////////선택지//////////////////////////////
    public void Positive()  //버튼이 긍정적일때
    {
        sceneManager.GetComponent<SceneController>().LoadNextScene(nextSceneName, 0.1f, true);
    }
    public void Negative()  //버튼이 부정적일때 대화 종료
    {
        DialogueOff.Invoke();
        ResetVar();
    }

    public void GetFromButton(string mode, string data)
    {
        if(mode == "MINIGAME")
        {
            sceneManager.GetComponent<SceneController>().LoadNextScene(data, 0.1f, true);
        }
        else if(mode == "END")
        {
            IncreaseStoryLine();
        }
        else if(mode == "DONOTHING")
        {
            return;
        }
        else if(mode == "CODEX")
        {
            again = true;
            recursionIndex = int.Parse(data);
        }
        isChoice = false;
    }
    void IncreaseStoryLine()
    {

    }
}
