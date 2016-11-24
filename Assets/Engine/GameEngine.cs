using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

    // Define Generic attributes
    public float _timeVariation = 1.0f;

    public int _playerMass = 0;
    public int _playerSpeed = 0;
    public int _playerSteer = 0;

    AgentEngine m_agentEngine;

    // Define Agent lists
    Agent[] _playerList;

    // Define Zone lists
    Zone[] _crossingList;
    GameObject[] _fireList;

    // Use this for initialization
    void Start()
    {
        m_agentEngine = this.GetComponent<AgentEngine>();
        m_agentEngine.initAgents();

        collectGameObjects();
        defineZones();
    }


    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime * _timeVariation;
   
        m_agentEngine.update(deltaTime);

        // DEBUG : Update Agent behavior
        //defineAgents();

        //updateAgentInteraction(deltaTime);
        //updateZoneInteraction(deltaTime);
        //updateGameObject(deltaTime);
    }


    // Update all behavior in the scene
    void updateAgentInteraction(float dTime)
    {
        //// Player
        //AgentEngine.player(_playerList, _coefPlayer);
    }


    // Update all behavior in the scene
    void updateZoneInteraction(float dTime)
    {
        //// Crossing
        //ZoneEngine.crossing(_crossingList, _leaderList, _coefCrossing);
        //ZoneEngine.crossing(_crossingList, _followerList, _coefCrossing);
    }


    // Update the agent in the game world
    void updateGameObject(float dTime)
    {
        // All crossing
        //for(int i=0; i < _crossingList.Length; i++)
        //{
        //    _crossingList[i].updateTime(dTime);
        //    ZoneEngine.fireLight(_crossingList[i], _fireList[i]);
        //}
    }


    // Define the agent properties
    void defineAgents()
    {
        // Player
        foreach(Agent player in _playerList)
        {
            player.defineAgent(_playerMass, _playerSpeed, _playerSteer, Random.Range(0.0f, 360.0f));
        }
    }


    // Define the zone properties
    void defineZones()
    {
        // Crossing
        foreach(Zone crossing in _crossingList)
        {
            crossing.defineTime(20);
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

        // Define Zone lists
        _fireList = GameObject.FindGameObjectsWithTag("Fire");
        GameObject[] crossingListGO = GameObject.FindGameObjectsWithTag("Cross");
        
        // Crossing
        _crossingList = new Zone[crossingListGO.Length];
        for(int i=0; i < crossingListGO.Length; i++)
        {
            _crossingList.SetValue(crossingListGO[i].GetComponent<Zone>(), i);
        }
    }
}
