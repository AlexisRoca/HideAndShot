using UnityEngine;
using System.Collections;

public class Crossing : MonoBehaviour {

    public int _crossTime = 0;

    private float _timer = 0.0f;
    public bool _allowAgent { get; private set; }

    // Dimension of the active crossing
    private Vector2 _min = Vector2.zero;
    private Vector2 _max = Vector2.zero;

    // Use this for initialization
    void Start () {
        _allowAgent = true;

        // Compute dimensions
        Transform dimension = GetComponent<Transform>();
        _min = new Vector2(dimension.position.x, dimension.position.z) - new Vector2(dimension.localScale.x, dimension.localScale.z) * 5;
        _max = new Vector2(dimension.position.x, dimension.position.z) + new Vector2(dimension.localScale.x, dimension.localScale.z) * 5;
    }
	
	// Update is called once per frame
	void Update () {
        _timer += Time.deltaTime;

        if (_timer > _crossTime) {
            _timer = 0.0f;
            _allowAgent = !_allowAgent;
        }

        if (! _allowAgent) {
            agentCross();
            GetComponent<Renderer>().material.color = Color.red;
        } else {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }


    // React with agent collision
    void agentCross () {
        // Get all Agent in the scene
        Agent[] agentList = GameObject.FindObjectsOfType<Agent>();

        // For each agent
        for (int i = 0; i < agentList.Length; i++) {
            if (agentList[i]._position.x > _min.x && agentList[i]._position.y > _min.y && agentList[i]._position.x < _max.x && agentList[i]._position.y < _max.y) {
                agentList[i]._velocity = Vector2.zero;
            }
        }
    }
}
