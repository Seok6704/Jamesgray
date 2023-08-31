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
    public List<TMP_Text> texts;
    public List<Image> sprites;

    public void SetContents(List<Inventory.Content> contents, AssetBundle assetBundle)
    {   //굳이 한 for문 안에 다 넣지 않은 이유는 텍스트와 스프라이트의 수가 일치한다는 보장을 할 수 없으므로... 스프라이트만 있는 레이아웃일 경우를 위해서 이와 같이함.
        int i;
        for(i = 0; i < sprites.Count; i++)
        {
            if(assetBundle.Contains(contents[i].sprite))
            {
                sprites[i].sprite = assetBundle.LoadAsset<Sprite>(contents[i].sprite);
            }
            else
            {
                Debug.Log("NO SUCH SPRITE");
            }
        }

        for(i = 0; i < texts.Count; i++)
        {
            texts[i].text = contents[i].content;
        }
    }

}
