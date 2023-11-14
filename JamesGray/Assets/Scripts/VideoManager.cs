using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

//https://docs.unity3d.com/kr/2020.3/Manual/Video.html  //참고

public class VideoManager : MonoBehaviour
{
    //readonly string path = Application.dataPath + "/Videos/NPC/";
    readonly string path = Application.streamingAssetsPath + "/Videos/";

    VideoPlayer video;

    public VideoManager(VideoPlayer v, AudioSource audiosrc = null)
    {
        video = v;      //영상을 재생할 플레이어 설정
        //video.SetTargetAudioSource(0, audiosrc);
    }

    string GetPath(string sceneName, int id, int lineID, int index)
    {
        return path + sceneName + '/' + id.ToString() + '/' + lineID.ToString() + '/' + index.ToString() + ".mp4";
    }
    string GetAndroidPath(string sceneName, int id, int lineID, int index)
    {
        return "jar:file://" + Application.dataPath + "!/assets/Videos/" + sceneName + '/' + id.ToString() + '/' + lineID.ToString() + '/' + index.ToString() + ".mp4";
    }

    public void PlayVideo(string sceneName, int id, int lineID, int index)
    {
        if(video.isPlaying) video.Stop();
        string url = GetPath(sceneName, id, lineID, index);
        //string output = Application.persistentDataPath + "/dial123789490123890124590.mp4";

        if (Application.platform == RuntimePlatform.Android)
        {
            //video.url = GetAndroidPath(sceneName, id, lineID, index);
            //UnityWebRequest w = UnityWebRequest.Get(url);
            //Debug.Log(output);
            url = GetAndroidPath(sceneName, id, lineID, index);
            //StartCoroutine(LoadVideo(url, output));
            Debug.Log(url);
            //return;
        }
        else
        {
            if(!System.IO.File.Exists(url)) 
            {
                Debug.Log("NO FILE : " + url);
                return;  //파일 존재 여부 확인
            }
        }
        video.url = url;

        //if(!System.IO.File.Exists(video.url)) 
        //{
         //   Debug.Log("NO FILE");
          //  return;  //파일 존재 여부 확인
        //}



        Debug.Log("Video is being Ready.");
        //StartCoroutine(PrepareVideo());

        video.Play();
    }

    IEnumerator LoadVideo(string url, string output)
    {
        UnityWebRequest req = UnityWebRequest.Get(url);
        req.downloadHandler = new DownloadHandlerFile(output);
        yield return req.SendWebRequest();
            
        if(req.isNetworkError || req.isHttpError) Debug.Log(req.error);
            
        yield return new WaitForSeconds(0.1f);
        
        video.url = output;
        if(!System.IO.File.Exists(video.url)) 
        {
            Debug.Log("NO FILE");
            yield break;  //파일 존재 여부 확인
        }
        video.Play();
    }

    public void PauseVideo()
    {
        video.Pause();
    }
    public void StopVideo()
    {
        video.Stop();
    }
    public bool GetStatus()
    {
        return video.isPlaying;
    }
}
