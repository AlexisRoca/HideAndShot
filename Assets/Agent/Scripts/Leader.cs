using UnityEngine;
using System.Collections;

public class Leader : Agent {
    
    // Define the wander circle behavior
    public int radiusCircle = 0;
    public float variation = 0.0f;

    // Define the random point position around the circle
    private float anglePoint = 0.0f;


    // Reimplemented function for different behavior
    override protected Vector2 steeringForces() {
        anglePoint = (anglePoint + (Random.Range(-1.0f, 1.0f)) * variation) % (2 * Mathf.PI);

        // If the agent is close to drop out terrain
        bool borderCondition = _position.x < 50 || _position.x > 450 || _position.y < 50 || _position.y > 450;
        anglePoint = (borderCondition) ? (anglePoint + Mathf.PI) % (2 * Mathf.PI) : anglePoint;
        _velocity= (borderCondition) ? -_velocity : _velocity;

        Vector2 force = new Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radiusCircle;

        return force;
    }


    // Force Visualization
    void OnDrawGizmosSelected () {
        Vector2 circlePosition = _position + _velocity.normalized * 1.5f * radiusCircle;
        Vector2 pointPosition = circlePosition + new Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radiusCircle;

        Vector2 force = new Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radiusCircle;

        Vector3 circlePosition3D = new Vector3(circlePosition.x, 0, circlePosition.y);
        Vector3 pointPosition3D = new Vector3(pointPosition.x, 0, pointPosition.y);
        Vector3 agentPosition3D = new Vector3(_position.x, 0, _position.y);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(circlePosition3D, radiusCircle);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pointPosition3D, 1);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(circlePosition3D, pointPosition3D);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(agentPosition3D, circlePosition3D);
        Gizmos.DrawLine(agentPosition3D, pointPosition3D);

    }
}
