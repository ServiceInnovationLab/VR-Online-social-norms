using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using VRTK;
using System.Linq;

public class AnimalAfterFoodAgent : MonoBehaviour
{
    [SerializeField] UnityEvent foodEaten;
    [SerializeField] Collider colliderBounds;
    [SerializeField] VRTK_BasicTeleport teleport;
    [SerializeField] float walkingDistance = 1.0f;
    [SerializeField] float speed = 2;

    NavMeshAgent agent;
    bool runningAway = false;
    bool skipRunningAway = false;
    
    Rigidbody[] targets;
    int target = -1;
    Vector3 lastPlayerPosition;    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        targets = GameObject.FindGameObjectsWithTag("Food").Select(x => x.GetComponent<Rigidbody>()).ToArray();
        teleport.Teleporting += Teleport_Teleported;
        teleport.Teleported += Teleport_Teleported;
    }

    private void Teleport_Teleported(object sender, DestinationMarkerEventArgs e)
    {
        skipRunningAway = true;
    }

    private void FixedUpdate()
    {
        var player = VRTK_DeviceFinder.HeadsetTransform();

        if (!player)
            return;

        Vector3 playerMovement = player.transform.position.XZ() - lastPlayerPosition.XZ();

        if (playerMovement.magnitude != 0 || skipRunningAway)
        {
            lastPlayerPosition = player.transform.position;
        }

        var direction = transform.position.XZ() - player.position.XZ();

        if (!skipRunningAway && direction.sqrMagnitude < 4 && playerMovement.magnitude > walkingDistance * Time.deltaTime)
        {
            agent.speed = speed;
            agent.SetDestination(-direction + transform.position);
            runningAway = true;
        }

        if (runningAway)
        {
            if (skipRunningAway)
            {
                runningAway = false;
            }
            else
            {
                runningAway = agent.velocity.magnitude >= 0.05f;
            }

            target = -1;
        }
        else
        {
            GoAfterTarget(player);
        }

        skipRunningAway = false;
    }

    private void EatFood(GameObject food)
    {
        Destroy(food);
        transform.localScale *= 1.05f;
        target = -1;

        foodEaten?.Invoke();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.transform.CompareTag("Food"))
            return;

        EatFood(hit.transform.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Food"))
            return;

        EatFood(collision.transform.gameObject);
    }

    void GoAfterTarget(Transform player)
    {
        if (target > -1 && (!targets[target] || targets[target].tag != "Food"))
        {
            target = -1;
        }

        float closest = 10000;
        //  if (target == -1)
        {
            int i = -1;
            foreach (var go in targets)
            {
                i++;

                if (!go || go.tag != "Food")
                    continue;

                var distance = Vector3.Distance(transform.position, go.transform.position);

                //if ((go.transform.position.XZ() - player.position.XZ()).sqrMagnitude < 9) continue;

                if (distance < closest && (go.isKinematic || !VectorUtils.IsPointWithinCollider(colliderBounds, go.position.XZ())))
                {
                    closest = distance;
                    target = i;
                }
            }
        }

        if (target > -1)
        {
            if (targets[target].isKinematic)
            {
                var lookPos = targets[target].transform.position - transform.position;
                lookPos.y = 0;
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(lookPos), 0.75f); ;
            }

            agent.speed = Random.Range(0, speed / 2);
            agent.SetDestination(targets[target].transform.position);
        }
    }
}
