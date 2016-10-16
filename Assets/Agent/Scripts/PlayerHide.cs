using UnityEngine;
using System.Collections;

public class PlayerHide : Agent
{

    public float sensibility = 0.0f;


    // Reimplemented function for different behavior
    override protected Vector2 steeringForces()
    {
        Vector2 force = Vector2.zero;

        force += (Input.GetKey(KeyCode.Z)) ? new Vector2(0, 1) : Vector2.zero;
        force += (Input.GetKey(KeyCode.S)) ? new Vector2(0, -1) : Vector2.zero;
        force += (Input.GetKey(KeyCode.Q)) ? new Vector2(-1, 0) : Vector2.zero;
        force += (Input.GetKey(KeyCode.D)) ? new Vector2(1, 0) : Vector2.zero;

        _velocity = force * sensibility;

        return Vector2.zero;
    }
}