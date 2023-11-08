using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

// 현재 진행사항을 저장하기 위한 클래스
// 일단 동적으로 씬에 배치되는 NPC는 없다고 가정하고 ID값으로 정렬하여 검사 없이 1대 1로 데이터를 넣도록 개발 예정, 추후 수정될 수 있음.
/// <summary>
/// 플레이어 세이브 관리 스크립트
/// </summary>
public class SAVEManager : MonoBehaviour
{
    public SceneController SceneController;  // 씬 이동을 위해 씬 컨트롤러를 가진 오브젝트 받기
    public UnityEvent NoFile;
    [SerializeField]
    string savefilename = "SAVE01";
    //string saveVer = "0.0"; //세이브 데이터의 버전

    [SerializeField]
    GameObject player;
    [SerializeField]
    static bool isLoad = false;
    //List<GameObject> NPCs;
    static SAVES saves = null;

    /// <summary>
    /// 세이브 파일에 저장된 챕터명 프로퍼티
    /// </summary>
    public static string saveSceneName 
    {
        get {
                if(ReferenceEquals(null, saves))    //세이브 파일이 없을경우
                    return "";
                return saves.chapter;
            }
    }

    private void Start() 
    {
        if(isLoad)
        {
            SetNPCs();
            isLoad = false;
        }    
    }

    public void SAVE()
    {
        List<GameObject> NPCs = FindByComponent<NPCManager>(FindObjectsOfType<GameObject>());

        player = GameObject.FindWithTag("Player");  //플레이어 검색

        saves = new SAVES();
        saves.chapter = player.GetComponent<PlayerStatus>().GetChapter();
        saves.player = new PlayerSave(player.transform.position);
        
        saves.npcs = new List<NPCSave>();

        for(int i = 0; i < NPCs.Count; i++)
        {
            NPCManager temp = NPCs[i].GetComponent<NPCManager>();
            saves.npcs.Add(new NPCSave(temp.ID, NPCs[i].transform.position, temp.i_Story));
        }

        saves.SortByID();      //정렬된 상태로 파싱하기

        string path = Application.persistentDataPath + "/saves/";
        string filePath = path + savefilename + ".json";
        string jsonData;

        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        if(!File.Exists(filePath))
        {
            FileStream temp = File.Create(filePath);
            temp.Close();
        }

        jsonData = JsonUtility.ToJson(saves, true);

        File.WriteAllText(filePath, jsonData);  //저장하기 덮어쓰기
    }

    public void LOAD()
    {
        string path = Application.persistentDataPath + "/saves/";
        string filePath = path + savefilename + ".json";

        if(!File.Exists(filePath))
        {
            NoFile.Invoke();
            return;
        }

        string jsonData = File.ReadAllText(filePath);
        saves = JsonUtility.FromJson<SAVES>(jsonData);

        isLoad = true;

        SceneController.LoadNextScene(saves.chapter);
    }
    /// <summary>
    /// 세이브 파일 로드 이후 NPC 설정을 적용하는 함수
    /// </summary>
    public void SetNPCs()
    {
        List<GameObject> NPCs = FindByComponent<NPCManager>(FindObjectsOfType<GameObject>());
        SortByID(ref NPCs); //정렬하기
        
        ///플레이어 및 npc 데이터 적용 부분

        player.GetComponent<PositionManager>().SetPos(saves.player.GetPos());

        for(int i = 0; i < NPCs.Count; i++) //데이터 반영
        {
            NPCs[i].GetComponent<NPCManager>().i_Story = saves.npcs[i].storyLine;
            NPCs[i].GetComponent<PositionManager>().SetPos(saves.npcs[i].GetPos());
        }
    }

    /// <summary>
    /// 인자로 주어진 게임오브젝트 배열에서 원하는 컴포넌트가 들어있는 오브젝트들의 리스트를 반환한는 함수
    /// </summary>
    /// <param name="objs">검색될 배열</param>
    public List<GameObject> FindByComponent<T>(GameObject[] objs)
    {
        List<GameObject> result = new List<GameObject>();

        for(int i = 0; i < objs.Length; i++)
        {
            T temp;
            if(objs[i].TryGetComponent<T>(out temp))
            {
                result.Add(objs[i]);
            }    
        }
        return result;
    }

    /// <summary>
    /// NPCManager 오브젝트들을 ID값을 이용하여 오름차순으로 정렬
    /// </summary>
    void SortByID(ref List<GameObject> npcs)
    {
        for(int i = 0; i < npcs.Count - 1; i++)
            {
                for(int j = i + 1; j < npcs.Count; j++)
                {
                    if(npcs[i].GetComponent<NPCManager>().ID > npcs[j].GetComponent<NPCManager>().ID) 
                    {
                        GameObject temp = npcs[i];
                        npcs[i] = npcs[j];
                        npcs[j] = temp;
                    }
                }
            }
    }
    

    [System.Serializable]
    class SAVES
    {
        public string chapter;
        public PlayerSave player;
        public List<NPCSave> npcs;

        /// <summary>
        /// npcs를 ID값을 이용하여 오름차순으로 정렬
        /// </summary>
        public void SortByID()
        {
            for(int i = 0; i < npcs.Count - 1; i++)
            {
                for(int j = i + 1; j < npcs.Count; j++)
                {
                    if(npcs[i].id > npcs[j].id) 
                    {
                        NPCSave temp = npcs[i];
                        npcs[i] = npcs[j];
                        npcs[j] = temp;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 플레이어 위치, 인벤토리 등등 저장
    /// </summary>
    [System.Serializable] 
    class PlayerSave
    {
        public float x, y, z; //vector3 pos

        public PlayerSave(Vector3 pos)
        {
            x = pos.x; y = pos.y; z = pos.z;
        }

        public Vector3 GetPos()
        {
            return new Vector3(x,y,z);
        }
    }
    [System.Serializable]
    class NPCSave
    {
        public int id;
        public float x, y, z;
        public int storyLine;

        public NPCSave(int id, Vector3 pos, int storyLine)
        {
            x = pos.x; y = pos.y; z = pos.z;
            this.id = id;
            this.storyLine = storyLine;
        }
        public Vector3 GetPos()
        {
            return new Vector3(x,y,z);
        }
    }
}
