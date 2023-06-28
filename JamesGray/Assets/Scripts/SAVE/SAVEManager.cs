using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 현재 진행사항을 저장하기 위한 클래스

/// <summary>
/// 플레이어 세이브 관리 스크립트
/// </summary>
public class SAVEManager : MonoBehaviour
{
    [SerializeField]
    string savefilename = "SAVE01";

    [SerializeField]
    GameObject player;
    [SerializeField]
    List<GameObject> NPCs;

    public void SAVE()
    {
        NPCs = new List<GameObject>();

        GameObject[] allObj = FindObjectsOfType<GameObject>();
        for(int i = 0; i < allObj.Length; i++)
        {
            NPCManager temp;
            if(allObj[i].TryGetComponent<NPCManager>(out temp)) //NPC만 골라내기
            {
                NPCs.Add(allObj[i]);
            }
        }
        player = GameObject.FindWithTag("Player");  //플레이어 검색

        SAVES saves = new SAVES();
        saves.player = new PlayerSave(player.transform.position);
        
        saves.npcs = new NPCSave[NPCs.Count];

        for(int i = 0; i < NPCs.Count; i++)
        {
            NPCManager temp = NPCs[i].GetComponent<NPCManager>();
            saves.npcs[i] = new NPCSave(temp.ID, NPCs[i].transform.position, temp.i_Story);
        }

        string path = Application.persistentDataPath + "/saves/";
        string filePath = path + savefilename + ".json";
        string jsonData;

        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        if(!File.Exists(filePath))
        {
            File.Create(filePath);
        }

        jsonData = JsonUtility.ToJson(saves);

        File.WriteAllText(filePath, jsonData);  //저장하기 덮어쓰기
    }

    public void LOAD()
    {
        string path = Application.persistentDataPath + "/saves/";
        string filePath = path + savefilename + ".json";

        if(!File.Exists(filePath)) return;

        string jsonData = File.ReadAllText(filePath);
        SAVES saves = JsonUtility.FromJson<SAVES>(jsonData);

    }

    [System.Serializable]
    class SAVES
    {
        public PlayerSave player;
        public NPCSave[] npcs;

    }
    /// <summary>
    /// 플레이어 위치, 인벤토리 등등 저장
    /// </summary>

    [System.Serializable] 
    class PlayerSave
    {
        float x, y, z; //vector3 pos

        public PlayerSave(Vector3 pos)
        {
            x = pos.x; y = pos.y; z = pos.z;
        }
    }
    [System.Serializable]
    class NPCSave
    {
        int id;
        float x, y, z;
        int storyLine;

        public NPCSave(int id, Vector3 pos, int storyLine)
        {
            x = pos.x; y = pos.y; z = pos.z;
            this.id = id;
            this.storyLine = storyLine;
        }
    }
}
