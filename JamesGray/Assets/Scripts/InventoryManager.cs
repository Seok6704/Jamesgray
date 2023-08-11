using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent LeftEnd, RightEnd, EnableAll;
    static Inventory inventory = null;
    public TMPro.TMP_Text text;

    Animator animator;

    private void Awake() 
    {
        if(inventory == null)   //싱글톤
            inventory = new Inventory();
        
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
