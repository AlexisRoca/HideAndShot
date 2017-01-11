using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMachine : MonoBehaviour {

    // Define Game attributes
    public int _nbBullet = 5;
    public int _timeGameSecond = 90;

    bool loadDone = false;

    // Define Game Engine
    PlayerSelector _playerSelector;
    GameEngine _gameEngine;

    // State Machine
    public enum stateMachine
    {
        playerLobby,
        LoadMainScene,
        InitMainScene,
        Play,
        End
    }

    stateMachine currentState;
    AsyncOperation sceneLoading;

    // Do not destroy this game object
    void Awake(){
        DontDestroyOnLoad(this);
    }


    // Use this for initialization
    void Start () {
        currentState = stateMachine.playerLobby;

        _playerSelector = this.gameObject.GetComponent<PlayerSelector>();
        _playerSelector.initPlayerSelector();

        _gameEngine = this.gameObject.AddComponent<GameEngine>();
    }
	

	// Update is called once per frame
	void Update ()
    {
        currentState = checkChangeState();
        updateState();
    }


    public void updateState()
    {
        switch(currentState)
        {
            case stateMachine.playerLobby:
                _playerSelector.updatePlayerSelector();
            break;


            case stateMachine.LoadMainScene:
            break;

            case stateMachine.InitMainScene:
            break;

            case stateMachine.Play:
                _gameEngine.updateGame();
            break;

            case stateMachine.End:
            //_gameEngine.endGame();
            break;
        }
    }

    // Change state of game machine
    public stateMachine checkChangeState() {
        switch(currentState)
        {
            case stateMachine.playerLobby:
                // If the game can start
                if(_playerSelector.isReady())
                {
                    sceneLoading = SceneManager.LoadSceneAsync("Scene_1");
                    return stateMachine.LoadMainScene;
                }
            break;


            case stateMachine.LoadMainScene:
                // Load Game
                if(sceneLoading.isDone)
                {
                    _gameEngine.loadGame();
                    return stateMachine.InitMainScene;
                }
            break;

            case stateMachine.InitMainScene:
                return stateMachine.Play;
            break;

            case stateMachine.Play:
            break;

            case stateMachine.End:
            //_gameEngine.endGame();
            break;
        }

        return currentState;
    }
}
