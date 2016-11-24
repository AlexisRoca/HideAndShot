using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

    // Attributes deffinition (get/set)
    // Personal attributes
    public int _mass { get; set; }
    public int _maxSpeed { get; set; }
    public int _maxSteer { get; set; }
    public float _wanderPoint = 0.0f;

    // Displacement attributes
    public float _orientation { get; set; }
    public Vector2 _position { get; set; }
    public Vector2 _velocity { get; set; }

    // Steering force memory (RAZ at each update)
    public Vector2 _steeringForce { get; set; }


    // Use this for initialization
    void Awake()
    {
        _mass = 50;
        _maxSpeed = 30;
        _maxSteer = 1000;

        _orientation = 0.0f;
        _position = new Vector2(transform.position.x, transform.position.z);
        _velocity = Vector2.zero;

        _steeringForce = Vector2.zero;
    }


    // Define personal agent
    public void defineAgent(int mass, int maxSpeed, int maxSteer, float orientation, float wander = 0.0f) {
        _mass = mass;
        _maxSpeed = maxSpeed;
        _maxSteer = maxSteer;
        _orientation = orientation;

        _wanderPoint = wander;
    }


    // Update the agent values
    public void updateAgent (float dTime) {
        // Collect the world position in case of collision
        _position = new Vector2(transform.position.x, transform.position.z);
        // _orientation = transform.rotation.eulerAngles.y;
        
        // Truncate steering forces by the max limit
        Vector2 steer = Vector2.ClampMagnitude(_steeringForce, _maxSteer);

        // Compute acceleration
        Vector2 acceleration = steer / _mass;

        // Compute new velocity
        _velocity += acceleration;

        // Apply new velocity under a max speed
        _velocity = Vector2.ClampMagnitude(_velocity, _maxSpeed);
        
        // Compute new orientation
        _orientation = Vector2.Angle(new Vector2(0.0f, 1.0f), _velocity);

        // Compute new position
        _position += _velocity * dTime;

        // RAZ Steering force for this delta time
        _steeringForce = Vector2.zero;
    }


    // Find the nearest agent taged T
    public Agent findNearest (Agent[] agentList) {
        // Define distance and GameObject for the nearest agent
        Agent nearestAgent = null;
        float nearestDistance = float.MaxValue;

        // Find the nearest to follow
        foreach (Agent currentAgent in agentList) {
            float currentDistance = (_position - currentAgent._position).magnitude;

            if (currentDistance < nearestDistance) {
                nearestAgent = currentAgent;
                nearestDistance = currentDistance;
            }
        }

        return nearestAgent;
    }


    // Find neighbours agent
    public Agent[] findNeighbours(Agent[] agentList, float distance) {
        // Get all Agent taged T in the scene
        bool[] inNeighbourhood = new bool[agentList.Length];

        int nbNeighbours = 0;

        // Find the number of contributors
        for (int i=0; i<agentList.Length; i++) {
            if (agentList[i].Equals(this)) continue;

            float currentDistance = (_position - agentList[i]._position).magnitude;

            if (currentDistance < distance) {
                nbNeighbours++;
                inNeighbourhood[i] = true;
            } else {
                inNeighbourhood[i] = false;
            }
        }

        Agent[] neighbours = new Agent[nbNeighbours];
        int it = 0;

        // Create the neighbour list
        for (int i=0; i<agentList.Length; i++) {
            if (inNeighbourhood[i]) {
                neighbours.SetValue(agentList[i], it);
                it++;
            }
        }

        return neighbours;
    }
}
