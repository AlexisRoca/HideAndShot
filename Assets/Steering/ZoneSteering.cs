using UnityEngine;
using System.Collections;

public class ZoneSteering {

    // Steering force to push the agent out the zone
    public static Vector2 pushOut(Vector2 agentPosition, bool upSide, bool rightSide, bool downSide, bool leftSide, Vector2 min, Vector2 max) {
        float minDistance = float.MaxValue;
        Vector2 force = Vector2.zero;

        if (upSide) {
            float distance = (agentPosition - new Vector2(agentPosition.x, max.y)).magnitude;
            if (distance < minDistance) {
                minDistance = distance;
                force = new Vector2(0.0f, 1.0f);
            }
        }

        if (rightSide) {
            float distance = (agentPosition - new Vector2(max.x, agentPosition.y)).magnitude;
            if (distance < minDistance) {
                minDistance = distance;
                force = new Vector2(1.0f, 0.0f);
            }
        }

        if (downSide) {
            float distance = (agentPosition - new Vector2(agentPosition.x, min.y)).magnitude;
            if (distance < minDistance) {
                minDistance = distance;
                force = new Vector2(0.0f, -1.0f);
            }
        }

        if (leftSide) {
            float distance = (agentPosition - new Vector2(min.x, agentPosition.y)).magnitude;
            if (distance < minDistance) {
                minDistance = distance;
                force = new Vector2(-1.0f, 0.0f);
            }
        }

        return force;
    }


    // Steering force to push the agent trough the zone
    public static Vector2 pushThrough(Vector2 agentVelocity, bool upSide, bool rightSide, bool downSide, bool leftSide) {
        float minAngle = float.MaxValue;
        Vector2 force = Vector2.zero;

        if (upSide) {
            Vector2 direction = new Vector2(0.0f, 1.0f);
            float angle = Mathf.Acos(Vector2.Dot(agentVelocity.normalized, direction));
            if (angle < minAngle) {
                minAngle = angle;
                force = direction;
            }
        }

        if (rightSide) {
            Vector2 direction = new Vector2(1.0f, 0.0f);
            float angle = Mathf.Acos(Vector2.Dot(agentVelocity.normalized, direction));
            if (angle < minAngle) {
                minAngle = angle;
                force = direction;
            }
        }

        if (downSide) {
            Vector2 direction = new Vector2(0.0f, -1.0f);
            float angle = Mathf.Acos(Vector2.Dot(agentVelocity.normalized, direction));
            if (angle < minAngle) {
                minAngle = angle;
                force = direction;
            }
        }

        if (leftSide) {
            Vector2 direction = new Vector2(-1.0f, 0.0f);
            float angle = Mathf.Acos(Vector2.Dot(agentVelocity.normalized, direction));
            if (angle < minAngle) {
                minAngle = angle;
                force = direction;
            }
        }

        return force;
    }
}
