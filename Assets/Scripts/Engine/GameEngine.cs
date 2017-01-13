using UnityEngine;
using UnityEngine.UI;
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
    Text m_bulletsCounterText;
    Text m_aliveHiddenPlayerCounterText;
    Cursor m_shooterPlayer;

    int m_nbHiddenPlayersAlive;

    // Load the Game
    public void loadGame()
    {
        initHUD();
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

                // HUD
                m_bulletsCounterText.text = "NB Bullets: " + m_shooterPlayer.m_nbShoot;
                m_aliveHiddenPlayerCounterText.text = "NB Hidden Player Alive: " + m_nbHiddenPlayersAlive;
            break;

            case StateGame.Pause:
            break;
        }
    }


    // Define the agent properties
    void createPlayers()
    {
        GameObject[] playerListGO = GameObject.FindGameObjectsWithTag("PlayerHide");
        m_nbHiddenPlayersAlive = PlayerSelection_Persistent.nbHidePlayers;
        _hiddenPlayersList = new HiddenAgent[m_nbHiddenPlayersAlive];

        for(int i=0; i<m_nbHiddenPlayersAlive; i++)
        {
            HiddenAgent hiddenAgent = playerListGO[i].GetComponent<HiddenAgent>();
            hiddenAgent.defineAgent(_playerMass,_playerSpeed,_playerSteer,Random.Range(0.0f,360.0f));

            if(PlayerSelection_Persistent.keyboardControl)
                hiddenAgent.m_controller = new KeyboardController();
            else
                hiddenAgent.m_controller = new GamepadController(PlayerSelection_Persistent.hiddenPlayerGamepadID[i]);

            _hiddenPlayersList[i] = hiddenAgent;
        }

        for(int i = m_nbHiddenPlayersAlive; i < 3; i++)
            playerListGO[i].gameObject.SetActive(false);

        m_shooterPlayer.m_nbShoot = m_nbHiddenPlayersAlive * 3;
    }

    void initHUD()
    {
        m_shooterPlayer = this.gameObject.AddComponent<Cursor>();
        m_HUD = GameObject.FindGameObjectWithTag("Canvas");

        m_bulletsCounterText = GameObject.Find("ShooterBulletsCounter").GetComponent<Text>();
        m_bulletsCounterText.text = "NB Bullets: " + m_shooterPlayer.m_nbShoot;

        m_aliveHiddenPlayerCounterText = GameObject.Find("AlivePlayersCounter").GetComponent<Text>();
        m_aliveHiddenPlayerCounterText.text = "NB Hidden Player Alive: " + m_nbHiddenPlayersAlive;
    }

    private bool checkIfAllHiddenPlayersAreDead()
    {
        return (m_nbHiddenPlayersAlive == 0);
    }

    // Game over
    public bool isOver()
    {
        return currentState == StateGame.Over;
    }
}