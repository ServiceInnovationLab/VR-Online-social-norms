using UnityEngine;
using VRTK;

/// <summary>
/// Triggers eating of edible objects when they are brought into the mouth
/// </summary>
[RequireComponent(typeof(VRTK_HeadsetCollision))]
public class HeadsetEat : MonoBehaviour
{
    VRTK_HeadsetCollision headsetCollision;

    void Awake()
    {
        headsetCollision = GetComponent<VRTK_HeadsetCollision>();    
    }

    void OnEnable()
    {
        headsetCollision.HeadsetCollisionDetect += HeadsetCollisionDetected;    
    }

    void OnDisable()
    {
        headsetCollision.HeadsetCollisionDetect -= HeadsetCollisionDetected;
    }

    private void HeadsetCollisionDetected(object sender, HeadsetCollisionEventArgs e)
    {
        var edibleObject = e.collider.GetComponent<Edible>();

        edibleObject?.OnEaten();
    }
}
