using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.AI;

public class RunAwayOnPhotoTaken : MonoBehaviour
{
    [SerializeField] UnityEvent onRunAway;
    [SerializeField] new Renderer renderer;    
    [SerializeField] NavMeshAgent agent;
    bool runningAway = false;

    private void Awake()
    {
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }
    
    void OnEnable()
    {
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
        StartCoroutine(RunAway(-agent.transform.forward.XZ().normalized));

        runningAway = true;
        EventManager.StopListening(Events.PhotoTaken, OnPhotoTaken);
    }

    IEnumerator RunAway(Vector3 direction)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            agent.SetDestination(transform.position + direction * 2);
        }
    }
}
