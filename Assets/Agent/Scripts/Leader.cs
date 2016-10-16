using UnityEngine;
using System.Collections;

public class Leader : Agent {
    
    // Define the wander circle behavior
    public int radiusCircle = 0;
    public float variation = 0.0f;

    // Define the random point position around the circle
    private float anglePoint = 0.0f;

    // Coefficient of avoiding
    public float coefAvoiding = 0.0f;


    // Reimplemented function for different behavior
    override protected Vector2 steeringForces () {
        Vector2 force = avoidObstacles();

        force += (force.Equals(Vector2.zero)) ? wanderWalk() : Vector2.zero;

        return force;
    }


    // Implement the wander walk for Leaders
    protected Vector2 wanderWalk () {
        anglePoint = (anglePoint + (Random.Range(-1.0f, 1.0f)) * variation) % (2 * Mathf.PI);

        Vector2 force = new Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radiusCircle;

        return force;
    }


    // Implement the obstacle avoid
    protected Vector2 avoidObstacles () {
        Vector2 force = Vector2.zero;

        Vector3 origine = new Vector3(_position.x, 10.0f, _position.y);
        Vector3 direction = new Vector3(_velocity.x, 10.0f, _velocity.y);

        RaycastHit hit;
                
        if (Physics.Raycast(origine, direction, out hit, 100.0f)) {
            if (! hit.collider.GetComponent<Agent>()) {
                force = _velocity.normalized + new Vector2(hit.normal.x, hit.normal.z);

                float side = Mathf.Acos(Vector2.Dot(_velocity.normalized, -new Vector2(hit.normal.x, hit.normal.z)));
                anglePoint += side;
            }
        }
        
        return force * coefAvoiding;
    }


    // Force Visualization
    void OnDrawGizmosSelected () {
        Vector2 circlePosition = _position + _velocity.normalized * 1.5f * radiusCircle;
        Vector2 pointPosition = circlePosition + new Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radiusCircle;

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
