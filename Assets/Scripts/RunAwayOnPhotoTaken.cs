using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.AI;

public class RunAwayOnPhotoTaken : MonoBehaviour
{
    [SerializeField] UnityEvent onRunAway;
    [SerializeField] new Renderer renderer;    
    [SerializeField] NavMeshAgent agent;
    [SerializeField] CharacterController controller;
    [SerializeField] Transform direction;
    [SerializeField] float characterSpeed = 2.0f;
    [SerializeField] float delay = 2.0f;
    bool runningAway = false;

    private void Awake()
    {
        if (!agent && !controller)
        {
            Debug.LogError("Some form of controller is needed on this script", gameObject);
        }
    }
    
    void OnEnable()
    {
        if (!agent && !controller)
            return;

        if (!runningAway)
        {
            EventManager.StartListening(Events.PhotoTaken, OnPhotoTaken);
        }
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.PhotoTaken, OnPhotoTaken);
    }

    void OnPhotoTaken(IEventArgs args)
    {
        if (!(args is PhotoTakenEventArgs))
            return;

        var photoArgs = (PhotoTakenEventArgs)args;

        // See if in the photo
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(photoArgs.Camera);
        if (!GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
            return;

        onRunAway?.Invoke();
        StartCoroutine(RunAway(-direction.forward.XZ().normalized));

        runningAway = true;
        EventManager.StopListening(Events.PhotoTaken, OnPhotoTaken);
    }

    IEnumerator RunAway(Vector3 direction)
    {
        yield return new WaitForSeconds(delay);

        while (agent)
        {
            yield return new WaitForSeconds(0.5f);
            agent.SetDestination(transform.position + direction * 2);
        }

        Quaternion startingRotation = transform.localRotation;
        float startTime = Time.time;

        while (controller)
        {
            yield return new WaitForFixedUpdate();
            controller.SimpleMove(direction * characterSpeed);

            transform.localRotation = Quaternion.Euler(0, Mathf.Lerp(0, 180, (Time.time - startTime) / characterSpeed), 0) * startingRotation;
        }
    }
}
