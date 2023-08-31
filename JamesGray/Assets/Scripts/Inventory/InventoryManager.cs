using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tools;

/// <summary>
/// 인벤토리 표시를 관리하는 클래스, 인벤토리 데이터 생성을 명령하고, 페이지 넘기기 데이터 추가 명령 등 인벤토리 표시 및 관리를 담당
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent LeftEnd, RightEnd, EnableAll;
    static Inventory inventory = null;
    public List<PageManager> Pages; 
    public TMPro.TMP_Text text;

    Animator animator;
    
    static AssetBundleManager assetBundle;
    static string bundleName = "inventory_assets";

    private void Awake() 
    {
        if(inventory == null)   //싱글톤
            inventory = new Inventory();
        
        if(assetBundle == null)
        {
            assetBundle = new AssetBundleManager(bundleName);
        }
        
        animator = GetComponent<Animator>();

        ShowInventory();
    }

    void ShowInventory()
    {
        text.text = (inventory.GetPageNum() + 1).ToString();
        

        EnableAll.Invoke(); //일단 모든 화살표를 이벤트로 활성화 한 후, 끝에 도달한 곳은 비활성화.

        if(inventory.IsLeftEnd()) LeftEnd.Invoke();
        if(inventory.IsRightEnd()) RightEnd.Invoke();    
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
        inventory = null;    
    }
}
