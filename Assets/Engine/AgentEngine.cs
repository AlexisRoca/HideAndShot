using UnityEngine;
using System.Collections;

public class AgentEngine : MonoBehaviour {

    // Define Generic attributes
    float _timeVariation = 1.0f;

    // Define Agent lists
    Agent[] _agentList;
    Agent[] _leaderList;
    Agent[] _followerList;
    Agent[] _drunkList;
    Agent[] _playerList;

    // Define Zone lists
    Zone[] _crossingList;


    // Use this for initialization
    void Start () {
        collectGameObjects();
        defineAgents();
    }


    // Update is called once per frame
    void Update () {
        float dTime = Time.deltaTime * _timeVariation;
        updateAgentInteraction(dTime);
        updateZoneInteraction(dTime);
        updateGameObject(dTime);
    }


    // Update all behavior in the scene
    void updateAgentInteraction(float dTime) {
        // Leaders
        foreach (Agent leader in _leaderList) {
            Vector2 leadSteer = SteeringBehavior.leader(20, 0.5f, ref leader._wanderPoint);
            Vector2 avoidSteer = SteeringBehavior.avoid(leader._position, leader._velocity);

            Vector2 force = leadSteer + avoidSteer * 50.0f;

            leader._steeringForce += force;
        }

        // Follower
        foreach (Agent follower in _followerList) {
            Agent nearestLeader = follower.findNearest("Leader");

            Vector2 followSteer = SteeringBehavior.follow(follower._position, nearestLeader._position, nearestLeader._velocity, dTime);
            Vector2 stayOutSteer = SteeringBehavior.stayOut(follower._position, nearestLeader._position, nearestLeader._velocity);

            Vector2 force = followSteer + stayOutSteer * 50.0f;

            follower._steeringForce += force;
        }

        // Drunk
        foreach (Agent drunk in _drunkList) {
            Vector2 drunkSteer = SteeringBehavior.leader(20, 0.5f, ref drunk._wanderPoint);

            drunk._steeringForce += drunkSteer;
        }

        // Player
        foreach (Agent player in _playerList) {
            Vector2 steer = SteeringBehavior.player(player._velocity) * 50.0f;

            player._steeringForce += steer;
        }


        // All Agent
        foreach (Agent agent in _agentList) {
            Agent[] neighbours = agent.findNeighbours(20.0f);
            Vector2[] positionNeighbours = new Vector2[neighbours.Length];

            for (int i=0; i<neighbours.Length; i++) {
                positionNeighbours.SetValue(neighbours[i]._position, i);
            }

            Vector2 steer = SteeringBehavior.separation(agent._position, positionNeighbours);
            agent._steeringForce += steer*100.0f;
        }
    }


    // Update all behavior in the scene
    void updateZoneInteraction(float dTime) {
        // Croosing
        foreach (Crossing crossing in _crossingList) {
            foreach (Agent agent in _agentList) {
                if (crossing.isInZone(agent._position)) {
                    agent._steeringForce += (crossing._allowAgent) ? crossing.pushThrough(agent._velocity) : crossing.pushOut(agent._position);
                }
            }
        }
    }


    // Update the agent in the game world
    void updateGameObject(float dTime) {
        // All agent
        foreach (Agent agent in _agentList) {
            agent.updateAgent(dTime);

            agent.transform.rotation = Quaternion.AngleAxis(agent._orientation, new Vector3(0, 1, 0));
            agent.transform.position = new Vector3(agent._position.x, agent.transform.position.y, agent._position.y);
        }

        foreach (Crossing crossing in _crossingList) {
            crossing.updateCrossing(dTime);
        }
    }


    // Define the agent properties
    void defineAgents() {
        // Leaders
        foreach (Agent leader in _leaderList) {
            leader.defineAgent(10, 30, 1000, Random.Range(0.0f, 360.0f), Random.Range(0.0f, 2*Mathf.PI));
        }

        // Follower
        foreach (Agent follower in _followerList) {
            follower.defineAgent(10, 30, 1000, Random.Range(0.0f, 360.0f));
        }

        // Drunk
        foreach (Agent drunk in _drunkList) {
            drunk.defineAgent(10, 15, 1000, Random.Range(0.0f, 360.0f), Random.Range(0.0f, 2 * Mathf.PI));
        }

        // Player
        foreach (Agent player in _playerList) {
            player.defineAgent(10, 30, 1000, Random.Range(0.0f, 360.0f));
        }
    }


    // Collect all game objects in the scene
    void collectGameObjects () {
        // Define Game Objects lists
        GameObject[] leaderListGO = GameObject.FindGameObjectsWithTag("Leader");
        GameObject[] followerListGO = GameObject.FindGameObjectsWithTag("Follower");
        GameObject[] drunkListGO = GameObject.FindGameObjectsWithTag("Drunk");
        GameObject[] playerListGO = GameObject.FindGameObjectsWithTag("PlayerHide");

        // Define Agent lists
        _agentList = GameObject.FindObjectsOfType<Agent>();
        
        // Leaders
        _leaderList = new Agent[leaderListGO.Length];
        for (int i=0; i<leaderListGO.Length; i++) {
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


        // Define Zone lists
        GameObject[] crossingListGO = GameObject.FindGameObjectsWithTag("Cross");
        
        // Crossing
        _crossingList= new Crossing[crossingListGO.Length];
        for (int i = 0; i < crossingListGO.Length; i++) {
            _crossingList.SetValue(crossingListGO[i].GetComponent<Zone>(), i);
        }
    }
}
