using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

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
        // Compute steering force under  max steer
        Vector2 steerForce = Vector2.ClampMagnitude(steering(), _maxSteer);

        // Compute acceleration
        Vector2 acceleration = steerForce / _mass;

        // Compute new velocity
        Vector2 newVelocity = _velocity + acceleration * Time.deltaTime;

        // Compute new orientation
        _orientation += Mathf.Acos(Vector2.Dot(_velocity, newVelocity)) % 360.0f;

        // Apply new velocity under a max speed
        _velocity = Vector2.ClampMagnitude(newVelocity, _maxSpeed);

        // Compute new position
        _position += _velocity * Time.deltaTime;


        // Apply changes to the GameObject
        updateGameObject();
	}


    // Update the agent in the game world
    void updateGameObject () {
        transform.position = new Vector3(_position.x, transform.position.y, _position.y);
    }


    // Define the agent properties
    virtual protected void defineAgent () {
        _mass = 0;
        _maxSpeed = 0;
        _maxSteer = 0;
        _orientation = 0.0f;
        _position = Vector2.zero;
        _velocity = Vector2.zero;
    }


    // Reimplemented function for different comportment
    virtual protected Vector2 steering () {
        return Vector2.zero;
    }     
}
