using UnityEngine;
using System.Collections;

public class ZoneEngine : MonoBehaviour {

    // Define steering coefficient
    public float _coefCrossing = 300.0f;
    public int _timeCrossing = 20;
    public float _percentCars = 0.1f;
    public int _maxCarsOnRoad = 5;

    int _nbCurrentCars = 0;


    // Define Zone lists
    public Zone[] _crossingList;
    public GameObject[] _fireList;
    public Car[] _carsList;

    // Constructor
    public void initZones() {
        // Define Zone lists
        _fireList = GameObject.FindGameObjectsWithTag("Fire");
        GameObject[] crossingListGO = GameObject.FindGameObjectsWithTag("Cross");

        // Crossing
        _crossingList = new Zone[crossingListGO.Length];
        for (int i = 0; i < crossingListGO.Length; i++)
        {
            _crossingList.SetValue(crossingListGO[i].GetComponent<Zone>(), i);
        }

        // Define Cars list
        _carsList = new Car[_maxCarsOnRoad];
    }

    public void update(Agent[] agentList, float deltaTime)
    {
        defineZoneProperties();

        updateZones(agentList, deltaTime);
    }


    // Define the zone properties
    public void defineZoneProperties() {
        // Crossing
        foreach (Zone crossing in _crossingList)
        {
            crossing.defineTime(_timeCrossing);
        }
    }


    // Update all behavior in the scene
    public void updateZones(Agent[] agentList, float dTime) {
        // Crossing
        foreach (Zone crossing in _crossingList)
        {
            crossing.updateTime(dTime);
        }

        crossingForce(agentList);
        carsGeneration(dTime);

        // Fire Light
        for (int i = 0; i < _crossingList.Length; i++)
        {
            fireLight(_crossingList[i], _fireList[i]);
        }
    }


    // Crossing gestion
    private void crossingForce(Agent[] agentList) {
        foreach (Zone crossing in _crossingList) {
            foreach (Agent agent in agentList) {
                if (crossing.isInZone(agent._position)) {
                    Vector2 steerPushThrough = ZoneSteering.pushThrough(agent._velocity, crossing._upSide, crossing._rightSide, crossing._downSide, crossing._leftSide);
                    Vector2 steerPushOut = ZoneSteering.pushOut(agent._position, crossing._upSide, crossing._rightSide, crossing._downSide, crossing._leftSide, crossing._min, crossing._max);
                    
                    agent._steeringForce += (crossing._allowAgent) ? steerPushThrough*_coefCrossing : steerPushOut*_coefCrossing;
                }
            }
        }
    }


    // Generate cars drive through the cross road
    private void carsGeneration(float dTime) {
        foreach (Zone crossing in _crossingList) {
            if (! crossing.isRed()) {
                // Generate a new car
                if (Random.value < _percentCars && _nbCurrentCars < _maxCarsOnRoad && crossing.isGreen())
                {
                    // int sidePossibilities = 0;
                    // sidePossibilities += (crossing._upSide) ? 0 : 1;
                    // sidePossibilities += (crossing._leftSide) ? 0 : 1;
                    // sidePossibilities += (crossing._downSide) ? 0 : 1;
                    // sidePossibilities += (crossing._rightSide) ? 0 : 1;

                    Vector3 position = new Vector3(250.0f, 15.0f, 0.0f);
                    Vector3 rotation = new Vector3(0.0f, 0.0f, 0.0f);
                    Vector3 velocity = new Vector3(0.0f, 0.0f, 500.0f);

                    _carsList.SetValue(crossing.gameObject.AddComponent<Car>(), _nbCurrentCars);
                    _carsList[_nbCurrentCars].create(position, rotation, velocity);

                    _nbCurrentCars += 1;
                }

                // Already existed cars gestion
                for (int i=0; i<_nbCurrentCars; i++)
                {
                    _carsList[i].update(dTime);
                }
            }
            else if(crossing.isRed())
            {
                for (int i = 0; i < _nbCurrentCars; i++)
                {
                    _carsList[i].destroy();
                }
                
                if(crossing.gameObject.GetComponent<Car>())
                {
                    Destroy(crossing.gameObject.GetComponent<Car>());
                }

                _nbCurrentCars = 0;
            }
        }
    }


    // Fire Light gestion
    private void fireLight(Zone cross, GameObject fire) {
        foreach (Renderer globe in fire.GetComponentsInChildren<Renderer>()) {
            switch (globe.name) {
                case "Sphere" :
                    globe.material.color = (cross.isRed()) ? Color.black : Color.white;
                    break;
                case "Sphere_001":
                    globe.material.color = (cross.isOrange()) ? Color.black : Color.white;
                    break;
                case "Sphere_002":
                    globe.material.color = (cross.isGreen()) ? Color.black : Color.white;
                    break;
                case "Sphere_003":
                    globe.material.color = (cross.isRed()) ? Color.black : Color.white;
                    break;
                case "Sphere_004":
                    globe.material.color = (cross.isOrange()) ? Color.black : Color.white;
                    break;
                case "Sphere_005":
                    globe.material.color = (cross.isGreen()) ? Color.black : Color.white;
                    break;
            }
        }
    }
}
