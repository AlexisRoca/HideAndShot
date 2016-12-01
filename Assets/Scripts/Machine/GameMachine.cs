using UnityEngine;
using System.Collections;

public class GameMachine : MonoBehaviour {

    // Define Game attributes
    public int _nbBullet = 5;
    public int _timeGameSecond = 90;

    // Define Game Engine
    GameEngine _gameEngine;

    // State Machine
    enum stateMachine
    {
        Menu,
        Load,
        Play,
        Pause,
        End
    }

    stateMachine currentState;


    // Use this for initialization
    void Start () {
        _gameEngine = this.GetComponent<GameEngine>();

        currentState = stateMachine.Menu;
    }
	

	// Update is called once per frame
	void Update () {
        switch (currentState)
        {
            case stateMachine.Menu:
                // menu();
                currentState = stateMachine.Load;
                break;

            case stateMachine.Load:
                _gameEngine.loadGame();
                currentState = stateMachine.Play;
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
}
