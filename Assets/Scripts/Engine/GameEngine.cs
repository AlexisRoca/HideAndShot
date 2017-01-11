using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

    // State Machine
    enum StateGame {
        Tuto,
        Play,
        Pause,
        Over
    }

    StateGame currentState;

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
    GameObject m_HUD;
    Cursor m_shooterPlayer;

    // Load the Game
    public void loadGame()
    {
        m_shooterPlayer = this.gameObject.AddComponent<Cursor>();
        m_HUD = GameObject.FindGameObjectWithTag("Canvas");
        createPlayers();

        m_agentEngine = this.gameObject.AddComponent<AgentEngine>();
        m_agentEngine.initAgents();

        m_zoneEngine = this.gameObject.AddComponent<ZoneEngine>();
        m_zoneEngine.initZones();

        currentState = StateGame.Play;
    }

    // Update the Game
    public void updateGame()
    {
        currentState = checkChangeGameState();
        updateGameState();
    }

    private StateGame checkChangeGameState()
    {
        switch(currentState)
        {
            case StateGame.Tuto:
            break;

            case StateGame.Play:
                if(Input.GetButtonDown("PauseButton") || Input.GetKeyDown(KeyCode.Escape))
                    return StateGame.Pause;

                if(m_shooterPlayer.m_nbShoot == 0)
                    return StateGame.Over;

                if(checkIfAllHiddenPlayersAreDead())
                    return StateGame.Over;
            break;

            case StateGame.Pause:
                if(Input.GetButtonDown("PauseButton") || Input.GetKeyDown(KeyCode.Escape))
                    return StateGame.Play;
            break;

            case StateGame.Over:
            break;
        }

        return currentState;
    }

    private void updateGameState()
    {
        switch (currentState)
        {
            case StateGame.Tuto:
                break;

            case StateGame.Play:
                float deltaTime = Time.deltaTime * _timeVariation;

                // Player
                foreach (HiddenAgent hiddenPlayer in _hiddenPlayersList)
                    hiddenPlayer.updateAgent(deltaTime);

                // Agent
                m_agentEngine.update(deltaTime);

                // Zone
                m_zoneEngine.update(m_agentEngine.m_leaderList, deltaTime);
            break;

            case StateGame.Pause:
            break;
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

        m_shooterPlayer.m_nbShoot = PlayerSelection_Persistent.nbHidePlayers * 3;
    }

    private bool checkIfAllHiddenPlayersAreDead()
    {
        foreach(HiddenAgent player in _hiddenPlayersList)
            if(!player._isDead)
                return false;

        return true;
    }

    // Game over
    public bool isOver()
    {
        return currentState == StateGame.Over;
    }
}