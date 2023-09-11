using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

//https://docs.unity3d.com/kr/2020.3/Manual/Video.html  //참고

public class VideoManager
{
    //readonly string path = Application.dataPath + "/Videos/NPC/";
    readonly string path = Application.streamingAssetsPath + "/Videos/";

    VideoPlayer video;

    public VideoManager(VideoPlayer v, AudioSource audiosrc)
    {
        video = v;      //영상을 재생할 플레이어 설정
        //video.SetTargetAudioSource(0, audiosrc);
    }

    string GetPath(string sceneName, int id, int lineID, int index)
    {
        return path + sceneName + '/' + id.ToString() + '/' + lineID.ToString() + '/' + index.ToString() + ".mp4";
    }

    public void PlayVideo(string sceneName, int id, int lineID, int index)
    {
        if(video.isPlaying) video.Stop();

        video.url = GetPath(sceneName, id, lineID, index);

        if(!System.IO.File.Exists(video.url)) return;  //파일 존재 여부 확인

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
