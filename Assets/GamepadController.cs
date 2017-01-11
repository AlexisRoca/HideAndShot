using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadController : Controller
{
    public string m_gamepadId;
    float m_sensibility = 50.0f;

    public GamepadController(string gamepadId)
    {
        m_gamepadId = gamepadId;
    }

    public override float horizontalAxis()  { return Input.GetAxis("LeftJoystickX_P" + m_gamepadId) * m_sensibility; }
    public override float verticalAxis()    { return Input.GetAxis("LeftJoystickY_P" + m_gamepadId) * m_sensibility; }
    public override bool actionButton()     { return Input.GetButtonDown("A_P" + m_gamepadId); }
}