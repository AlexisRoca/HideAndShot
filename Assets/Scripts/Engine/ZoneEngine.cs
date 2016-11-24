using UnityEngine;
using System.Collections;

public class ZoneEngine : MonoBehaviour {

    // Define steering coefficient
    public float _coefCrossing = 300.0f;
    public int _timeCrossing = 20;

    // Define Zone lists
    public Zone[] _crossingList;
    public GameObject[] _fireList;

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
