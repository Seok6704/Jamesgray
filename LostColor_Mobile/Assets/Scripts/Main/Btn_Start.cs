using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Btn_Start : MonoBehaviour
{
   public GameObject panel;
   public UnityEvent start_Btn_ONClick;
   public void OnClikcK_Btn(){
      Debug.Log("StartBtn");
      start_Btn_ONClick.Invoke();
      //panel.SetActive(false);
   }
}
