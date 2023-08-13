using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.IO;
using UnityEngine;

/// <summary>
/// 인벤토리 데이터를 가지는 클래스, 인벤토리 내용을 저장, 불러오기 및 데이터 관리를 한다. 
/// </summary>
public class Inventory
{
    sbyte index;
    List<Page> pages;
    string savefilename = "INV01";

    public Inventory()
    {
        pages = LoadInventory();
        index = 0;

    }
    public int GetPageNum()   //페이지 정보 불러오기
    {
        return pages[index].GetNum();
    }

    public Page GetPage()
    {
        return pages[index];
    }

    public void MoveLeftPage()   //왼쪽 페이지로 이동
    {
        if(IsLeftEnd())
            return;

        index -= 1;
    }

    public void MoveRightPage()  //오른쪽 페이지로 이동
    {
        if(IsRightEnd())
            return;

        index += 1;
    }

    public bool IsLeftEnd()
    {
        if(index == 0) return true;
        return false;
    }
    public bool IsRightEnd()
    {
        if(index == pages.Count - 1 || pages.Count == 0) return true;
        return false;
    }

    void SaveInventory()
    {
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

        jsonData = JsonUtility.ToJson(pages);

        File.WriteAllText(filePath, jsonData);  //저장하기 덮어쓰기
    }

    List<Page> LoadInventory()
    {
        string path = Application.persistentDataPath + "/saves/";
        string filePath = path + savefilename + ".json";

        if(!File.Exists(filePath))
        {
            return MakeDefaultPages();
        }

        string jsonData = File.ReadAllText(filePath);

        return JsonUtility.FromJson<List<Page>>(jsonData);
    }

    List<Page> MakeDefaultPages()
    {   
        //일단 필요하다고 생각되는 페이지 : 1. 목표, 2. 아이템, 3. 일지
        List<Page> newPages = new List<Page>();
        for(int i = 0; i < 2; i++)  //디폴트로 3페이지 생성하기
        {
            newPages.Add(new Page((sbyte)i) {pageContext = (CONTEXT.OBJECTIVE + i).ToString()});
        }

        return newPages;
    }

    private void OnDestroy() 
    {
        SaveInventory();    
    }

    [System.Serializable]
    public class Page
    {
        public sbyte num;
        public string pageContext;
        public List<Content> lContents;
        public List<Content> rContents;
        
        public Page(sbyte num)
        {
            this.num = num;
            lContents = new List<Content>();
            rContents = new List<Content>();
        }

        public int GetNum()
        {
            return num;
        }
    } 
    [System.Serializable]
    public class Content
    {
        public string spritePath;   //보여줄 스프라이트 경로
        public string content;      //인벤토리에서 설명으로 사용될 내용
    }

    public enum CONTEXT
    {
        OBJECTIVE,
        JOURNAL
    }
}
