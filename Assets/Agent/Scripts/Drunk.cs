using UnityEngine;
using System.Collections;

public class Drunk : Leader {

    // Define the agent properties
    override protected void defineAgent()
    {
        _mass = 5;
        _maxSpeed = 15;
        _maxSteer = 500;
        _orientation = Random.Range(0.0f, 360.0f);
        _position = new Vector2(Random.Range(50, 450), Random.Range(50, 450));
        _velocity = Vector2.zero;
    }
}
