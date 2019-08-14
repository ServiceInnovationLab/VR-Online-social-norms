using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Linq;

using VRTK;

[RequireComponent(typeof(CharacterController))]
public class AnimalAfterFood : MonoBehaviour
{
    [SerializeField] UnityEvent foodEaten;

    [SerializeField] float speed = 5;
    [SerializeField] float walkingDistance = 1.0f;
    [SerializeField] VRTK_BasicTeleport teleport;

    CharacterController controller;
    float velocity;
    bool runningAway = false;
    bool skipRunningAway = false;

    Rigidbody[] targets;
    int target = -1;
    Vector3 lastPlayerPosition;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

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
            transform.LookAt(transform.position + direction);
            velocity = 1.0f;
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
                controller.Move(transform.forward.XZ() * speed * velocity * Time.deltaTime);

                velocity *= 0.9f;
                runningAway = velocity >= 0.05f;
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
        if (target == -1)
        {
            int i = -1;
            foreach (var go in targets)
            {
                i++;

                if (!go || go.tag != "Food")
                    continue;

                var distance = Vector3.Distance(transform.position, go.transform.position);

                //if ((go.transform.position.XZ() - player.position.XZ()).sqrMagnitude < 9) continue;

                if (distance < closest && (go.transform.position.y < 1 || !go.IsSleeping() || go.isKinematic))
                {
                    closest = distance;
                    target = i;
                }
            }
        }

        if (target > -1)
        {
            /* if ((targets[target].transform.position.XZ() - player.position.XZ()).sqrMagnitude < 9)
             {
                 target = -1;
                 return;
             }*/

            /* var distance = Vector3.Distance(transform.position.XZ(), targets[target].transform.position.XZ());

             if (distance < 0.5f)
             {
                 EatFood(targets[target].gameObject);
                 return;
             }*/

            var lookPos = targets[target].transform.position - transform.position;
            lookPos.y = 0;            
            transform.localRotation = Quaternion.LookRotation(lookPos);

            velocity = Random.Range(0.0f, 0.5f);
            controller.SimpleMove(transform.forward.XZ() * velocity * speed);// * Time.deltaTime);
        }
    }
}
