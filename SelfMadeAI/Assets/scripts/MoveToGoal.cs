using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MoveToGoal : Agent
{
    float movespeed = 5.0f;
    [SerializeField] private Transform target;
    [SerializeField] private Material win;
    [SerializeField] private Material lose;
    [SerializeField] private MeshRenderer floor;

    public Vector3 distance;
    public Vector3 distance2;
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-3f, 1.5f), 5.2466f, Random.Range(-0.8f, 1.7f));
        //target.localPosition = new Vector3(Random.Range(-3.37f, 4.13f), 5.2466f, Random.Range(-5.888f , - 4.138f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);
        distance2 = transform.localPosition - target.transform.localPosition;
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float MoveX = actions.ContinuousActions[0];
        float MoveZ = actions.ContinuousActions[1];



        transform.localPosition += new Vector3(MoveX, 0, MoveZ) * Time.deltaTime * movespeed;
        //distance = transform.localPosition - target.transform.localPosition;
        //if(distance.magnitude < distance2.magnitude)
        //{
        //    SetReward(0.1f * tinyAward);
        //}else
        //{
        //    SetReward(-tinyAward);
        //}
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        continousActions[0] = Input.GetAxisRaw("Horizontal");
        continousActions[1] = Input.GetAxisRaw("Vertical");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            SetReward(+1000f);
            floor.material = win;
            EndEpisode();
        }
        if (other.tag == "wall")
        {
            SetReward(-10f);
            floor.material = lose;
            //EndEpisode();
        }
    }
    private void Update()
    {
        if(transform.position.y < 4.2f)
        {
            SetReward(-100f);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "GameController")
        {
            SetReward(-8f);
            //Debug.Log(1);
        }
        if(other.tag == "offRegion")
        {
            SetReward(-0.1f);
            floor.material = win;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "wallOut")
        {
            SetReward(-10000f);
            floor.material = lose;
            Debug.Log(5);
        }
        if (collision.collider.tag == "wall")
        {
            SetReward(-10f);
            floor.material = lose;
            Debug.Log(3);
            //EndEpisode();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "GameController")
        {
            SetReward(9f);
            Debug.Log(2);
            floor.material = win;
        }
        if(other.tag == "offRegion")
        {
            SetReward(-100f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "wall")
        {//-1 -6 x z
            SetReward(-10f);
            floor.material = lose;
            Debug.Log(3);
            //EndEpisode();
        }
        if(collision.collider.tag == "wallOut")
        {
            SetReward(-10000f);
            floor.material = lose;
            Debug.Log(4);
        }
    }
}
