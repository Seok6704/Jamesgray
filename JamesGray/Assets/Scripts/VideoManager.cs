using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager
{
    string path = Application.dataPath + "/Videos/NPC/";

    VideoPlayer video;

    public VideoManager(VideoPlayer v)
    {
        video = v;
    }

    string GetPath(int id, int lineID, int index)
    {
        return path + id.ToString() + '/' + lineID.ToString() + '/' + index.ToString() + ".mp4";
    }

    public void PlayVideo(int id, int lineID, int index)
    {
        video.url = GetPath(id, lineID, index);

        if(!System.IO.File.Exists(video.url)) return;

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
