using UnityEngine;
using System.Collections;

public class Zone : MonoBehaviour {

    // Allow escape side
    public bool _upSide = false;
    public bool _rightSide = false;
    public bool _downSide = false;
    public bool _leftSide = false;

    // Fire time change
    private float _timer = 0.0f;
    public int _crossTime { get; set; }
    public bool _allowAgent { get; set; }

    // Dimension of the active crossing
    public Vector2 _min { get; set; }
    public Vector2 _max { get; set; }


    // Use this for initialization
    void Start () {
        // Compute dimensions
        Transform dimension = GetComponent<Transform>();
        _min = new Vector2(dimension.position.x, dimension.position.z) - new Vector2(dimension.localScale.x, dimension.localScale.z) * 5;
        _max = new Vector2(dimension.position.x, dimension.position.z) + new Vector2(dimension.localScale.x, dimension.localScale.z) * 5;

        _allowAgent = true;
        _crossTime = 10;
    }


    // Define personal crossing
    public void defineTime(int crossTime) {
        _crossTime = crossTime;
    }


    // Indicate if the agent is in the zone
    public bool isInZone (Vector2 position) {
        return position.x > _min.x
            && position.y > _min.y
            && position.x < _max.x
            && position.y < _max.y;
    }


    // Update is called once per frame
    public void updateTime (float dTime) {
        _timer += dTime;

        if (_timer > _crossTime) {
            _timer = 0.0f;
            _allowAgent = !_allowAgent;
        }

        // GetComponent<Renderer>().material.color = (_allowAgent) ? Color.green : Color.red;
    }


    // If fire is green
    public bool isGreen () {
        if (!_allowAgent)
            if (_crossTime - _timer > _crossTime / 10.0f)
                return true;
        return false;
    }

    // If fire is orange
    public bool isOrange() {
        if (!_allowAgent)
            if (_crossTime - _timer < _crossTime / 10.0f)
                return true;
        return false;
    }

    // If fire is red
    public bool isRed() {
        if (_allowAgent)
            return true;
        return false;
    }
}
