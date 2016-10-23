using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

    // Define Generic attributes
    public float _timeVariation = 1.0f;

    public float _coefAvoidObs = 50.0f;
    public float _coefStayOut = 500.0f;
    public float _coefPlayer = 50.0f;
    public float _coefSeparation = 100.0f;
    public float _coefCrossing = 100.0f;

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
        defineZones();
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
        AgentEngine.leader(_leaderList, _coefAvoidObs);

        // Follower
        AgentEngine.follower(_followerList, _coefStayOut, dTime);

        // Drunk
        AgentEngine.drunk(_drunkList, _coefAvoidObs);

        // Player
        AgentEngine.player(_playerList, _coefPlayer);

        // Separation
        AgentEngine.separation(_followerList, _coefSeparation);
    }


    // Update all behavior in the scene
    void updateZoneInteraction(float dTime) {
        // Croosing
        ZoneEngine.crossing(_crossingList, _leaderList, _coefCrossing);
    }


    // Update the agent in the game world
    void updateGameObject(float dTime) {
        // All agent
        foreach (Agent agent in _agentList) {
            agent.updateAgent(dTime);

            agent.transform.rotation = Quaternion.Euler(0.0f, agent._orientation, 0.0f);
            agent.transform.position = new Vector3(agent._position.x, agent.transform.position.y, agent._position.y);

            if (agent.GetComponent<Animator>() != null) agent.GetComponent<Animator>().Play("Take 001");
        }

        // All crossing
        foreach (Zone crossing in _crossingList) {
            crossing.updateTime(dTime);
        }
    }


    // Define the agent properties
    void defineAgents() {
        // Leaders
        foreach (Agent leader in _leaderList) {
            leader.defineAgent(10, 30, 100, Random.Range(0.0f, 360.0f), Random.Range(0.0f, 2*Mathf.PI));
        }

        // Follower
        foreach (Agent follower in _followerList) {
            follower.defineAgent(10, 30, 100, Random.Range(0.0f, 360.0f));
        }

        // Drunk
        foreach (Agent drunk in _drunkList) {
            drunk.defineAgent(10, 10, 30, Random.Range(0.0f, 360.0f), Random.Range(0.0f, 2 * Mathf.PI));
        }

        // Player
        foreach (Agent player in _playerList) {
            player.defineAgent(10, 30, 100, Random.Range(0.0f, 360.0f));
        }
    }


    // Define the zone properties
    void defineZones() {
        // Crossing
        foreach (Zone crossing in _crossingList) {
            crossing.defineTime(20);
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
        _crossingList= new Zone[crossingListGO.Length];
        for (int i = 0; i < crossingListGO.Length; i++) {
            _crossingList.SetValue(crossingListGO[i].GetComponent<Zone>(), i);
        }
    }
}
