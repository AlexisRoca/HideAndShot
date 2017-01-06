using UnityEngine;
using System.Collections;

public class AgentEngine : MonoBehaviour {

    public float _coefLeader = 1.0f;
    public float _coefAvoidObs = 300.0f;
    public float _coefStayOut = 300.0f;
    public float _coefSeparation = 300.0f;

    // Define Agent attributes
    public int _leaderMass = 10;
    public int _leaderSpeed = 50;
    public int _leaderSteer = 100;
    public int _leaderRadius = 10;
    public float _leaderVariation = 0.001f;

    public int _followerMass = 10;
    public int _followerSpeed = 45;
    public int _followerSteer = 100;

    public int _drunkMass = 10;
    public int _drunkSpeed = 20;
    public int _drunkSteer = 100;

    // Define Agent lists
    public Agent[] m_agentList;
    public Agent[] m_leaderList;
    public Agent[] m_followerList;
    public Agent[] m_drunkList;


    public void initAgents()
    {
        // Define Agent lists
        m_agentList = GameObject.FindObjectsOfType<Agent>();

        // Define Game Objects lists
        GameObject[] leaderListGO   = GameObject.FindGameObjectsWithTag("Leader");
        GameObject[] followerListGO = GameObject.FindGameObjectsWithTag("Follower");
        GameObject[] drunkListGO    = GameObject.FindGameObjectsWithTag("Drunk");

        // Leaders
        m_leaderList = new Agent[leaderListGO.Length];
        for(int i=0; i < leaderListGO.Length; i++)
        {
            m_leaderList.SetValue(leaderListGO[i].GetComponent<Agent>(), i);
        }

        // Follower
        m_followerList = new Agent[followerListGO.Length];
        for(int i=0; i < followerListGO.Length; i++)
        {
            m_followerList.SetValue(followerListGO[i].GetComponent<Agent>(), i);
        }

        // Drunk
        m_drunkList = new Agent[drunkListGO.Length];
        for(int i=0; i < drunkListGO.Length; i++)
        {
            m_drunkList.SetValue(drunkListGO[i].GetComponent<Agent>(), i);
        }

        // Define Agent properties
        defineAgentProperties();
    }

    public void update(float deltaTime)
    {
        redefineAgentProperties();

        updateLeaders();
        updateFollowers();
        updateDrunk();
        updateSeparation(m_followerList,_coefSeparation);

        updateAgentPosition(deltaTime);
    }

    // First definition of agent (with orientation)
    private void defineAgentProperties()
    {
        // Leaders
        foreach(Agent leader in m_leaderList)
            leader.defineAgent(_leaderMass,_leaderSpeed,_leaderSteer,Random.Range(0.0f,360.0f),Random.Range(0.0f,2 * Mathf.PI));

        // Follower
        foreach(Agent follower in m_followerList)
            follower.defineAgent(_followerMass,_followerSpeed,_followerSteer,Random.Range(0.0f,360.0f));

        // Drunk
        foreach(Agent drunk in m_drunkList)
            drunk.defineAgent(_drunkMass,_drunkSpeed,_drunkSteer,Random.Range(0.0f,360.0f),Random.Range(0.0f,2 * Mathf.PI));
    }

    // Update definition (without orientation)
    private void redefineAgentProperties()
    {
        // Leaders
        foreach (Agent leader in m_leaderList)
            leader.redefineAgent(_leaderMass, _leaderSpeed, _leaderSteer);

        // Follower
        foreach (Agent follower in m_followerList)
            follower.redefineAgent(_followerMass, _followerSpeed, _followerSteer);

        // Drunk
        foreach (Agent drunk in m_drunkList)
            drunk.redefineAgent(_drunkMass, _drunkSpeed, _drunkSteer);
    }


    // Leader gestion
    private void updateLeaders()
    {
        foreach(Agent leader in m_leaderList)
        {
            Vector2 leadSteer = AgentSteering.leader(leader._velocity,_leaderRadius,_leaderVariation, ref leader._wanderPoint) * _coefLeader;
            Vector2 avoidSteer = AgentSteering.avoid(leader._position, leader._velocity, ref leader._wanderPoint) * _coefAvoidObs;

            Vector2 force = leadSteer + avoidSteer;

            leader._steeringForce += force;
        }
    }


    // Follower gestion
    private void updateFollowers()
    {
        foreach(Agent follower in m_followerList)
        {
            Agent nearestLeader = findNearest(follower, m_leaderList);

            Vector2 followSteer = AgentSteering.follow(follower._position, nearestLeader._position, nearestLeader._velocity);
            Vector2 stayOutSteer = AgentSteering.stayOut(follower._position, nearestLeader._position, nearestLeader._velocity) * _coefStayOut;

            Vector2 force = followSteer + stayOutSteer;

            follower._steeringForce += force;
        }
    }


    // Drunk gestion
    private void updateDrunk()
    {
        foreach(Agent drunk in m_drunkList)
        {
            Vector2 drunkSteer = AgentSteering.leader(drunk._velocity, 20, 0.5f, ref drunk._wanderPoint);
            Vector2 avoidSteer = AgentSteering.avoid(drunk._position, drunk._velocity, ref drunk._wanderPoint) * _coefAvoidObs;

            Vector2 force = drunkSteer + avoidSteer;

            drunk._steeringForce += force;
        }
    }

    // Separation gestion
    private void updateSeparation(Agent [] separationList, float coefSeparation)
    {
        foreach(Agent separation in separationList)
        {
            Agent[] neighbours = findNeighbours(separation, 20.0f);
            Vector2[] positionNeighbours = new Vector2[neighbours.Length];

            for(int i=0; i < neighbours.Length; i++)
            {
                positionNeighbours.SetValue(neighbours[i]._position, i);
            }

            Vector2 steer = AgentSteering.separation(separation._position, positionNeighbours) * coefSeparation;
            separation._steeringForce += steer;
        }
    }

    private void updateAgentPosition(float deltaTime)
    {
        // All agent
        foreach(Agent agent in m_agentList)
        {
            if (! agent._isDead)
            {
                agent.updateAgent(deltaTime);
                agent.transform.rotation = Quaternion.Euler(0.0f, agent._orientation, 0.0f);
                agent.transform.position = new Vector3(agent._position.x, agent.transform.position.y, agent._position.y);
            } else {
                agent.isBlooding(deltaTime);
            }
        }
    }


    // Find the nearest agent taged T
    public Agent findNearest(Agent agent, Agent[] nearestList)
    {
        // Define distance and GameObject for the nearest agent
        Agent nearestAgent = null;
        float nearestDistance = float.MaxValue;

        // Find the nearest to follow
        foreach(Agent currentAgent in nearestList)
        {
            if(currentAgent.Equals(agent) || currentAgent._isDead)
                continue;

            float currentDistance = (agent._position - currentAgent._position).magnitude;

            if(currentDistance < nearestDistance)
            {
                nearestAgent = currentAgent;
                nearestDistance = currentDistance;
            }
        }

        return nearestAgent;
    }


    // Find neighbours agent
    public Agent [] findNeighbours(Agent agent, float distance)
    {
        // Get all Agent taged T in the scene
        Agent [] agentList = GameObject.FindObjectsOfType<Agent>();
        bool [] inNeighbourhood = new bool [agentList.Length];

        int nbNeighbours = 0;

        // Find the number of contributors
        for(int i = 0; i < agentList.Length; i++)
        {
            if(agentList[i].Equals(agent))
                continue;

            float currentDistance = (agent._position - agentList [i]._position).magnitude;

            if(currentDistance < distance)
            {
                nbNeighbours++;
                inNeighbourhood [i] = true;
            }
            else
            {
                inNeighbourhood [i] = false;
            }
        }

        Agent [] neighbours = new Agent [nbNeighbours];
        int it = 0;

        // Create the neighbour list
        for(int i = 0; i < agentList.Length; i++)
        {
            if(inNeighbourhood [i])
            {
                neighbours.SetValue(agentList [i],it);
                it++;
            }
        }

        return neighbours;
    }
}