using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tools;
using Unity.VisualScripting;

/// <summary>
/// 인벤토리 표시를 관리하는 클래스, 인벤토리 데이터 생성을 명령하고, 페이지 넘기기 데이터 추가 명령 등 인벤토리 표시 및 관리를 담당
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent LeftEnd, RightEnd, EnableAll;
    static Inventory inventory = null;
    public List<PageManager> Pages; 
    public TMPro.TMP_Text textL, textR;

    Animator animator;
    
    //static AssetBundleManager assetBundle;
    //static string bundleName = "inventory_assets";

    private void Start() 
    {
        if(ReferenceEquals(null, inventory))
            inventory = new Inventory();
        
        //if(assetBundle == null)
        //{
            //assetBundle = new AssetBundleManager(bundleName);
        //}
        
        animator = GetComponent<Animator>();

        ShowInventory();
    }

    public void ShowInventory()
    {
        int num = inventory.GetPageNum();
        //textR.text = (inventory.GetPageNum() + 1).ToString();
        ShowPage(textL, num);
        ShowPage(textR, num + 1);

        EnableAll.Invoke(); //일단 모든 화살표를 이벤트로 활성화 한 후, 끝에 도달한 곳은 비활성화.

        if(inventory.IsLeftEnd()) LeftEnd.Invoke();
        if(inventory.IsRightEnd()) RightEnd.Invoke();    
    }
    /// <summary>
    /// 책에 내용을 표시하기 위해 text 객체를 전달하면 자식 텍스트에 내용을 입력한다. 따라서 인자값은 3개의 자녀가 필요하다.
    /// </summary>
    /// <param name="texts">TMP_Text 자녀가 3개있는 Text 객체</param>
    void ShowPage(TMPro.TMP_Text pagetext, int pageNum)
    {
        Inventory.Page page = inventory.GetPage(pageNum);
        pagetext.text = page.GetNum() == -1 ? "" : page.GetNum().ToString();    //-1페이지라면 빈칸출력
        
        TMPro.TMP_Text[] texts = pagetext.transform.GetComponentsInChildren<TMPro.TMP_Text>();
        string[] textarray = {
            page.pageContext,
            page.content.title,
            page.content.content
        };

        SetPageText(texts, textarray);
    }

    void SetPageText(TMPro.TMP_Text[] texts, string[] textarray)
    {
        for(int i = 1; i <= texts.Length && i <= textarray.Length; i++) //GetComponentsInChildren이 자기자신의 컴포넌트까지 반환하는듯....
        {
            texts[i].text = textarray[i - 1];
        }
    }

    public void AddPage(string title, string contents, string context, string sprite = "")
    {
        inventory.AddPage(title, contents, context);
    }

    public void GetLeftPage()
    {
        inventory.MoveLeftPage();
        ShowInventory();

        animator.SetTrigger("LEFT");
    }
    public void GetRightPage()
    {
        inventory.MoveRightPage();
        ShowInventory();

        animator.SetTrigger("RIGHT");
    }

    private void OnDestroy() 
    {
        inventory.SaveInventory();
        inventory = null;    
    }
}
