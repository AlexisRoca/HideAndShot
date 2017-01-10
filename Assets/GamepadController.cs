using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadController : Controller
{
    public string gamepadId;

    public override float horizontalAxis()  { return Input.GetAxis("LeftJoystickX_P" + gamepadId); }
    public override float verticalAxis()    { return Input.GetAxis("LeftJoystickY_P" + gamepadId); }
    public override bool actionButton()     { return Input.GetButtonDown("A_P" + gamepadId); }
}