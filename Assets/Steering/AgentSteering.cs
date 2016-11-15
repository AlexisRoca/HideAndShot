using UnityEngine;
using System.Collections;

public class AgentSteering{

    // Leader behavior
    public static Vector2 leader (Vector2 agentVelocity, int radiusCircle, float variation, ref float anglePoint) {
        anglePoint = (anglePoint + (Random.Range(-1.0f, 1.0f)) * variation) % (2 * Mathf.PI);

        Vector2 force = new Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radiusCircle;
        force += agentVelocity.normalized * radiusCircle;

        return force;
    }


    // Follow behavior
    public static Vector2 follow (Vector2 agentPosition, Vector2 targetPosition, Vector2 targetVelocity, float dTime) {
        Vector2 predictedLeaderPosition = targetPosition;//;+ targetVelocity//; *dTime;
        Vector2 force = predictedLeaderPosition - agentPosition;

        return force;
    }


    // Stay out behavior
    public static Vector2 stayOut(Vector2 agentPosition, Vector2 targetPosition, Vector2 targetVelocity) {
        // Compute the circle area to stay out
        Vector2 leaderAreaCenter = targetPosition + targetVelocity;

        // Compute the distance to this circle
        float distance = (agentPosition - leaderAreaCenter).magnitude;

        // If the agent is in the circle, apply a steering force
        // Vector2 force = (distance < targetVelocity.magnitude) ? (agentPosition - leaderAreaCenter).normalized / distance : Vector2.zero;
        Vector2 force = (distance < targetVelocity.magnitude) ? (agentPosition - leaderAreaCenter).normalized : Vector2.zero;
        return force;
    }


    // Avoid behavior
    public static Vector2 avoid (Vector2 agentPosition, Vector2 agentVelocity, ref float wander) {
        Vector2 force = Vector2.zero;

        Vector3 origine = new Vector3(agentPosition.x, 10.0f, agentPosition.y);
        Vector3 direction = new Vector3(agentVelocity.x, 10.0f, agentVelocity.y);

        RaycastHit hit;

        if (Physics.Raycast(origine, direction, out hit, 50.0f)) {
            if (hit.collider.GetType() != typeof(Agent)) {
                force = - agentVelocity * 2.0f;
                wander += Mathf.PI;
            }
        }

        return force;
    }


    // Separation behavior
    public static Vector2 separation (Vector2 agentPosition, Vector2 [] neighboursPosition) {
        Vector2 force = Vector2.zero;

        // For each agent
        foreach (Vector2 currentPosition in neighboursPosition) {
            // Compute the distance to the other follower
            Vector2 positionDifference = agentPosition - currentPosition;
            float distance = positionDifference.magnitude;

            // If he is in the living space
            force += positionDifference.normalized / distance;
        }

        return force;
    }


    // Hide Player control
    public static Vector2 player (Vector2 agentVelocity) {
        Vector2 force = Vector2.zero;

        force += (Input.GetKey(KeyCode.Z)) ? new Vector2(0, 1) : Vector2.zero;
        force += (Input.GetKey(KeyCode.S)) ? new Vector2(0, -1) : Vector2.zero;
        force += (Input.GetKey(KeyCode.Q)) ? new Vector2(-1, 0) : Vector2.zero;
        force += (Input.GetKey(KeyCode.D)) ? new Vector2(1, 0) : Vector2.zero;

        force = (force.Equals(Vector2.zero)) ? -agentVelocity*0.2f : force;

        return force;
    }
}
