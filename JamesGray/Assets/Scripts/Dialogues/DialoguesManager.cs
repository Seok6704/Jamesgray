using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;

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

    string[] content;   //대화 내용이 저장될 곳
    string spritePath;
    string audioPath;

    Queue sentenceQ, emotionQ;

    bool flag;

    private void Awake() 
    {
        dialogues = new JSONManager(SceneManager.GetActiveScene().name);
        sentenceQ = new Queue();
        emotionQ = new Queue();

        flag = false;
    }


    public void SetDialogue(int id, int lineID)     //이 클래스의 진입 부분...
    {
        DialogueOn.Invoke();
        tmp_NpcName.text = dialogues.GetName(id);
        content = dialogues.GetContent(id, lineID);

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

    IEnumerator LoadTyping(int id, int lineID) 
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

                    /*if(Input.GetKeyDown(KeyCode.Space)) //스킵일때,
                    {
                        tmp_Dialogue.GetComponent<TextOutputManager>().PrintDirect(wholeSentence);
                        break;
                    } */ 
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
    } 

    void SetSentence()      //기존의 함수는 한번에 한 문장만 로드하는 문제가 있어 이 함수에서 큐로 한번에 전부 로드
    {
        
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

        emotionQ.Clear();
        sentenceQ.Clear();
        flag = false;
    }

    /*IEnumerator TestUnityWebRequest(int id, int lineID, int index)  //작동 하지 않습니다....
    {
        Debug.Log("file:///" + Application.dataPath + "/Sounds/NPC/" + id.ToString() + '/' + lineID.ToString() + '/' + index.ToString() + ".mp3");
        using(UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(Application.dataPath + "/Sounds/" + id.ToString() + '/' + lineID.ToString() + '/' + index.ToString() + ".mp3", AudioType.MPEG))
        {
            yield return www.SendWebRequest();
            if(www.result == UnityWebRequest.Result.ConnectionError) {Debug.Log(www.error); Debug.Log("NOPE");}
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioSrc.PlayOneShot(clip);
            }
        }
    }*/

    void SetImage(string emotion) //emotion을 통해 스프라이트 변경하는 함수
    {
        byte[] byteTexture = System.IO.File.ReadAllBytes(spritePath + emotion + ".png");
        Texture2D texture = new Texture2D(0,0);
        texture.LoadImage(byteTexture);

        Rect rect = new Rect(0, 0, texture.width, texture.height);
        npcImage.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }

}
