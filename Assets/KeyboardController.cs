using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : Controller
{
    public override float horizontalAxis()
    {
        if(Input.GetKeyDown(KeyCode.Q))
            return -1.0f;
        else if(Input.GetKeyDown(KeyCode.D))
            return 1.0f;
        else
            return 0.0f;
    }

    public override float verticalAxis()
    {
        if(Input.GetKeyDown(KeyCode.Z))
            return -1.0f;
        else if(Input.GetKeyDown(KeyCode.S))
            return 1.0f;
        else
            return 0.0f;
    }

    public override bool actionButton() { return Input.GetKeyDown(KeyCode.Space); }
}
