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

}
