using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScrollManager : MonoBehaviour
{
    public ScrollRect scroll;
    public RectTransform contents;
    public UnityEvent scrollDone;

    bool stopScroll = false;
    Vector3 orgin;
    Coroutine co = null;
    

    public void StartScroll()
    {
        stopScroll = false;
        co = null;
        scroll.verticalScrollbar.interactable = false;
        //scroll.verticalScrollbar.handleRect.anchoredPosition;
        Invoke("StartCo", 2f);
    }

    void StartCo()
    {
        orgin = contents.position;
        co = StartCoroutine("ScrollDown");
    }

    IEnumerator ScrollDown()
    {
        while(true)
        {
            contents.position = Vector2.MoveTowards(contents.position, new Vector2(contents.position.x, contents.position.y + 100), 1f);
            yield return null;

            if(stopScroll)
            {
                break;
            }
        }
        scrollDone.Invoke();
    }

    public void ResetCredit()
    {
        if(!ReferenceEquals(null, co))
        {
            StopCoroutine(co);
        }
        contents.position = orgin;
    }

    public void SetProgress(Vector2 input)
    {
        //Debug.Log(input);
        if(input.y < 0.07f)
        {
            stopScroll = true;
        }
    }
}
