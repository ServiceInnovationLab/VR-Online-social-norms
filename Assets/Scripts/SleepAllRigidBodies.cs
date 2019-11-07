using UnityEngine;
using System.Collections.Generic;

public class SleepAllRigidBodies : MonoBehaviour
{
    [SerializeField] Rigidbody[] excludes;

    private void Start()
    {
        var excludesSet = new HashSet<Rigidbody>(excludes);

        foreach (var body in FindObjectsOfType<Rigidbody>())
        {
            if (excludesSet.Contains(body))
                continue;

            body.Sleep();
        }
    }
}
