using UnityEngine;
using VRTK;
using System.Linq;
using Valve.VR;
using System;

[Flags]
public enum DesiredPoint
{
    None = 0,
    Left,
    Right,
    Front,
    Back
}

public class VR_StartingPoint : MonoBehaviour
{
    [SerializeField] VRTK_SDKManager manager;
    [SerializeField] VRTK_BasicTeleport teleport;
    [SerializeField] bool rotate;
    [SerializeField] bool rotationAbsolute;
    [SerializeField] bool autoTrigger = true;
    

    [SerializeField, EnumFlag] DesiredPoint orientation = DesiredPoint.Back | DesiredPoint.Right;

    Vector3[] boundryVertices;
    [SerializeField] HeadsetEat headsetEat;

    private void Awake()
    {
        if (autoTrigger)
        {
            manager.LoadedSetupChanged += LoadedSetupChanged;
        }

        if (!headsetEat)
        {
            headsetEat = FindObjectOfType<HeadsetEat>();
        }

        if (headsetEat)
        {
            headsetEat.enabled = false;
        }
    }

    public void DoTeleport()
    {
        Quaternion? rotation = null;

        if (rotate)
        {
            var rotationY = rotationAbsolute ? transform.rotation.eulerAngles.y : VectorUtils.AngleOffAroundAxis(transform.forward, VRTK_DeviceFinder.HeadsetTransform().forward, Vector3.up);
            rotationY += VRTK_SDKManager.instance.transform.rotation.eulerAngles.y;
            rotation = Quaternion.Euler(0, rotationY, 0);
        }

        var playAreaTeleport = teleport as PlayAreaLimitedTeleport;
        bool originalCheckForCollisions = false;

        if (playAreaTeleport)
        {
            originalCheckForCollisions = playAreaTeleport.checkForCollisions;
            playAreaTeleport.checkForCollisions = false;
        }

        teleport.skipBlink = true;
        teleport.Teleport(transform, transform.position, rotation);
        teleport.skipBlink = false;

        headsetEat.enabled = true;

        Destroy(gameObject);

        if (playAreaTeleport)
        {
            playAreaTeleport.checkForCollisions = originalCheckForCollisions;
        }
    }

    private void SteamTransformUpdate(SteamVR_Behaviour_Pose fromAction, SteamVR_Input_Sources fromSource)
    {
        fromAction.onTransformUpdated.RemoveListener(SteamTransformUpdate);
        fromAction.onTransformChanged.RemoveListener(SteamTransformUpdate);       

        // TODO: Movement of the play area

        //var allZValues = boundryVertices.Select(v => v.z).ToArray();
        //var allXValues = boundryVertices.Select(v => v.x).ToArray();

        //float minX = Mathf.Min(allXValues);
        //float maxX = Mathf.Max(allXValues);
        //float minZ = Mathf.Min(allZValues);
        //float maxZ = Mathf.Max(allZValues);

        //float centreX = (maxX + minX) / 2;
        //float centreZ = (maxZ + minZ) / 2;

        //var playerPos = fromAction.transform.localPosition;

        //var currentPoint = DesiredPoint.None;

        //if (playerPos.x < centreX)
        //{
        //    currentPoint |= DesiredPoint.Left;
        //}
        //else
        //{
        //    currentPoint |= DesiredPoint.Right;
        //}

        //if (playerPos.z < centreZ)
        //{
        //    currentPoint |= DesiredPoint.Front;
        //}
        //else
        //{
        //    currentPoint |= DesiredPoint.Back;
        //}

        //if (currentPoint != orientation)
        //{
        //    var playarea = VRTK_DeviceFinder.PlayAreaTransform();

        //    if (currentPoint.HasFlag(DesiredPoint.Front) && orientation.HasFlag(DesiredPoint.Back))
        //    {
        //        playarea.transform.localPosition += new Vector3(0, 0, minZ);
        //    }
        //    else if (currentPoint.HasFlag(DesiredPoint.Back) && orientation.HasFlag(DesiredPoint.Front))
        //    {
        //        playarea.transform.localPosition += new Vector3(0, 0, maxZ);
        //    }

        //    if (currentPoint.HasFlag(DesiredPoint.Left) && orientation.HasFlag(DesiredPoint.Right))
        //    {
        //        playarea.transform.localPosition += new Vector3(minX, 0, 0);
        //        playarea.transform.localRotation = Quaternion.Euler(0, 180, 0);
        //    }
        //    else if (currentPoint.HasFlag(DesiredPoint.Right) && orientation.HasFlag(DesiredPoint.Left))
        //    {
        //        playarea.transform.localPosition += new Vector3(maxX, 0, maxZ);
        //        playarea.transform.localRotation = Quaternion.Euler(0, 180, 0);
        //    }
        //}

       DoTeleport();
    }

    private void LoadedSetupChanged(VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
    {
        if (e.currentSetup == null)
            return;

        var headsetPose = e.currentSetup.actualHeadset.GetComponent<SteamVR_Behaviour_Pose>();

        if (headsetPose)
        {
            boundryVertices = e.currentSetup.boundariesSDK.GetPlayAreaVertices();

            headsetPose.onTransformUpdated.AddListener(SteamTransformUpdate);
            headsetPose.onTransformChanged.AddListener(SteamTransformUpdate);
        }
        else
        {
            DoTeleport();
        }
    }
}
