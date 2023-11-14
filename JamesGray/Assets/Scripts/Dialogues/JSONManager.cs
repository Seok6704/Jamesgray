using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*  ////////////////////////////////////////////

    Json 파일을 통해 대화 시스템을 구현.
    이 스크립트는 그 중 해당 챕터의 대화를 전부 로드하는 역할을 합니다.
    생성자에 변수로 해당 챕터의 스트링(씬 이름) 을 받으면 해당하는 씬의
    json파일을 찾아 로드하는 방식으로 구성합니다.

    Json 파일의 변수이름과 아래 클래스들의 변수 이름은 일치해야 정상작동하므로 데이터를 추가할때 이 점 주의하시기 바랍니다.
    또한 자료형도 같아야하며 배열 형식은 리스트나 배열로 받아들여야합니다.
    또한 새로운 객체를 추가할때는 class로 받아야하며 [Serializable]을 꼭 붙여주시기 바랍니다.

    아래 GetName, GetContent 함수를 이용해 대화 내용을 반환합니다.
    dial 변수가 private이기 때문에 함수들로 접근해야합니다.
    lineID는 각 NPC의 변수값을 통해 알아낼수있으므로 따로 Dictionary화 하지 않았습니다.
    순차적으로 증가할 것이라 예상되므로 ++ 연산자로도 충분히 동작할것이라 생각됩니다.

*/  ////////////////////////////////////////////
public class JSONManager
{
    Dialogue dial;

    [System.Serializable]
    class Dialogue
    {
        public string spritePath;
        public string audioPath;
        public NPC_Class[] NPC;
        public Dictionary<int, int> index;

        public void SetDictionary()   // 검색의 효율을 위해 딕셔너리로 ID값을 통해 인덱싱하기 (조금 과한것일수도?)
        {
            index = new Dictionary<int, int>();

            for(int i = 0; i < NPC.Length; i++)
            {
                index.Add(NPC[i].ID, i);
            }
        }
        /// <summary>
        /// 디버그 용 함수입니다. 사용하지말아주세요.
        /// </summary>
        public void printContent(int index) //Debug 용도
        {
            Debug.Log(NPC[index].ID);
            Debug.Log(NPC[index].NPCName);
            Debug.Log(NPC[index].storyLine[0].lineID);
            Debug.Log(NPC[index].storyLine[0].content[0]);
            Debug.Log(NPC[index].storyLine[0].content[1]);
        }
    }

    [System.Serializable]
    class NPC_Class
    {
        public int ID;
        public string NPCName;
        public string[] defaultLine;    //할말없을때 NPC가 할 말들
        public StoryLineClass[] storyLine;
        public CodexClass[] codex;
    }

    [System.Serializable]
    class StoryLineClass
    {
        public int lineID;
        public string[] content;
    }

    [System.Serializable]
    class CodexClass
    {
        public int num;
        public string[] codexes;
    }

    public JSONManager(string sceneName)
    {
        //string filePath = Application.dataPath + "/Data/Dialogues/" + sceneName + ".json";
        string filePath = Application.streamingAssetsPath + "/Dialogues/" + sceneName + ".json";
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW w = new WWW(filePath);
            Debug.Log(filePath);
            while(!w.isDone);
            string androidJson = w.text;
            dial = JsonUtility.FromJson<Dialogue>(androidJson);
            dial.SetDictionary();
            return;
        }
        else if(!File.Exists(filePath)) return; 
        string jsonText = File.ReadAllText(filePath);
        dial = JsonUtility.FromJson<Dialogue>(jsonText);

        dial.SetDictionary();
    }

    int FindIndexFromKey(int id)     //id 값을 통해 인덱스 값을 찾아내는 함수
    {
        return dial.index[id];
    }

    public string GetName(int id)
    {
        return dial.NPC[FindIndexFromKey(id)].NPCName;
    }

    public string[] GetContent(int id, int lineID)
    {
        return dial.NPC[FindIndexFromKey(id)].storyLine[lineID].content;
    }

    public string GetSpritePath() 
    {
        return dial.spritePath;
    }

    public string GetAudioPath()
    {
        return dial.audioPath;
    }
    public int GetContentLength(int id, int lineID)
    {
        return dial.NPC[FindIndexFromKey(id)].storyLine[lineID].content.Length;
    }
    public int GetStoryLineLength(int id)
    {
        return dial.NPC[FindIndexFromKey(id)].storyLine.Length;
    }

    public int GetDefaultLength(int id)
    {
        return dial.NPC[FindIndexFromKey(id)].defaultLine.Length;
    }
    public string GetContent(int id, int lineID, int i)     //한문장 반환
    {
        return dial.NPC[FindIndexFromKey(id)].storyLine[lineID].content[i];
    }
    public string[] GetContents(int id, int lineID)
    {
        return dial.NPC[FindIndexFromKey(id)].storyLine[lineID].content;
    }

    public string GetDefaultLine(int id)
    {
        string[] temp = dial.NPC[FindIndexFromKey(id)].defaultLine;
        int max = temp.Length;
        return temp[Random.Range(0, max)];

    }
    public string[] GetCodexLine(int id, int codexIndex)
    {
        return dial.NPC[FindIndexFromKey(id)].codex[codexIndex].codexes;
    }
}
