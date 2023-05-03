using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class Image_Click : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onClick, onEnter, onExit; 

	public void OnPointerClick(PointerEventData eventData)
	{
		//Debug.Log("click");
        onClick.Invoke();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		//Debug.Log("enter");
        onEnter.Invoke();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//Debug.Log("exit");
        onExit.Invoke();
	}
}
