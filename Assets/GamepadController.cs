using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadController : Controller
{
    public string m_gamepadId;

    public GamepadController(string gamepadId)
    {
        m_gamepadId = gamepadId;
    }

    public override float horizontalAxis()  { return Input.GetAxis("LeftJoystickX_P" + m_gamepadId); }
    public override float verticalAxis()    { return Input.GetAxis("LeftJoystickY_P" + m_gamepadId); }
    public override bool actionButton()     { return Input.GetButtonDown("A_P" + m_gamepadId); }
}