using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Inventory
{
    sbyte index;
    List<Page> pages;

    public Inventory()
    {
        pages = new List<Page>();
        index = 0;
        /*
            추후에 json파일로 인벤토리 내역을 불러오는 로직 추가 예정
        */
        for(int i = 0; i < 5; i++)
            pages.Add(new Page((sbyte)i));
    }
    public int GetPage()   //페이지 정보 불러오기
    {
        return pages[index].GetNum();
    }

    public void MoveLeftPage()   //왼쪽 페이지로 이동
    {
        if(index == 0)
            return;

        index -= 1;
    }

    public void MoveRightPage()  //오른쪽 페이지로 이동
    {
        if(index == pages.Count - 1 || pages.Count == 0)
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

    class Page
    {
        sbyte num;
        public Page(sbyte num)
        {
            this.num = num;
        }

        public int GetNum()
        {
            return num;
        }
    }
}
