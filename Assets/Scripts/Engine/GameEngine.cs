using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

    // State Machine
    enum stateGame {
        Tuto,
        Play,
        Pause
    }

    stateGame currentState;

    // Define Generic attributes
    public float _timeVariation = 1.0f;

    public float _coefPlayer = 100.0f;

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
        currentState = stateGame.Pause;

        collectGameObjects();
        definePlayers();

        m_agentEngine = this.gameObject.AddComponent<AgentEngine>();
        m_agentEngine.initAgents();

        m_zoneEngine = this.gameObject.AddComponent<ZoneEngine>();
        m_zoneEngine.initZones();
    }

    // Update the Game
    public void updateGame()
    {
        switch (currentState) {
            case stateGame.Tuto:
                break;

            case stateGame.Play:
                if (Input.GetButtonDown("PauseButton"))
                    currentState = stateGame.Pause;

                float deltaTime = Time.deltaTime * _timeVariation;

                // Player
                updatePlayers();
                // Agent
                m_agentEngine.update(deltaTime);
                // Zone
                m_zoneEngine.update(m_agentEngine.m_leaderList, deltaTime);
                break;

            case stateGame.Pause:
                if (Input.GetButtonDown("PauseButton"))
                    currentState = stateGame.Play;

                break;
        }
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

            player._steeringForce += playSteer * _coefPlayer;
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
