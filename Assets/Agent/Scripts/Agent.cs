using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

    public float livingSpaceRadius = 0.0f;
    public float coefSeparation = 1.0f;

    // Attributes deffinition (get/set)
    public int _mass { get; protected set; }
    public int _maxSpeed { get; protected set; }
    public int _maxSteer { get; protected set; }

    public float _orientation { get; protected set; }
    public Vector2 _position { get; protected set; }
    public Vector2 _velocity { get; protected set; }


    // Use this for initialization
    void Start () {
        defineAgent();
	}
	

	// Update is called once per frame
	void Update () {

        // Apply changes with steering force
        updateAgent();
        
        // Apply changes to the GameObject
        updateGameObject();
	}


    // Update the agent values
    void updateAgent () {
        // Compute steering force under  max steer
        Vector2 steerForce = steeringForces() + steeringSeparation()*coefSeparation;
        steerForce = Vector2.ClampMagnitude(steerForce, _maxSteer);

        // Compute acceleration
        Vector2 acceleration = steerForce / _mass;

        // Compute new velocity
        Vector2 newVelocity = _velocity + acceleration;

        // Compute new orientation
        _orientation += Mathf.Acos(Vector2.Dot(_velocity, newVelocity)) % 360.0f;

        // Apply new velocity under a max speed
        _velocity = Vector2.ClampMagnitude(newVelocity, _maxSpeed);

        // Compute new position
        _position += _velocity * Time.deltaTime;
    }


    // Update the agent in the game world
    void updateGameObject () {
        transform.rotation = Quaternion.AngleAxis(_orientation, new Vector3(0,1,0));
        transform.position = new Vector3(_position.x, transform.position.y, _position.y);
    }


    // Define the agent properties
    virtual protected void defineAgent () {
        _mass = 5;
        _maxSpeed = 30;
        _maxSteer = 1000;
        _orientation = Random.Range(0.0f,360.0f);
        _position = new Vector2(Random.Range(50,450), Random.Range(50, 450));
        _velocity = Vector2.zero;
    }


    // Reimplemented function for different comportment
    virtual protected Vector2 steeringForces () {
        return Vector2.zero;
    }


    // Keep a living space
    protected Vector2 steeringSeparation() {
        Vector2 force = Vector2.zero;

        // Get all Followers in the scene
        Agent[] agentList = GameObject.FindObjectsOfType<Agent>();

        // For each follower
        for (int i = 0; i < agentList.Length; i++)
        {
            // Don't apply force with himself
            if (!agentList[i].Equals(this))
            {
                // Compute the distance to the other follower
                Vector2 positionDifference = _position - agentList[i]._position;
                float distance = positionDifference.magnitude;

                // If he is in the living space
                force = (distance < livingSpaceRadius) ? (force + positionDifference.normalized / distance) : force;
            }
        }

        return force;
    }
}
