using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class RunAwayOnPhotoTaken : MonoBehaviour
{
    [SerializeField] new Renderer renderer;
    [SerializeField] Transform facing;
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    void OnEnable()
    {
        EventManager.StartListening(Events.PhotoTaken, OnPhotoTaken);
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

        StartCoroutine(RunAway(-facing.forward.XZ().normalized));
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
