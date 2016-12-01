using UnityEngine;
using System.Collections;

public class Gamepad : MonoBehaviour
{
    enum Substate
    {
        Static,
        Moving
    };
        
    public string id;
    public Vector3 initialPosition;
    public Vector3 previousPosition;
    public Vector3 wantedPosition;
    public Vector3 speed;

    public Substate m_substate;
    
    public void update(float deltaTime)
    {

    }

    Substate checkChangeState()
    {
        switch(m_substate)
        {
            case Moving:
            break;
        }
    }
}
