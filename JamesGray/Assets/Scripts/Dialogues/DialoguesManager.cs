using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DialoguesManager : MonoBehaviour
{
    public UnityEvent DialogueOn, DialogueOff;
    JSONManager dialogues;

    [Header("GameObject which has TMPro")]
    public TMP_Text tmp_NpcName;
    public TMP_Text tmp_Dialogue;

    [Header("NPC Image")]
    public Image npcImage;

    [Header("Audio Sorce")] //대화 음성 출력 오브젝트
    public AudioSource audioSrc;
    string spritePath;
    string audioPath;
    int index; // 다이얼로그 인덱스

    bool flag, again;

    private void Awake() 
    {
        dialogues = new JSONManager(SceneManager.GetActiveScene().name);

        flag = false;
        again = false;
    }


    public void SetDialogue(int id, int lineID)     //이 클래스의 진입 부분...
    {
        DialogueOn.Invoke();
        tmp_NpcName.text = dialogues.GetName(id);
        //content = dialogues.GetContent(id, lineID);

        spritePath = dialogues.GetSpritePath();
        spritePath += id.ToString() + '/';

        audioPath = dialogues.GetAudioPath();
        audioPath += id.ToString() + '/' + lineID.ToString() + '/';

        StartCoroutine(LoadTyping(id, lineID));
    }


    IEnumerator WaitKey()                       // 이 클래스의 마지막 부분...
    {     //마지막 문장을 사라지지 않게 대기하게 해주는 코루틴 함수
        while(true) 
        {
            if(Input.GetKeyDown(KeyCode.Space)) break;
            yield return null;
        }
        //gameObject.SetActive(false);
        //LoadScene(i_dialogNum);
        DialogueOff.Invoke();
        ResetVar();
    }

    IEnumerator LoadTyping(int id, int lineID)      //기존의 함수는 한번에 한 문장만 로드하는 문제가 있어 이 함수에서 큐로 한번에 전부 로드
    {
        index = 0;
        while(index < dialogues.GetContentLength(id, lineID))
        {
            tmp_Dialogue.GetComponent<TextOutputManager>().StopTyping();    //만약 아직 타이핑 중인데 넘기기 동작이면 멈추기
            tmp_Dialogue.GetComponent<TextOutputManager>().ClearText();

            /*
                출력 시작 부 - 1
            */
            PlayAudio(id, lineID, index);   //음성 출력
            tmp_Dialogue.GetComponent<TextOutputManager>().Typing(dialogues.GetContent(id, lineID, index));
            flag = false;
            again = false;

            yield return new WaitForSeconds(0.3f); // 이전에 이미 눌렀던 스페이스바가 그대로 유지되는 것을 막기위해 대기
            /*
                대기 부 - 2
            */
            //yield return new WaitUntil( () => flag);        //참이 될때까지 대기
            while(!again)     //터치시까지 대기 또는 다시 재생 변수가 활성화 될때까지 대기
            {
                if(Input.GetKeyDown(KeyCode.Space) )
                {
                    index++;
                    break;
                }
                yield return null;
            }
        }
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
        audioSrc.PlayOneShot(clip);
    }

    void ResetVar()     //초기화 함수. 혹시나 남아있을 큐를 비우고, flag를 원위치하도록 합니다.
    {   
        if(audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
        flag = false;
    }

    void SetImage(string emotion) //emotion을 통해 스프라이트 변경하는 함수
    {
        byte[] byteTexture = System.IO.File.ReadAllBytes(spritePath + emotion + ".png");
        Texture2D texture = new Texture2D(0,0);
        texture.LoadImage(byteTexture);

        Rect rect = new Rect(0, 0, texture.width, texture.height);
        npcImage.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }
    
    public void ShowPrevious()  // 이벤트로 불러와지면 
    {
        index = index >= 1 ? index - 1 : 0; //혹시라도 음수로 가는 것을 방지
        ShowAgain();        //이전것을 다시 보여주기
    }
    public void ShowAgain() //다시 보여주기
    {
        again = true;
    }

}

/*      지우기 아까워서 저장 삭제 해도 무방
    IEnumerator LoadTyping_de(int id, int lineID) 
    {   //한글자씩 출력하게 해주는 함수
        int index = 0;
        string wholeSentence = "";
        string sentence = "";
        string emotion = "";
        char c;

        while(index < content.Length)
        {
            if(Input.GetKeyDown(KeyCode.Space) || index == 0) 
            {
                tmp_Dialogue.GetComponent<TextOutputManager>().StopTyping();    //만약 아직 타이핑 중인데 넘기기 동작이면 멈추기
                tmp_Dialogue.GetComponent<TextOutputManager>().ClearText();
                content[index] += " ";                                          // 공백 추가
                wholeSentence = "";

                if(content[index][0] != '[') emotionQ.Enqueue("DEFAULT");

                PlayAudio(id, lineID, index);   //음성 출력
                //StartCoroutine(TestUnityWebRequest(id, lineID, index));
                
                for(int i = 0; i < content[index].Length;)
                {
                    c = content[index][i];
                    if(c == '[') 
                    {
                        if(sentence != "")
                        {
                            sentenceQ.Enqueue(sentence);
                            wholeSentence += sentence;
                            sentence = "";
                        }
                        while(true)
                        {
                            c = content[index][++i];
                            if(c == ']') 
                            {
                                emotionQ.Enqueue(emotion);
                                emotion = "";
                                ++i;
                                break;
                            }
                            emotion += c;
                        }
                    }
                    else
                    {
                        sentence += c;
                        ++i;
                    }
                }
                if(sentence != "") 
                {   
                    sentenceQ.Enqueue(sentence);
                    wholeSentence += sentence;
                    sentence = "";
                }
                if(emotion != "") 
                {
                    emotionQ.Enqueue(emotion);
                    emotion = "";
                }
                //if(emotionQ.Count == 0) emotionQ.Enqueue("DEFAULT");    //만약 지정된 감정이 없다면 기본 스프라이트로 설정
                
                flag = true;

                while(true)
                {
                    if(sentenceQ.Count == 0) //출력할 문장이 더이상 없다면 탈출
                    {
                        break;
                    }

                    if(flag)
                    {
                        flag = false;
                        tmp_Dialogue.GetComponent<TextOutputManager>().Typing((string)sentenceQ.Dequeue());
                        if(emotionQ.Count != 0) SetImage((string)emotionQ.Dequeue());
                    }

                    yield return new WaitForFixedUpdate();   //좋은지는 모르겠음   
                }
                emotionQ.Clear();
                sentenceQ.Clear();
                flag = false;

                ++index;
                yield return new WaitForSeconds(0.3f); 
            }
            yield return null;
        }
        StartCoroutine(WaitKey());        
    } */
