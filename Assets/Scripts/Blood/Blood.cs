using UnityEngine;
using System.Collections;

public class Blood : MonoBehaviour {

    // Generic attributes
    GameObject _gameObject;
    Rigidbody _rigidbody;
    Color _color = Color.red;

    float _timer = 0.0f;
    float _timeLife = 10.0f;

    public bool _canBeDestroy = false;

    // Create the Game Object
    public void create(Vector3 position, Vector3 velocity)
    {
        _gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        _gameObject.name = "Blood";
        _gameObject.transform.position = position;
        _gameObject.GetComponent<Renderer>().material.color = _color;

        _rigidbody = _gameObject.AddComponent<Rigidbody>();
        _rigidbody.mass = 5.0f;
        _rigidbody.useGravity = true;
        _rigidbody.AddForce(velocity, ForceMode.Impulse);
    }

    // Update blood position
    void Update()
    {
        if(_timer < _timeLife)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _canBeDestroy = true;
        }
    }

    // Destroy the Game Object
    public void destroy()
    {
        Destroy(_gameObject);
    }
}
