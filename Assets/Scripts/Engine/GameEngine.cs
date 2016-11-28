using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

    // Define Generic attributes
    public float _timeVariation = 1.0f;

    public float _coefPlayer = 1.0f;

    public int _playerMass = 10;
    public int _playerSpeed = 50;
    public int _playerSteer = 100;

    AgentEngine m_agentEngine;
    ZoneEngine m_zoneEngine;

    // Define Player lists
    Agent[] _playerList;


    // Load the Game
    public void loadGame()
    {
        collectGameObjects();
        definePlayers();

        m_agentEngine = this.GetComponent<AgentEngine>();
        m_agentEngine.initAgents();

        m_zoneEngine = this.GetComponent<ZoneEngine>();
        m_zoneEngine.initZones();
    }

    // Update the Game
    public void updateGame()
    {
        float deltaTime = Time.deltaTime * _timeVariation;

        // Player
        updatePlayers();

        // Agent
        m_agentEngine.update(deltaTime);

        // Zone
        //int agentListSize = m_agentEngine.m_leaderList.Length + m_agentEngine.m_followerList.Length;
        //Agent[] agentFeelCrossing = new Agent[agentListSize];
        //agentFeelCrossing.SetValue(m_agentEngine.m_leaderList, 0);
        //agentFeelCrossing.SetValue(m_agentEngine.m_followerList, m_agentEngine.m_leaderList.Length);
        Agent[] agentFeelCrossing = m_agentEngine.m_leaderList;

        m_zoneEngine.update(agentFeelCrossing, deltaTime);
    }



    // Define the agent properties
    void definePlayers()
    {
        // Player
        foreach(Agent player in _playerList)
            player.defineAgent(_playerMass, _playerSpeed, _playerSteer, Random.Range(0.0f, 360.0f));
    }


    // Player gestion
    void updatePlayers()
    {
        foreach (Agent player in _playerList)
        {
            Vector2 playSteer = AgentSteering.player(player._velocity);

            player._steeringForce += playSteer;
        }
    }


    // Collect all game objects in the scene
    void collectGameObjects ()
    {
        // Define Game Objects lists
        GameObject[] playerListGO = GameObject.FindGameObjectsWithTag("PlayerHide");

        // Player
        _playerList = new Agent[playerListGO.Length];
        for(int i=0; i < playerListGO.Length; i++)
        {
            _playerList.SetValue(playerListGO[i].GetComponent<Agent>(), i);
        }
    }
}
