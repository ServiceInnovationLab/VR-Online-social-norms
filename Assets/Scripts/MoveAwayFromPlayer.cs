using UnityEngine;
using System.Collections;

using VRTK;

[RequireComponent(typeof(CharacterController))]
public class MoveAwayFromPlayer : MonoBehaviour
{
    public float speed = 5;

    CharacterController controller;
    float velocity;
    bool runningAway = false;

    GameObject[] targets;
    int target = -1;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        targets = GameObject.FindGameObjectsWithTag("Food");
    }

    private void FixedUpdate()
    {
        var player = VRTK_DeviceFinder.HeadsetTransform();

        if (!player)
            return;

        var direction = transform.position.XZ() - player.position.XZ();

        if (direction.sqrMagnitude < 9)
        {
            transform.LookAt(transform.position + direction);
            velocity = 1.0f;
            runningAway = true;
        }        

        if (runningAway)
        {
            controller.Move(transform.forward * speed * velocity * Time.deltaTime);

            velocity *= 0.9f;
            runningAway = velocity >= 0.05f;
        }
        else
        {
            GoAfterTarget(player);
        }
    }

    void GoAfterTarget(Transform player)
    {
        if (target > -1 && targets[target].tag != "Food")
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

                if (go.tag != "Food")
                    continue;

                var distance = Vector3.Distance(transform.position, go.transform.position);

                if ((go.transform.position.XZ() - player.position.XZ()).sqrMagnitude < 9) continue;

                if (distance < closest)
                {
                    closest = distance;
                    target = i;
                }
            }
        }

        if (target > -1)
        {
            if ((targets[target].transform.position.XZ() - player.position.XZ()).sqrMagnitude < 9)
            {
                target = -1;
                return;
            }

            var distance = Vector3.Distance(transform.position.XZ(), targets[target].transform.position.XZ());

            if (distance < 0.5f)
                return;

            transform.LookAt(targets[target].transform.position.XZ());
            velocity = Random.Range(0.0f, 0.5f);
            controller.Move(transform.forward * velocity * speed * Time.deltaTime);
        }
    }
}
