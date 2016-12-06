using UnityEngine;
using System.Collections;

public class GamepadSelector : MonoBehaviour
{
    public enum Substate
    {
        Static,
        Moving
    };
        
    public string m_id;
    public Vector3 m_initialPosition;
    public Vector3 m_previousPosition;
    public Vector3 m_wantedPosition;
    public Vector3 m_speed;
    public float m_maxSpeed;

    public float K = 0.015f;
    public float friction = 0.94f;

    public Substate m_substate;
    
    public void update(float deltaTime)
    {
        switch(m_substate)
        {
            case Substate.Static:
            break;

            case Substate.Moving:
            {
                m_previousPosition = transform.position;
                m_speed = m_speed * friction;

                Vector3 simulatedNextPosition = transform.position + m_speed * deltaTime;
                Vector3 error = m_wantedPosition - simulatedNextPosition;

                Vector3 acceleration = K * error / deltaTime;
                Vector3 nextSpeed = m_speed + acceleration;

                m_speed.x = Mathf.Clamp(nextSpeed.x,-m_maxSpeed,m_maxSpeed);
                m_speed.y = Mathf.Clamp(nextSpeed.y,-m_maxSpeed,m_maxSpeed);

                transform.position = transform.position + m_speed * deltaTime;
            }
            break;
        }
        m_substate = checkChangeState();
    }

    Substate checkChangeState()
    {
        switch(m_substate)
        {
            case Substate.Static:
            break;

            case Substate.Moving:
                if(((Mathf.Abs(m_speed.x) < 0.1f) && (Mathf.Abs(m_speed.y) < 0.1f))
                   && ((Mathf.Abs(m_wantedPosition.x-transform.position.x)<0.2f) && ((m_wantedPosition.y - transform.position.y)<0.2f)))
                    return Substate.Static;
            break;
        }

        return m_substate;
    }
}
