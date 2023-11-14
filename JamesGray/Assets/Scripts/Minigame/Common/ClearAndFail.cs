using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAndFail : MonoBehaviour
{
    // Start is called before the first frame update
    public static void GameClear()
    {
        GameObject.Find("Panel_Clear").GetComponent<UI_Mover>().SetPos2Parent();
    }

    public static void GameFail()
    {
        GameObject.Find("Panel_Fail").GetComponent<UI_Mover>().SetPos2Parent();
    }
}
