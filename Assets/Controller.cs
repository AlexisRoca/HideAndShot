using UnityEngine;
using System.Collections;

public class Controller
{
    public int gamepadId;

    public float    horizontalAxis()    { return Input.GetAxis("LeftJoystickX_P" + gamepadId.ToString());   }
    public float    verticalAxis()      { return Input.GetAxis("LeftJoystickY_P" + gamepadId.ToString());        }
    public bool     actionButton()      { return Input.GetButtonDown("A_P" + gamepadId.ToString());         }   
}