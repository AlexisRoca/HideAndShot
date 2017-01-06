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
        Menu,
        Load,
        Init,
        Play,
        Pause,
        End
    }

    stateMachine currentState;

    
    // Do not destroy this game object
    void Awake(){
        DontDestroyOnLoad(this);
    }


    // Use this for initialization
    void Start () {
        currentState = stateMachine.Menu;

        _playerSelector = this.gameObject.AddComponent<PlayerSelector>();
        _gameEngine = this.gameObject.AddComponent<GameEngine>();
    }
	

	// Update is called once per frame
	void Update () {
        switch (currentState)
        {
            case stateMachine.Menu:
                // Load Menu
                if(! loadDone) {
                    // SceneManager.LoadScene("Scene_Menu");
                    _playerSelector.initPlayerSelector();
                    loadDone = true;
                }

                // Update Menu
                _playerSelector.updatePlayerSelector();

                // If the game can start
                if (_playerSelector.isReady())
                    changeState(stateMachine.Load);

                break;


            case stateMachine.Load:
                // Load Game
                if (!loadDone) {
                    SceneManager.LoadScene("Scene_1");
                    _gameEngine.loadGame();
                    loadDone = true;
                }

                changeState(stateMachine.Init);
                break;

            case stateMachine.Init:
                changeState(stateMachine.Play);
                break;

            case stateMachine.Play:
                _gameEngine.updateGame();
                break;

            case stateMachine.Pause:
                //pause();
                break;

            case stateMachine.End:
                //_gameEngine.endGame();
                break;

            default: break;
        }
    }


    // Change state of game machine
    public void changeState(stateMachine newState) {
        currentState = newState;
        loadDone = false;
    }
}
