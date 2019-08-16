using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class AgentTarget : MonoBehaviour
{

    public Transform goal;

    // Use this for initialization
    void Start()
    {
        GetComponent<NavMeshAgent>().SetDestination(goal.position);
    }

}
