using UnityEngine;
using VRTK;

/// <summary>
/// This component allows teleporting to a target game object and have the player replace them,
/// with custom blink transition times to improve the feel of the POV change.
/// </summary>
public class BlinkAndReplaceTarget : MonoBehaviour
{
    [Tooltip("The target location the player will be teleported to on entering the sphere")]
    [SerializeField] Transform target;

    [Tooltip("The custom blink distance to use for the teleport to allow for a longer blink when teleporting to a nearby location")]
    [SerializeField, Range(0f, 32.9f)] float blinkDistance = 20;

    [Tooltip("The custom blink transition time to use for the teleport")]
    [SerializeField, Range(0, 10)] float blinkTransition = 2;

    public void DoTeleport()
    {
        var teleportPosition = target.position;
        var teleportRotation = target.rotation;

        Destroy(target.gameObject);

        var teleporter = FindObjectOfType<VRTK_BasicTeleport>();
        var originalBlinkDelay = teleporter.distanceBlinkDelay;
        var originalBlinkTransition = teleporter.blinkTransitionSpeed;

        teleporter.distanceBlinkDelay = blinkDistance;
        teleporter.blinkTransitionSpeed = blinkTransition;

        teleporter.ForceTeleport(teleportPosition, teleportRotation);

        teleporter.distanceBlinkDelay = originalBlinkDelay;
        teleporter.blinkTransitionSpeed = originalBlinkTransition;
    }
}