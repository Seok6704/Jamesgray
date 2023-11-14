using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationGame : MonoBehaviour
{
    public FixedFollowCamera cam;
    int round = 0;
    public GameObject Dialog;
    PlayerController_v3 pc;

    void Start()
    {
        cam.SetForceFollow(true);
        pc = GameObject.Find("Player").GetComponent<PlayerController_v3>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.name)
        {
            case "DB" :
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 6);
                pc.ChangeisOn();
                ClearAndFail.GameFail();
                Invoke("OnReset", 2f);
                break;
            case "JS" :
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 6);
                pc.ChangeisOn();
                ClearAndFail.GameFail();
                Invoke("OnReset", 2f);
                break;
            case "GS" :
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 6);
                pc.ChangeisOn();
                ClearAndFail.GameFail();
                Invoke("OnReset", 2f);
                break;
            case "SG" :
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 6);
                pc.ChangeisOn();
                ClearAndFail.GameFail();
                Invoke("OnReset", 2f);
                break;
            case "UR" :
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 6);
                pc.ChangeisOn();
                ClearAndFail.GameFail();
                Invoke("OnReset", 2f);
                break;
            case "Start" :
                if(round == 0) round++;
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 1);
                break;
            case "BB" :
                if(round == 1) round++;
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 2);
                break;
            case "SS" :
                if(round == 2) round++;
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 3);
                break;
            case "GSA" :
                if(round == 3) round++;
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 4);
                break; 
            case "SGA" :
                if(round == 4) round++;
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 5);
                break;
            case "End" :
                Dialog.GetComponent<DialoguesManager>().SetDialogue(900, 7);
                pc.ChangeisOn();
                ClearAndFail.GameClear();
                Invoke("EndGame", 2f);
                break;
        }
    }

    void OnReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void EndGame()
    {
        SceneManager.LoadScene("Chapter4-1");
    }
}
