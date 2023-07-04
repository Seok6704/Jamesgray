using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    public string chapter {get; set;}
    private void Start() 
    {
        chapter = SceneManager.GetActiveScene().name;
    }
    public string GetChapter()
    {
        return chapter;
    }
}
