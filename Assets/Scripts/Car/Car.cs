using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {

    // Generic attributes
    GameObject _gameObject;
    Color _color;

    Vector3 _position;
    Vector3 _rotation;
    Vector3 _velocity;


    // Create the Game Object
    public void create(Vector3 position, Vector3 rotation, Vector3 velocity) {
        _gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _gameObject.name = "Car";
        _gameObject.tag = "KillAgent";

        _gameObject.transform.localScale = new Vector3(30.0f, 20.0f, 70.0f);

        Rigidbody _rigidbody = _gameObject.AddComponent<Rigidbody>();
        _rigidbody.isKinematic = false;

        _position = position;
        _rotation = rotation;
        _velocity = velocity;

        _gameObject.transform.position = _position;
        _gameObject.transform.rotation = Quaternion.Euler(_rotation);
    }


    // Update car position
    public void update(float dTime)
    {
        _position = _position + _velocity * dTime;

        _gameObject.transform.position = _position;
    }


    // Destroy the Game Object
    public void destroy()
    {
        Destroy(_gameObject);
    }
}
