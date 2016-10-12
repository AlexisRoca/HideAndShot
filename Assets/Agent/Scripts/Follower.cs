using UnityEngine;
using System.Collections;

public class Follower : Agent {

    public float coefFollow = 1.0f;
    public float coefStayOut = 1.0f;

    // Reimplemented function for different behavior
    override protected Vector2 steeringForces() {
        // Get the leader to follow
        Leader leader = findNearestLeader();

        // Compute steering force
        Vector2 followForce = steeringFollowTheLeader(leader);
        Vector2 stayOutForce = steeringStayOutTheWay(leader);

        return followForce*coefFollow + stayOutForce*coefStayOut;
    }


    // Find the nearest leader in the scene
    protected Leader findNearestLeader () {
        // Get all Leaders in the scene
        GameObject[] leaderList = GameObject.FindGameObjectsWithTag("Leader");

        // Define distance and GameObject for the nearest leader
        Leader nearestLeader = null;
        float nearestDistance = float.MaxValue;

        // Find the nearest to follow
        for (int i = 0; i < leaderList.Length; i++)
        {
            float currentDistance = (_position - leaderList[i].GetComponent<Agent>()._position).magnitude;

            if (currentDistance < nearestDistance)
            {
                nearestLeader = leaderList[i].GetComponent<Leader>();
                nearestDistance = currentDistance;
            }
        }

        return nearestLeader;
    }


    // Follow the nearest leader
    protected Vector2 steeringFollowTheLeader (Leader leader) {
        Vector2 predictedLeaderPosition = leader._position + leader._velocity;
        Vector2 positionDifference = predictedLeaderPosition - _position;

        Vector2 force = positionDifference;
        return force;
    }


    // Stay out of the leader way
    protected Vector2 steeringStayOutTheWay (Leader leader) {
        // Compute the circle area to stay out
        Vector2 leaderAreaCenter = leader._position + leader._velocity;

        // Compute the distance to this circle
        float distance = (_position - leaderAreaCenter).magnitude;

        // If the agent is in the circle, apply a steering force
        Vector2 force = (distance < leader._velocity.magnitude) ? (_position - leaderAreaCenter).normalized / distance : Vector2.zero;
        return force;
    }
}
