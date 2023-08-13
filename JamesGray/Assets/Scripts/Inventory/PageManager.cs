using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Tools;

/// <summary>
/// 인벤토리 페이지 마다 구조가 다르므로 이 클래스에서 데이터가 주어지면 알맞은 형식으로 Grid Layout Group을 사용하여 표시하는 역할을 맡는다.
/// </summary>
public class PageManager : MonoBehaviour
{
    public List<TMP_Text> contents;
    public List<Image> sprites;

    public void SetContents(List<Inventory.Content> contents)
    {
        for(int i = 0; i < sprites.Count; i++)
        {
            
        }
    }

}
