using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

    // Define Generic attributes
    public float _timeVariation = 1.0f;

    // Define Game Engines
    AgentEngine _agentEngine;
    ZoneEngine _zoneEngine;


    // Use this for initialization
    void Start () {
        _agentEngine = new AgentEngine();
        _zoneEngine = new ZoneEngine();
        
        _agentEngine.defineAgents();
        _zoneEngine.defineZones();
    }


    // Update is called once per frame
    void Update () {
        float dTime = Time.deltaTime * _timeVariation;

        // Create Agent list with Leader and Follower
        Agent[] agentCrossList = new Agent[_agentEngine._leaderList.Length + _agentEngine._followerList.Length];
        _agentEngine._leaderList.CopyTo(agentCrossList, 0);
        _agentEngine._followerList.CopyTo(agentCrossList, _agentEngine._leaderList.Length);

        _agentEngine.updateAgentInteraction(dTime);
        _zoneEngine.updateZoneInteraction(agentCrossList, dTime);

        // Refresh Game Object
        updateGameObject(dTime);
    }

    
    // Update the agent in the game world
    void updateGameObject(float dTime) {
        // All agent
        foreach (Agent agent in _agentEngine._agentList) {
            agent.updateAgent(dTime);

            agent.transform.rotation = Quaternion.Euler(0.0f, agent._orientation, 0.0f);
            agent.transform.position = new Vector3(agent._position.x, agent.transform.position.y, agent._position.y);
        }

        // All crossing
        for (int i = 0; i < _zoneEngine._crossingList.Length; i++) {
            _zoneEngine._crossingList[i].updateTime(dTime);
            _zoneEngine.fireLight(_zoneEngine._crossingList[i], _zoneEngine._fireList[i]);
        }
    }


   
}
