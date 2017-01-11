using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

    // Attributes deffinition (get/set)
    // Personal attributes
    public int _mass { get; set; }
    public int _maxSpeed { get; set; }
    public int _maxSteer { get; set; }
    public float _wanderPoint = 0.0f;

    // Dead attributes
    public bool _isDead = false;
    public float _timer = 0.0f;
    public float _timeBlooding = 5.0f;
    Vector3 bloodDirection = Vector3.zero;

    // Displacement attributes
    public float _orientation { get; set; }
    public Vector2 _position { get; set; }
    public Vector2 _velocity { get; set; }

    // Steering force memory (RAZ at each update)
    public Vector2 _steeringForce { get; set; }


    // Use this for initialization
    void Awake()
    {
        _orientation = 0.0f;
        _position = new Vector2(transform.position.x, transform.position.z);
        _velocity = Vector2.zero;

        this.GetComponent<Rigidbody>().mass = 100;

        _steeringForce = Vector2.zero;
    }


    // Define personal agent
    public void defineAgent(int mass, int maxSpeed, int maxSteer, float orientation, float wander = 0.0f) {
        _mass = (mass == 0) ? 1 : mass;
        _maxSpeed = maxSpeed;
        _maxSteer = maxSteer;
        _orientation = orientation;

        _wanderPoint = wander;
    }


    // Re-Define personal agent
    public void redefineAgent(int mass, int maxSpeed, int maxSteer)
    {
        _mass = (mass == 0) ? 1 : mass;
        _maxSpeed = maxSpeed;
        _maxSteer = maxSteer;
    }


    // Update the agent values
    public virtual void updateAgent (float dTime) {
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


    // Blooding agent
    public void isBlooding(float dTime)
    {
        if(_timer < _timeBlooding)
        {
            _timer += dTime;
            Blood blood = this.gameObject.AddComponent<Blood>();
            blood.create(this.gameObject.transform.position, bloodDirection);
        }
        else if (this.gameObject.GetComponent<Blood>())
        {
            Blood currentBlood = this.gameObject.GetComponent<Blood>();
            if(currentBlood._canBeDestroy)
            {
                currentBlood.destroy();
                Destroy(currentBlood);
            }
        }
    }


    // Call while there is a collision
    void OnCollisionEnter(Collision collision)
    {
        if (_isDead) return;

        if (collision.gameObject.tag == "KillAgent")
        {
            _isDead = true;

            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            gameObject.GetComponent<Rigidbody>().AddForce(collision.impulse);

            Debug.Log(collision.impulse);

            // Blood emmision
            bloodDirection = collision.relativeVelocity;
        }
    }
}
