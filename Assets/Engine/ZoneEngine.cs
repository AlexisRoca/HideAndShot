using UnityEngine;
using System.Collections;

public class ZoneEngine {

    // Crossing gestion
    public static void crossing(Zone[] crossingList, Agent[] agentList, float coefCrossing) {
        foreach (Zone crossing in crossingList) {
            foreach (Agent agent in agentList) {
                if (crossing.isInZone(agent._position)) {
                    Vector2 steerPushThrough = ZoneSteering.pushThrough(agent._velocity, crossing._upSide, crossing._rightSide, crossing._downSide, crossing._leftSide);
                    Vector2 steerPushOut = ZoneSteering.pushOut(agent._position, crossing._upSide, crossing._rightSide, crossing._downSide, crossing._leftSide, crossing._min, crossing._max);
                    
                    agent._steeringForce += (crossing._allowAgent) ? steerPushThrough*coefCrossing : steerPushOut*coefCrossing;
                }
            }
        }
    }


    // Fire Light gestion
    public static void fireLight(Zone cross, GameObject fire) {
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
