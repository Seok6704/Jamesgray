using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationGame : MonoBehaviour
{
    public FixedFollowCamera cam;

    void Start()
    {
        cam.SetForceFollow(true);
    }
}
