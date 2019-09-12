using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using VRTK;
using System.Linq;
using System.Collections;

public class AnimalAfterFoodAgent : MonoBehaviour
{
    [SerializeField] UnityEvent foodEaten;
    [SerializeField] Collider colliderBounds;
    [SerializeField] VRTK_BasicTeleport teleport;
    [SerializeField] float walkingDistance = 1.0f;
    [SerializeField] float speed = 2;
    [SerializeField] bool runFromPlayer = false;
    [SerializeField] int updateRate = 20;
    [SerializeField] float waterOffset = -0.1f;

    NavMeshAgent agent;
    bool runningAway = false;
    bool skipRunningAway = false;
    
    Rigidbody[] targets;
    int target = -1;
    Vector3 lastPlayerPosition;

    private int times;
    new Animation animation;

    string walkingAnimation;
    string idleAnimation;
    string eatingAnimation;

    int waterLayer;
    bool eating = false;
    float eatingLength;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == waterLayer)
        {
            agent.baseOffset = waterOffset;
        }
        else
        {
            if (!other.transform.CompareTag("Food"))
                return;

            EatFood(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == waterLayer)
        {
            agent.baseOffset = 0;
        }
    }

    private void Awake()
    {
        waterLayer = LayerMask.NameToLayer("Water");
        agent = GetComponent<NavMeshAgent>();
        animation = GetComponent<Animation>();

        if (animation)
        {
            foreach (AnimationState state in animation)
            {
                var name = state.name.ToLower();

                if (name.Contains("eat"))
                {
                    eatingAnimation = state.name;
                    eatingLength = state.clip.length;
                }
                else if (name.Contains("idle"))
                {
                    idleAnimation = state.name;
                }
                else if (name.Contains("walk"))
                {
                    walkingAnimation = state.name;
                }
            }
        }

        targets = GameObject.FindGameObjectsWithTag("Food").Select(x => x.GetComponent<Rigidbody>()).ToArray();
        teleport.Teleporting += Teleport_Teleported;
        teleport.Teleported += Teleport_Teleported;
    }

    private void Teleport_Teleported(object sender, DestinationMarkerEventArgs e)
    {
        skipRunningAway = true;
    }

    private void Update()
    {
        if (!animation || eating)
            return;

        if (agent.velocity.sqrMagnitude >= 0.1f)
        {
            animation.Play(walkingAnimation);
        }
        else
        {
            animation.Play(idleAnimation);
        }
    }

    private void FixedUpdate()
    {
        if (!runFromPlayer)
        {
            GoAfterTarget();
            return;
        }


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
            GoAfterTarget();
        }

        skipRunningAway = false;
    }

    private void EatFood(GameObject food)
    {
        food.tag = "Untagged";
        target = -1;
        foodEaten?.Invoke();
        agent.ResetPath();

        if (animation && !string.IsNullOrEmpty(eatingAnimation))
        {
            StartCoroutine(DoEatFoodAnimation(food));
        }
        else
        {
            Destroy(food);
            transform.localScale *= 1.05f;
        }
    }

    IEnumerator DoEatFoodAnimation(GameObject food)
    {
        var lookPos = food.transform.position - transform.position;
        lookPos.y = 0;
        transform.localRotation = Quaternion.LookRotation(lookPos);

        eating = true;
        animation.Play(eatingAnimation);

        float startTime = Time.time;

        Vector3 originalScale = food.transform.localScale;

        do
        {
            yield return new WaitForFixedUpdate();

            float scale = (Time.time - startTime) / eatingLength;
            scale = Mathf.Clamp(scale, 0, 1);
            scale = 1 - (scale * scale);

            food.transform.localScale = originalScale * scale;
        }
        while (animation.IsPlaying(eatingAnimation));

        Destroy(food);
        transform.localScale *= 1.05f;
        eating = false;
    }

    void GoAfterTarget()
    {
        if (times++ < updateRate || eating)
        {
            return;
        }
        times = 0;

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

                if (distance < closest && (go.isKinematic || !VectorUtils.IsPointWithinCollider(colliderBounds, go.position.XZ(), true)))
                {
                    closest = distance;
                    target = i;
                }
            }
        }

        if (target > -1)
        {
            var distance = Vector3.Distance(transform.position, targets[target].transform.position);

            if (distance < agent.radius)
            {
                EatFood(targets[target].gameObject);
                return;
            }

            if (targets[target].isKinematic)
            {
                var lookPos = targets[target].transform.position - transform.position;
                lookPos.y = 0;
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(lookPos), 0.75f);
            }

            agent.speed = Random.Range(speed / 4, speed / 2);
            agent.SetDestination(targets[target].transform.position);
        }
        else
        {
            agent.ResetPath();
        }
    }
}
