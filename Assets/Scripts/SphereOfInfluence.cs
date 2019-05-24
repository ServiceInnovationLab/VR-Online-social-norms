using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTK;

public class SphereOfInfluence : MonoBehaviour
{
    public Transform target;


    public void DoMove()
    {
        var pos = target.position;

        Destroy(target.gameObject);
        Destroy(gameObject);

        var teleport = FindObjectOfType<VRTK_BasicTeleport>();        
        teleport.ForceTeleport(pos);
    }
    
}
