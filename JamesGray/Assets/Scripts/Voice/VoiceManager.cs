using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    /////////////////////////////////////////////////*********///////////////////////////////////////////////////
    /////////////////////////////////////////////////인터페이스///////////////////////////////////////////////////
    /////////////////////////////////복잡한 STT 클래스를 접근할 수 있도록 interface////////////////////////////////
    
    public UnityEngine.Events.UnityEvent STTDone;   //데이터 수신 이벤트 발생
    static VoiceManager instance = null;   //싱글톤 디자인

    /// <summary>
    /// 음성 파일을 STT 서버로 전송하였다면, 가장 최근 변환 데이터 읽는다.
    /// </summary>
    public string Text  //변환된 텍스트
    {
        get {
            if(ReferenceEquals(result, null))   //만약 데이터가 아예 없다면 빈칸 리턴하기 
                return new string("");
            isDone = false;
            return result.results.utterances[0].msg;
        }
    }

    /// <summary>
    /// STT 서버와의 통신을 위한 인증 토큰이 만료되었는지 체크한다. True = 만료됨.
    /// </summary>
    public bool isExpired       //인증정보 만료 되었는지 체크
    {
        get {
            return auth.isExpired();
        }
    }

    /// <summary>
    /// Get 통신의 주기를 설정한다. Must be (0 <= Time < 10)
    /// </summary>
    public float Time   //get 동작의 시간 설정
    {
        get { return waitTime; }
        set {
            if(value <= 0.0f || value > 10f)
            {
                waitTime = 5f;
            }
            else
            {
                waitTime = value;
            }
        }
    }
    /// <summary>
    /// 변환된 데이터가 수신되었는지 여부, 만약 Text를 통해 데이터를 읽었다면 False로 변경된다.
    /// </summary>
    public bool isDone; //변환 완료 표시

    /// <summary>
    /// 녹음될 AudioClip의 길이를 설정. 짧든 길든 설정된 값으로 오디오를 저장한다. Default = 8 (단위: 초/sec)
    /// </summary>
    public int Length
    {
        get { return recordingLengthSec; }
        set { recordingLengthSec = value; }
    }

    /// <summary>
    /// 샘플링 레이트를 설정, 높을수록 품질이 좋고 용량이 커진다. Default = 22050 hz
    /// </summary>
    public int SampleRate
    {
        get { return recordingHz; }
        set { recordingHz = value; }
    }

    /// <summary>
    /// 녹음 반복 여부, Length만큼 녹음했을때 멈추지 않고 다시 녹음할지 여부. Default = false
    /// </summary>
    public bool Loop
    {
        get { return recordingLoop; }
        set { recordingLoop = value; }
    }
    
    /////////////////////////////////////////////////*************///////////////////////////////////////////////////
    /////////////////////////////////////////////////STT 관련 설정///////////////////////////////////////////////////
    public TMPro.TMP_Text DebugText;    //디버그용 텍스트

    public AudioSource audioSrc;        //디버그를 위해 음성을 출력할 오디오소스
    string microphoneID = null;     //녹음할때 사용할 마이크 ID
    AudioClip recording = null;     //녹음 데이터가 저장될 임시 변수
    int recordingLengthSec = 8;    //녹음 AudioClip 길이
    int recordingHz = 22050;        //Sample Rate, 오디오 품질을 결정
    bool recordingLoop = false;
    
    string url = "";                //자주사용되는 빈 URL... 미리 선언된 전역변수
    const string APIURL = "https://openapi.vito.ai/v1/"; //자주 사용되는 VITO API URL 
    string authJsonPath;    //인증 정보가 저장되는 경로. 런타임 중에 설정

    float waitTime = 5f;

    authenticateDataClass auth; //API 인증 키와 만료 시간(6시간) 데이터 저장 객체
    STTResponseClass result;    //STT 결과 값
    JsonClass config;           //STT 설정 json
    transcribeClass transcribeID;  //transcribe id;


    /////////////////////////////////////////////////********************///////////////////////////////////////////////////
    /////////////////////////////////////////////////Inspector 관련 변수들///////////////////////////////////////////////////
    ///
    ///////////////////////STT 서버로 음성파일이 전송될때, 함께 전송되어 변환 결과를 원하는 형식으로 설정/////////////////////////

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
    /////////////////////////////////////////////////********************///////////////////////////////////////////////////

    /// <summary>
    /// 싱글톤 객체를 가져온다.
    /// </summary>
    /// <returns>VoiceManager 객체를 반환한다.</returns>
    public static VoiceManager getInstance()    //싱글톤 객체 가져오기
    {
        return instance;
    }

    private void Start() 
    {
        if(ReferenceEquals(instance, null))
        {
            instance = this;
        }
        else
        {
            return; //이미 VoiceManager객체가 존재한다면 현재 객체 사용하지 않음.
        }
        isDone = false;

        //audioSrc = GetComponent<AudioSource>();

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

    private void OnDestroy() 
    {
        instance = null;    
    }

    /// <summary>
    /// 인스펙터에서 설정된 STT 설정들을 서버로 전송하기 위해 클래스로 변환한다.
    /// </summary>
    /// <returns>JsonClass 형식의 객체를 반환한다.</returns>
    JsonClass setConfig() //config 객체 초기화
    {
        JsonClass temp = new JsonClass();
        temp.config = new configClass();
        temp.config.diarization = new diarizationClass();
        temp.config.paragraph_splitter = new paragraphClass();

        temp.config.use_diarization = Enable_multi_speaker;
        
        temp.config.use_multi_channel = Enable_Multi_Channel;
        temp.config.use_itn = Enable_itn;
        temp.config.use_disfluency_filter = Enable_disfluency_filter;
        temp.config.use_profanity_filter = Enable_insult_filter;
        temp.config.paragraph_splitter.min = min;
        temp.config.paragraph_splitter.max = max;

        return temp;
    }

    /// <summary>
    /// 녹음을 시작한다.
    /// </summary>
    public void startRecording()
    {
        Debug.Log("Record Start");
        recording = Microphone.Start(microphoneID, recordingLoop, recordingLengthSec, recordingHz);
    }

    /// <summary>
    /// 녹음을 멈추고 .wav 형식의 파일로 저장한다.
    /// </summary>
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

    /// <summary>
    /// API 서버와 통신을 통해 인증 토큰을 발급받는 코루틴.
    /// </summary>
    IEnumerator InitAuthenticate()  //vito 인증하여 키값 받기
    {
        //post
        //url = "https://openapi.vito.ai/v1/authenticate";
        url = APIURL + "authenticate";
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

    /// <summary>
    /// This is Deprecated. Use SavWav instead. AudioClip을 byte[]으로 변환한다. 서버로 전송하기 위해서는 AudioClip형식이 아닌 byte[]형식이여야 한다. 정상적으로 동작하지 않으므로 사용되지 않음.
    /// </summary>
    /// <param name="audioClip">byte[]으로 변환될 AudioClip</param>
    /// <returns>byte[] 형식의 AudioClip을 반환한다.</returns>
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

    /// <summary>
    /// 사전에 저장된 .wav 음성 파일을 API 서버로 전송하는 코루틴 함수를 호출한다.
    /// PostVoice() 코루틴을 호출한다.
    /// </summary>
    public void CallPost()
    {
        //byte[] temp = System.Text.Encoding.UTF8.GetBytes("test");
        StartCoroutine(PostVoice());
    }

    IEnumerator PostVoice()      //음성 데이터 전송
    {
        isDone = false; //Post 시작시 변환은 완료되지 않은 상태로 표기

        yield return StartCoroutine(checkExpired());    //만약 인증 정보가 만료되었다면 코루틴이기때문에 기다려야 오류가 없음

        //url = "https://openapi.vito.ai/v1/transcribe";
        url = APIURL + "transcribe";

        string json = JsonUtility.ToJson(config);
        
        byte[] wavBytes = File.ReadAllBytes(Application.persistentDataPath + "/voice.wav");

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

    /// <summary>
    /// Post 통신으로 받은 TranscribeID를 사용하여 변환된 데이터를 API 서버에 요청한다. GetText() 코루틴을 호출한다. 만약 데이터가 여전히 변환 중이라면 일정 주기마다 재요청을 한다. *** See VoiceManager.Time
    /// </summary>
    public void CallGetText()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()       //transcribe ID 를 통해 결과값 받기
    {
        yield return StartCoroutine(checkExpired());    //만약 인증 정보가 만료되었다면 코루틴이기때문에 기다려야 오류가 없음

        //url = "https://openapi.vito.ai/v1/transcribe/" + transcribeID.id;
        url = APIURL + "transcribe/" + transcribeID.id;

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
            yield return new WaitForSeconds(waitTime);    //5초 정도 기다리기
            StartCoroutine(GetText());
        }
        else if(result.status == "completed")
        {
            Debug.Log(result.results.utterances[0].msg);

            if(DebugText != null)   //디버그용
            {
                DebugText.GetComponent<TextOutputManager>().ClearText();
                DebugText.GetComponent<TextOutputManager>().Typing(result.results.utterances[0].msg);
            }
            isDone = true;
            STTDone.Invoke();
        }
        else   
        {
            Debug.Log(result.status);
        }
        www.Dispose();  //통신 종료, 안하면 메모리 누수 발생
    }

    /// <summary>
    /// 녹음된 audioclip 재생
    /// </summary>
    public void PlayRecord()
    {
        if(ReferenceEquals(audioSrc, null)) return;

        audioSrc.PlayOneShot(recording);
    }
    
    /// <summary>
    /// 인증 정보를 갱신해야할지 판단하여 만료되었다면 코루틴으로 인증 통신을 진행한다.
    /// </summary>
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

    /// <summary>
    /// 인증 정보가 담기는 클래스
    /// </summary>
    [System.Serializable]
    class authenticateDataClass
    {
        public string access_token;
        public int expire_at; //expiration in unixtimestamp

        /// <summary>
        /// 인증 토큰 및 만료 시간(unix timestamp) 표시
        /// </summary>
        public void show() //debug
        {
            Debug.Log(access_token);
            Debug.Log(expire_at);
        }

        /// <summary>
        /// 인증 토큰이 만료되었는지 체크
        /// </summary>
        /// <returns>True = 만료됨</returns>
        public bool isExpired() //unix timestamp를 계산하여 인증토큰가 만료되었는지 체크
        {
            var now = System.DateTime.Now.ToLocalTime();
            var span = (now - new System.DateTime(1970,1,1,0,0,0,0).ToLocalTime());
            int timestamp = (int)span.TotalSeconds;

            //Debug.Log(timestamp);

            if(expire_at + 21600 < timestamp ) // 시간 만료 6시간 = 21600초
            {
                return true;    //만료됨
            }
            else 
            {
                return false;    
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
        public bool use_diarization;
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
        public int spk_count;
    }

    [System.Serializable]
    class paragraphClass
    {
        public int min;
        public int max;
    }
}
