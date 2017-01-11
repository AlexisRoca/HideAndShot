using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenAgent : Agent
{
    public Controller m_controller;
    public float walkspeed = 100.0f;

    //public override void updateAgent(float dTime)
    //{
    //    Vector2 speed;
    //    speed.x = m_controller.horizontalAxis() * walkspeed;
    //    speed.y = m_controller.verticalAxis() * walkspeed;

    //    _position = _position + speed * dTime;
    //}

    void Update()
    {
        Vector2 speed = Vector2.zero;
        speed.x = m_controller.horizontalAxis() * walkspeed;
        speed.y = m_controller.verticalAxis() * walkspeed;

        speed = (speed.Equals(Vector2.zero)) ? -_velocity * 0.2f : speed;

        _steeringForce = speed;
    }
}