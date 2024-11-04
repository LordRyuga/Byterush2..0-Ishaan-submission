using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class target : MonoBehaviour
{
    [SerializeField] private Transform movePos;

    public GameObject player;
    private NavMeshAgent agent;
    bool patrol1 = false;
    bool patrol2 = true;
    bool patrol3 = false;
    bool sendAtHome = true;

    public GameObject p1Obj;
    public GameObject p2Obj;
    public GameObject p3Obj;
    

    public float detectPlayer = 4.0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < detectPlayer) 
        {
            agent.destination = player.transform.position;
        }
        else
        {
            if (patrol1)
            {
                agent.destination = p2Obj.transform.position;
                sendAtHome = false;
            }
            if (patrol2)
            {
                if (!sendAtHome)
                {
                    agent.destination = p3Obj.transform.position;
                }
                else 
                {
                    agent.destination = p1Obj.transform.position;
                }
            }
            if (patrol3)
            {
                agent.destination = p2Obj.transform.position;
                sendAtHome = true;
            }
        }
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(1);
        if(other.name == "pt1")
        {
            patrol1 = true;
            patrol2 = false;
            patrol3 = false;
            //Debug.Log(patrol1);
        }
        if (other.name == "pt2")
        {
            patrol1 = false;
            patrol2 = true;
            patrol3 = false;
            //Debug.Log(patrol2);
        }
        if (other.name == "pt3")
        {
            patrol1 = false;
            patrol2 = false;
            patrol3 = true;
            //Debug.Log(patrol3);
        }
    }
}
