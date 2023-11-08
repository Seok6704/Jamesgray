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
    InvSave save;   //저장데이터
    List<Page> pages;
    string savefilename = "INV01";

    Page nullPage;

    public Inventory()
    {
        save = LoadInventory();
        pages = save.pages;
        index = 0;
        nullPage = new Page(-1, "", "", "")
        {
            pageContext = ""
        };

    }
    public int GetPageNum()   //페이지 정보 불러오기
    {
        return pages[index].GetNum();
    }

    /// <summary>
    /// 현재 인덱스의 페이지를 반환한다.
    /// </summary>
    /// <returns>현재 인덱스가 가르키고 있는 지점의 페이지를 반환</returns>
    public Page GetPage()
    {
        return pages[index];
    }
    /// <summary>
    /// 주어진 인덱스의 페이지를 반환한다. 만약 인덱스가 벗어났다면 null 반환
    /// </summary>
    /// <param name="i">받고자 하는 페이지의 인덱스</param>
    /// <returns>해당 인덱스의 페이지</returns>
    public Page GetPage(int i)
    {
        if(0 > i || i > pages.Count) return nullPage;

        return pages[i - 1];
    }

    /// <summary>
    /// 현재 인덱스의 값을 범위 내에서 감소.
    /// </summary>
    public void MoveLeftPage()   //왼쪽 페이지로 이동
    {
        if(IsLeftEnd())
            return;

        index -= 2;
    }
    /// <summary>
    /// 현재 인덱스의 값을 범위 내에서 증가.
    /// </summary>
    public void MoveRightPage()  //오른쪽 페이지로 이동
    {
        if(IsRightEnd())
            return;

        index += 2;
    }

    public bool IsLeftEnd()
    {
        if(index == 0) return true;
        return false;
    }
    public bool IsRightEnd()
    {
        if(index + 2 >= pages.Count || pages.Count == 0) return true;
        return false;
    }

    /// <summary>
    /// 마지막 페이지 다음에 새 페이지를 추가함.
    /// </summary>
    /// <param name="title">제목</param>
    /// <param name="contents">내용</param>
    /// <param name="context">페이지 종류</param>
    /// <param name="sprite">스프라이트 경로. 현재는 사용되지 않음</param>
    public void AddPage(string title, string contents, string context, string sprite = "")
    {
        pages.Add(new Page((sbyte)(pages.Count + 1), sprite, title, contents)  //마지막 장 다음에 새 페이지 추가
        {
            pageContext = context  //MAIN PAGE 생성
        });
    }
    public void SaveInventory()
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
        save.pages = pages; //업데이트
        jsonData = JsonUtility.ToJson(save, true);
        Debug.Log(jsonData);

        File.WriteAllText(filePath, jsonData);  //저장하기 덮어쓰기
    }

    InvSave LoadInventory()
    {
        string path = Application.persistentDataPath + "/saves/";
        string filePath = path + savefilename + ".json";

        if(!File.Exists(filePath))
        {
            return MakeDefaultPages();
        }

        string jsonData = File.ReadAllText(filePath);

        return JsonUtility.FromJson<InvSave>(jsonData);
    }

    InvSave MakeDefaultPages()
    {   
        string title = " \"탐정\" 제임스 그레이";
        string contents = "";
        List<Page> newPages = new List<Page>();
        newPages.Add(new Page(1, "", title, contents) 
        {
            pageContext = "인적 사항"   //MAIN PAGE 생성
        });
        InvSave inv = new InvSave
        {
            pages = newPages
        };
        return inv;
    }
    [System.Serializable]
    public class InvSave
    {
        public List<Page> pages;
    }
    [System.Serializable]
    public class Page
    {
        public sbyte num;
        public string pageContext;  //페이지 종류
        public Content content; //페이지 내용용
        //public List<Content> lContents;
        //public List<Content> rContents;
        
        public Page(sbyte num, string sprite, string title, string contents)
        {
            this.num = num;
            //lContents = new List<Content>();
            //rContents = new List<Content>();
            //this.content = new Content();
            content = new Content("", title, contents); //스프라이트는 없음
        }

        public int GetNum()
        {
            return num;
        }
    } 
    [System.Serializable]
    public class Content
    {
        public string sprite;       //보여줄 스프라이트 경로, 사용되지 않을예정
        public string title; //제목
        public string content;      //인벤토리에서 설명으로 사용될 내용

        public Content(string sprite, string title, string content)
        {
            this.sprite = sprite;
            this.title = title;
            this.content = content;
        }
    }
}
