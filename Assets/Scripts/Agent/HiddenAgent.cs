using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenAgent : Agent
{
    public Controller m_controller;
    public float walkspeed;

    public override void updateAgent(float dTime)
    {
        Vector2 speed;
        speed.x = m_controller.horizontalAxis() * walkspeed;
        speed.y = m_controller.verticalAxis() * walkspeed;

        _position = _position + speed * dTime;
    }
}