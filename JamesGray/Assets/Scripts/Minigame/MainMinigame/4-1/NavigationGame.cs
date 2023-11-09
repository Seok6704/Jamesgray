using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationGame : MonoBehaviour
{
    FixedFollowCamera cam;

    void Start()
    {
        cam.SetForceFollow(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
