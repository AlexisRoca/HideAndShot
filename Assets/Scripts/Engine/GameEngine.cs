using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

    // State Machine
    enum stateGame {
        Tuto,
        Play,
        Pause,
        End
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
    HiddenAgent [] _hiddenPlayersList;


    // Load the Game
    public void loadGame()
    {
        createPlayers();

        m_agentEngine = this.gameObject.AddComponent<AgentEngine>();
        m_agentEngine.initAgents();

        m_zoneEngine = this.gameObject.AddComponent<ZoneEngine>();
        m_zoneEngine.initZones();

        currentState = stateGame.Play;
    }


    // Update the Game
    public void updateGame()
    {
        switch (currentState) {
            case stateGame.Tuto:
                break;

            case stateGame.Play:
                if(Input.GetButtonDown("PauseButton") || Input.GetKeyDown(KeyCode.Escape))
                    currentState = stateGame.Pause;

                float deltaTime = Time.deltaTime * _timeVariation;

                // Player
                foreach (HiddenAgent hiddenPlayer in _hiddenPlayersList)
                    hiddenPlayer.updateAgent(deltaTime);

                // Agent
                m_agentEngine.update(deltaTime);
                // Zone
                m_zoneEngine.update(m_agentEngine.m_leaderList, deltaTime);
                break;

            case stateGame.Pause:
                if(Input.GetButtonDown("PauseButton") || Input.GetKeyDown(KeyCode.Escape))
                    currentState = stateGame.Play;

                break;
        }

        // Check if all players are dead
        foreach(HiddenAgent player in _hiddenPlayersList)
        {
            if (!player._isDead)
                break;

            currentState = stateGame.End;
        }
    }


    // Define the agent properties
    void createPlayers()
    {
        GameObject[] playerListGO = GameObject.FindGameObjectsWithTag("PlayerHide");
        _hiddenPlayersList = new HiddenAgent[PlayerSelection_Persistent.nbHidePlayers];

        for(int i=0; i<PlayerSelection_Persistent.nbHidePlayers; i++)
        {
            HiddenAgent hiddenAgent = playerListGO[i].GetComponent<HiddenAgent>();
            hiddenAgent.defineAgent(_playerMass,_playerSpeed,_playerSteer,Random.Range(0.0f,360.0f));

            if(PlayerSelection_Persistent.keyboardControl)
                hiddenAgent.m_controller = new KeyboardController();
            else
                hiddenAgent.m_controller = new GamepadController(PlayerSelection_Persistent.hiddenPlayerGamepadID[i]);

            _hiddenPlayersList[i] = hiddenAgent;
        }

        for(int i = PlayerSelection_Persistent.nbHidePlayers; i < 3; i++)
            playerListGO[i].gameObject.SetActive(false);
    }


    // Game over
    public bool isOver()
    {
        return currentState == stateGame.End;
    }
}
