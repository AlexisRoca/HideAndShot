using UnityEngine;
using System.Collections;

public class AgentEngine {

    // Leader gestion
    public static void leader (Agent[] leaderList, float coefAvoidObs) {
        foreach (Agent leader in leaderList) {
            Vector2 leadSteer = AgentSteering.leader(20, 0.2f, ref leader._wanderPoint);
            Vector2 avoidSteer = AgentSteering.avoid(leader._position, leader._velocity, ref leader._wanderPoint) * coefAvoidObs;

            Vector2 force = leadSteer + avoidSteer;

            leader._steeringForce += force;
        }
    }


    // Follower gestion
    public static void follower (Agent[] followerList, float coefStayOut, float dTime) {
        foreach (Agent follower in followerList) {
            Agent nearestLeader = follower.findNearest("Leader");

            Vector2 followSteer = AgentSteering.follow(follower._position, nearestLeader._position, nearestLeader._velocity, dTime);
            Vector2 stayOutSteer = AgentSteering.stayOut(follower._position, nearestLeader._position, nearestLeader._velocity) * coefStayOut;

            Vector2 force = followSteer + stayOutSteer;

            follower._steeringForce += force;
        }
    }


    // Drunk gestion
    public static void drunk(Agent[] drunkList, float coefAvoidObs) {
        foreach (Agent drunk in drunkList) {
            Vector2 drunkSteer = AgentSteering.leader(20, 0.5f, ref drunk._wanderPoint);
            Vector2 avoidSteer = AgentSteering.avoid(drunk._position, drunk._velocity, ref drunk._wanderPoint) * coefAvoidObs;

            Vector2 force = drunkSteer + avoidSteer;

            drunk._steeringForce += force;
        }
    }


    // Player gestion
    public static void player(Agent[] playerList, float coefPlayer) {
        foreach (Agent player in playerList) {
            Vector2 steer = AgentSteering.player(player._velocity) * coefPlayer;

            player._steeringForce += steer;
        }
    }


    // Separation gestion
    public static void separation(Agent[] separationList, float coefSeparation) {
        foreach (Agent separation in separationList) {
            Agent[] neighbours = separation.findNeighbours(20.0f);
            Vector2[] positionNeighbours = new Vector2[neighbours.Length];

            for (int i = 0; i < neighbours.Length; i++) {
                positionNeighbours.SetValue(neighbours[i]._position, i);
            }

            Vector2 steer = AgentSteering.separation(separation._position, positionNeighbours) * coefSeparation;
            separation._steeringForce += steer;
        }
    }
}
