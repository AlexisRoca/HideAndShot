using UnityEngine;
using System.Collections;

public class AgentEngine {
    
    // Define Agent lists
    public Agent[] _agentList;
    public Agent[] _leaderList;
    public Agent[] _followerList;
    public Agent[] _drunkList;
    public Agent[] _playerList;

    // Define Agent attributes
    int _leaderMass = 50;
    int _leaderSpeed = 50;
    int _leaderSteer = 500;
    int _leaderRadius = 10;
    float _leaderVariation = 0.001f;

    int _followerMass = 50;
    int _followerSpeed = 40;
    int _followerSteer = 100;

    int _drunkMass = 100;
    int _drunkSpeed = 10;
    int _drunkSteer = 30;

    int _playerMass = 50;
    int _playerSpeed = 40;
    int _playerSteer = 500;

    // Define steering coefficient
    float _coefLeader = 1.0f;
    float _coefAvoidObs = 200.0f;
    float _coefStayOut = 500.0f;
    float _coefPlayer = 100.0f;
    float _coefSeparation = 200.0f;


    // Constructor
    public AgentEngine () {
        // Collect all agent in scene
        collectGameObjects();
    }


    // Define the agent properties
    public void defineAgents() {
        // Leaders
        foreach (Agent leader in _leaderList) {
            leader.defineAgent(_leaderMass, _leaderSpeed, _leaderSteer, Random.Range(0.0f, 360.0f), Random.Range(0.0f, 2 * Mathf.PI));
        }

        // Follower
        foreach (Agent follower in _followerList) {
            follower.defineAgent(_followerMass, _followerSpeed, _followerSteer, Random.Range(0.0f, 360.0f));
        }

        // Drunk
        foreach (Agent drunk in _drunkList) {
            drunk.defineAgent(_drunkMass, _drunkSpeed, _drunkSteer, Random.Range(0.0f, 360.0f), Random.Range(0.0f, 2 * Mathf.PI));
        }

        // Player
        foreach (Agent player in _playerList) {
            player.defineAgent(_playerMass, _playerSpeed, _playerSteer, Random.Range(0.0f, 360.0f));
        }
    }


    // Update all behavior in the scene
    public void updateAgentInteraction(float dTime) {
        // Leaders
        leader();

        // Follower
        follower();

        // Drunk
        drunk();

        // Player
        player();

        // Separation
        separation();
    }


    // Collect all game objects in the scene
    void collectGameObjects() {
        // Define Game Objects lists
        GameObject[] leaderListGO = GameObject.FindGameObjectsWithTag("Leader");
        GameObject[] followerListGO = GameObject.FindGameObjectsWithTag("Follower");
        GameObject[] drunkListGO = GameObject.FindGameObjectsWithTag("Drunk");
        GameObject[] playerListGO = GameObject.FindGameObjectsWithTag("PlayerHide");

        // Define Agent lists
        _agentList = GameObject.FindObjectsOfType<Agent>();

        // Leaders
        _leaderList = new Agent[leaderListGO.Length];
        for (int i = 0; i < leaderListGO.Length; i++) {
            _leaderList.SetValue(leaderListGO[i].GetComponent<Agent>(), i);
        }

        // Follower
        _followerList = new Agent[followerListGO.Length];
        for (int i = 0; i < followerListGO.Length; i++) {
            _followerList.SetValue(followerListGO[i].GetComponent<Agent>(), i);
        }

        // Drunk
        _drunkList = new Agent[drunkListGO.Length];
        for (int i = 0; i < drunkListGO.Length; i++) {
            _drunkList.SetValue(drunkListGO[i].GetComponent<Agent>(), i);
        }

        // Player
        _playerList = new Agent[playerListGO.Length];
        for (int i = 0; i < playerListGO.Length; i++) {
            _playerList.SetValue(playerListGO[i].GetComponent<Agent>(), i);
        }
    }


    // Leader gestion
    void leader () {
        foreach (Agent leader in _leaderList) {
            Vector2 leadSteer = AgentSteering.leader(leader._velocity, _leaderRadius, _leaderVariation, ref leader._wanderPoint) * _coefLeader;
            Vector2 avoidSteer = AgentSteering.avoid(leader._position, leader._velocity, ref leader._wanderPoint) * _coefAvoidObs;

            Vector2 force = (avoidSteer == Vector2.zero) ? leadSteer : avoidSteer;

            leader._steeringForce += force;

            //Vector3 start = new Vector3(leader._position.x, 0.0f, leader._position.y);
            //Vector3 end = start + new Vector3(force.x, 0.0f, force.y);
            //Debug.DrawLine(start, end);
        }
    }


    // Follower gestion
    void follower () {
        foreach (Agent follower in _followerList) {
            Agent nearestLeader = follower.findNearest(_leaderList);

            Vector2 followSteer = AgentSteering.follow(follower._position, nearestLeader._position, nearestLeader._velocity);

            Vector2 stayOutSteer = Vector2.zero;
            foreach (Agent leader in _leaderList) {
                stayOutSteer += AgentSteering.stayOut(follower._position, leader._position, leader._velocity) * _coefStayOut;
            }

            Vector2 force = followSteer + stayOutSteer;

            follower._steeringForce += force;
        }
    }


    // Drunk gestion
    void drunk() {
        foreach (Agent drunk in _drunkList) {
            Vector2 drunkSteer = AgentSteering.leader(drunk._velocity, 20, 0.5f, ref drunk._wanderPoint);
            Vector2 avoidSteer = AgentSteering.avoid(drunk._position, drunk._velocity, ref drunk._wanderPoint) * _coefAvoidObs;

            Vector2 force = drunkSteer + avoidSteer;

            drunk._steeringForce += force;
        }
    }


    // Player gestion
    void player() {
        foreach (Agent player in _playerList) {
            Vector2 steer = AgentSteering.player(player._velocity) * _coefPlayer;

            player._steeringForce += steer;
        }
    }


    // Separation gestion
    void separation() {
        foreach (Agent separation in _agentList) {
            Agent[] neighbours = separation.findNeighbours(_agentList, 20.0f);
            Vector2[] positionNeighbours = new Vector2[neighbours.Length];

            for (int i = 0; i < neighbours.Length; i++) {
                positionNeighbours.SetValue(neighbours[i]._position, i);
            }

            Vector2 steer = AgentSteering.separation(separation._position, positionNeighbours) * _coefSeparation;
            separation._steeringForce += steer;
        }
    }
}
