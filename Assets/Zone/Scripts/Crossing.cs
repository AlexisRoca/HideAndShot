using UnityEngine;
using System.Collections;

public class Crossing : MonoBehaviour {

    // Allow escape side
    public bool _upSide = false;
    public bool _rightSide = false;
    public bool _downSide = false;
    public bool _leftSide = false;

    // Fire time change
    public int _crossTime { get; set; }
    private float _timer = 0.0f;
    public bool _allowAgent { get; set; }

    // Dimension of the active crossing
    protected Vector2 _min = Vector2.zero;
    protected Vector2 _max = Vector2.zero;


    // Use this for initialization
    void Start () {
        // Compute dimensions
        Transform dimension = GetComponent<Transform>();
        _min = new Vector2(dimension.position.x, dimension.position.z) - new Vector2(dimension.localScale.x, dimension.localScale.z) * 5;
        _max = new Vector2(dimension.position.x, dimension.position.z) + new Vector2(dimension.localScale.x, dimension.localScale.z) * 5;

        _allowAgent = true;
        _crossTime = 5;
    }


    // Define personal crossing
    public void defineCrossing(int crossTime) {
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
    public void updateCrossing (float dTime) {
        _timer += dTime;

        if (_timer > _crossTime) {
            _timer = 0.0f;
            _allowAgent = !_allowAgent;
        }

        GetComponent<Renderer>().material.color = (_allowAgent) ? Color.green : Color.red;
    }


    // Steering force to push the agent out the zone
    public Vector2 pushOut(Vector2 agentPosition) {
        float minDistance = float.MaxValue;
        Vector2 force = Vector2.zero;

        if (_upSide) {
            float distance = (agentPosition - new Vector2(agentPosition.x, _max.y)).magnitude;
            if (distance < minDistance) {
                minDistance = distance;
                force = new Vector2(0.0f, 1.0f);
            }
        }

        if (_rightSide) {
            float distance = (agentPosition - new Vector2(_max.x, agentPosition.y)).magnitude;
            if (distance < minDistance) {
                minDistance = distance;
                force = new Vector2(1.0f, 0.0f);
            }
        }

        if (_downSide) {
            float distance = (agentPosition - new Vector2(agentPosition.x, _min.y)).magnitude;
            if (distance < minDistance) {
                minDistance = distance;
                force = new Vector2(0.0f, -1.0f);
            }
        }

        if (_leftSide) {
            float distance = (agentPosition - new Vector2(_min.x, agentPosition.y)).magnitude;
            if (distance < minDistance) {
                minDistance = distance;
                force = new Vector2(-1.0f, 0.0f);
            }
        }

        return force;
    }


    // Steering force to push the agent trough the zone
    public Vector2 pushThrough(Vector2 agentVelocity) {
        float minAngle = float.MaxValue;
        Vector2 force = Vector2.zero;

        if (_upSide) {
            Vector2 direction = new Vector2(0.0f, 1.0f);
            float angle = Mathf.Acos(Vector2.Dot(agentVelocity.normalized, direction));
            if (angle < minAngle) {
                minAngle = angle;
                force = direction;
            }
        }

        if (_rightSide) {
            Vector2 direction = new Vector2(1.0f, 0.0f);
            float angle = Mathf.Acos(Vector2.Dot(agentVelocity.normalized, direction));
            if (angle < minAngle) {
                minAngle = angle;
                force = direction;
            }
        }

        if (_downSide) {
            Vector2 direction = new Vector2(0.0f, -1.0f);
            float angle = Mathf.Acos(Vector2.Dot(agentVelocity.normalized, direction));
            if (angle < minAngle) {
                minAngle = angle;
                force = direction;
            }
        }

        if (_leftSide) {
            Vector2 direction = new Vector2(-1.0f, 0.0f);
            float angle = Mathf.Acos(Vector2.Dot(agentVelocity.normalized, direction));
            if (angle < minAngle) {
                minAngle = angle;
                force = direction;
            }
        }

        return force;
    }
}
