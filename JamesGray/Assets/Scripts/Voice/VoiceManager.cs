using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent stt_Done;
    public TMPro.TMP_Text DebugText;    //디버그용 텍스트

    AudioSource audioSrc;           //디버그를 위해 음성을 출력할 오디오소스
    string microphoneID = null;     
    AudioClip recording = null;     //녹음 데이터가 저장될 임시 변수
    int recordingLengthSec = 15;    
    int recordingHz = 22050;
    
    string url = "";
    string authJsonPath;

    authenticateDataClass auth; //API 인증 키와 만료 시간(6시간) 데이터 저장 객체
    STTResponseClass result;    //STT 결과 값
    JsonClass config;           //STT 설정 json
    transcribeClass transcribeID;  //transcribe id;

    [Header("STT 설정")]    //vito 개발자 가이드 참조,

    [Tooltip("다중 화자")]
    public bool Enable_multi_speaker;
    public string user_id;
    public string partner_id;
    [Tooltip("다중채널 - 채널로 화자 구분")]
    public bool Enable_Multi_Channel;
    [Tooltip("영어, 숫자, 단위 변환")]
    public bool Enable_itn;
    [Tooltip("간투어 필터 - \"어\" , \"음\" 과 같은 음절을 제거하여 명료하게 변환")]
    public bool Enable_disfluency_filter;
    [Tooltip("비속어 필터")]
    public bool Enable_insult_filter;
    [Tooltip("문단 나누기")]
    public int min;
    public int max;

    private void Start() 
    {
        audioSrc = GetComponent<AudioSource>();

        microphoneID = Microphone.devices[0];
        config = setConfig();
        authJsonPath = Application.persistentDataPath + "/Authenticate.json";

        if(File.Exists(authJsonPath))
        {
            auth = JsonUtility.FromJson<authenticateDataClass>(File.ReadAllText(authJsonPath));
            if(auth.isExpired()) 
            {
                StartCoroutine(InitAuthenticate()); //인증정보가 만료되었다면 재인증
            }
        }
        else         //파일이 존재하지 않았다면 실행
        {
            StartCoroutine(InitAuthenticate());
        }
    }
    JsonClass setConfig() //config 객체 초기화
    {
        JsonClass temp = new JsonClass();
        temp.config = new configClass();
        temp.config.diarization = new diarizationClass();
        temp.config.paragraph_splitter = new paragraphClass();

        temp.config.diarization.use_verification = Enable_multi_speaker;
        temp.config.use_multi_channel = Enable_Multi_Channel;
        temp.config.use_itn = Enable_itn;
        temp.config.use_disfluency_filter = Enable_disfluency_filter;
        temp.config.use_profanity_filter = Enable_insult_filter;
        temp.config.paragraph_splitter.min = min;
        temp.config.paragraph_splitter.max = max;

        return temp;
    }

    public void startRecording()
    {
        Debug.Log("Record Start");
        recording = Microphone.Start(microphoneID, false, recordingLengthSec, recordingHz);
    }

    public void stopRecording()
    {
        if(Microphone.IsRecording(microphoneID))
        {
            Microphone.End(microphoneID);
            Debug.Log("Stop Recording");

            if(recording == null)
            {
                Debug.LogError("Nothing recorded");
                return;
            }
            SavWav.Save(Application.persistentDataPath+"/voice.wav", recording);
            //byte[] byteData = getByteFromAudioClip(recording);
            //StartCoroutine(PostVoice(byteData));
        }
    }

    IEnumerator InitAuthenticate()  //vito 인증하여 키값 받기
    {
        //post
        url = "https://openapi.vito.ai/v1/authenticate";
        string clientID = "WzZQNfCUtwj-E6AA4f9E";
        string clientSecret = "8Qdb0APc8KihbvHVw32ngEVKqhMrM5GmNU8x-5sC";
        WWWForm form = new WWWForm();
        form.AddField("client_id", clientID);
        form.AddField("client_secret", clientSecret);
        
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();

        if(www.error == null)
        {
            auth = JsonUtility.FromJson<authenticateDataClass>(www.downloadHandler.text);
            Debug.Log(auth.isExpired());
            File.WriteAllText(authJsonPath, www.downloadHandler.text);
        }
        else
        {
            FailClass fail = JsonUtility.FromJson<FailClass>(www.downloadHandler.text);
            fail.showError();
        }
        Debug.Log("Athu    -    " + www.downloadHandler.text);
        www.Dispose();  //통신 종료, 안하면 메모리 누수 발생
    }

    byte[] getByteFromAudioClip(AudioClip audioClip)
    {
        float[] floatArrayAudioClip = new float[audioClip.samples]; 
        audioClip.GetData(floatArrayAudioClip, 0);

        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);

        foreach(float sample in floatArrayAudioClip)
        {
            writer.Write(sample);
        }
        byte[] bytes = stream.ToArray();
        return bytes;
    }

    public void CallPost_De()
    {
        byte[] temp = System.Text.Encoding.UTF8.GetBytes("test");
        StartCoroutine(PostVoice(temp));
    }

    IEnumerator PostVoice(byte[] data)      //음성 데이터 전송
    {
        yield return StartCoroutine(checkExpired());    //만약 인증 정보가 만료되었다면 코루틴이기때문에 기다려야 오류가 없음

        url = "https://openapi.vito.ai/v1/transcribe";

        string json = JsonUtility.ToJson(config.config);
        byte[] wavBytes = File.ReadAllBytes(Application.persistentDataPath + "/test.wav");

        //json = "--asdandkawdnakjsdnakwda\r\n" + "Content-Disposition: form-data; name=\"config\"\r\n" + "Content-Type: application/json\r\n" + json + "--asdandkawdnakjsdnakwda";
        //json = "\"congif\":{" + json + "}";
        byte[] jsonbytes = System.Text.Encoding.UTF8.GetBytes(json);

        string multipartBoundary = "asdandkawdnakjsdnakwda";  //중복을 막기 위한 아무 문자열
        byte[] boundaryBytes = System.Text.Encoding.UTF8.GetBytes(multipartBoundary);

        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormFileSection("file", wavBytes, "test.wav", "audio/wav"));
        form.Add(new MultipartFormDataSection("config", json, "application/json"));

        Debug.Log(form[0].contentType + "\n" + form[0].fileName + "\n" + form[0].sectionName + "\n" + form[0].sectionData + "\n");
        
        UnityWebRequest www = UnityWebRequest.Post(url, form, boundaryBytes);

        www.SetRequestHeader("Authorization", "Bearer " + auth.access_token);
        www.SetRequestHeader("Content-Type", "multipart/form-data; boundary=" + multipartBoundary);

        yield return www.SendWebRequest();

        if(www.error == null)
        {
            transcribeID = JsonUtility.FromJson<transcribeClass>(www.downloadHandler.text);
            Debug.Log("TranscribeID : " + transcribeID.id);
        }
        else
        {
            FailClass fail = JsonUtility.FromJson<FailClass>(www.downloadHandler.text);
            fail.showError();
        }  
        Debug.Log("Post   -   " + www.downloadHandler.text);
        www.Dispose();  //통신 종료, 안하면 메모리 누수 발생
    }

    public void Debug_GetText()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()       //transcribe ID 를 통해 결과값 받기
    {
        yield return StartCoroutine(checkExpired());    //만약 인증 정보가 만료되었다면 코루틴이기때문에 기다려야 오류가 없음

        url = "https://openapi.vito.ai/v1/transcribe/" + transcribeID.id;

        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Authorization", "Bearer " + auth.access_token);
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if(www.error == null)
        {
            result = JsonUtility.FromJson<STTResponseClass>(www.downloadHandler.text);
            Debug.Log(result);
        }
        else
        {
            FailClass fail = JsonUtility.FromJson<FailClass>(www.downloadHandler.text);
            fail.showError();
        }
        Debug.Log("GET  -  " + www.downloadHandler.text);

        if(result.status == "transcribing")     //변환 중
        {
            yield return new WaitForSeconds(5f);    //5초 정도 기다리기
            StartCoroutine(GetText());
        }
        else if(result.status == "completed")
        {
            Debug.Log(result.results.utterances[0].msg);

            if(DebugText != null)   //디버그용
            {
                DebugText.GetComponent<TextOutputManager>().ClearText();
                DebugText.GetComponent<TextOutputManager>().Typing(result.results.utterances[0].msg);

                stt_Done.Invoke();
            }
        }
        else   
        {
            Debug.Log(result.status);
        }
        www.Dispose();  //통신 종료, 안하면 메모리 누수 발생
    }

    public void PlayRecord()
    {
        audioSrc.PlayOneShot(recording);
    }
    
    IEnumerator checkExpired()
    {
        if(auth.isExpired())
        {
            yield return StartCoroutine(InitAuthenticate());
        }
    }

    /*
        이 아래 클래스들은 restAPI의 JSON 형식의 데이터들을 받기 위해 작성된 클래스입니다.
    */
    [System.Serializable]
    class authenticateDataClass
    {
        public string access_token;
        public int expire_at; //expiration in unixtimestamp

        public void show() //debug
        {
            Debug.Log(access_token);
            Debug.Log(expire_at);
        }

        public bool isExpired() //unix timestamp를 계산하여 인증토큰가 만료되었는지 체크
        {
            var now = System.DateTime.Now.ToLocalTime();
            var span = (now - new System.DateTime(1970,1,1,0,0,0,0).ToLocalTime());
            int timestamp = (int)span.TotalSeconds;

            //Debug.Log(timestamp);

            if(expire_at + 21600 < timestamp ) // 시간 만료 6시간 = 21600초
            {
                return true;
            }
            else 
            {
                return false;    //만료됨
            }
        }
    }
    [System.Serializable]
    class transcribeClass
    {
        public string id;   //transcribeID
    }
    [System.Serializable]
    class FailClass     //에러 코드 관리 클래스 json
    {
        public string code;

        public void showError() 
        {
            string error = code + " : ";
            if(code == "H0001")         error += "BadRequest";
            else if(code == "H0010")    error += "Not Support";
            else if(code == "H0002")    error += "Unauthorized or Unvalid Token";
            else if(code == "H0003")    error += "Unauthorized";
            else if(code == "H0004")    error += "NO result";
            else if(code == "H0005")    error += "Big File Size";
            else if(code == "H0006")    error += "File Length excess";
            else if(code == "H0007")    error += "result Exipired";
            else if(code == "A0001")    error += "Total Usage excess";
            else if(code == "E500")     error += "ServerError";
            else                        error += "Unknown Error";

            Debug.Log(error);
        }
    }

    [System.Serializable]
    class STTResponseClass
    {
        public string id;
        public string status;
        public resultClass results;

    }
    [System.Serializable]
    class resultClass
    {
        public utteranceClass[] utterances;
        public bool verified;
    }
    [System.Serializable]
    class utteranceClass
    {
        public int start_at;
        public int duration;
        public string msg;
        public int spk;
        public string spk_type;
    }
    [System.Serializable]
    class JsonClass
    {
        public configClass config;
    }
    [System.Serializable]
    class configClass
    {
        public diarizationClass diarization;
        public bool use_multi_channel;
        public bool use_itn;
        public bool use_disfluency_filter;
        public bool use_profanity_filter;
        public paragraphClass paragraph_splitter;
    }
    [System.Serializable]
    class diarizationClass
    {
        public bool use_verification;
    }

    [System.Serializable]
    class paragraphClass
    {
        public int min;
        public int max;
    }
}
