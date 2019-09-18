using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class PlayerPoseDriver : TrackedPoseDriver
{
    private GameObject player;
    protected override void Awake() {
        base.Awake();
        player = GameObject.FindWithTag("Player");
    }

    protected override void SetLocalTransform(Vector3 newPosition, Quaternion newRotation, PoseDataFlags poseFlags)
    {
        if ((trackingType == TrackingType.RotationAndPosition ||
            trackingType == TrackingType.RotationOnly) && 
            (poseFlags & PoseDataFlags.Rotation) > 0)
        {
            player.transform.localRotation = (newRotation * Quaternion.Inverse(transform.localRotation)) * player.transform.localRotation;
            transform.localRotation = newRotation;
        }

        if ((trackingType == TrackingType.RotationAndPosition ||
            trackingType == TrackingType.PositionOnly) &&
            (poseFlags & PoseDataFlags.Position) > 0)
        {
            player.transform.localPosition += newPosition - transform.localPosition;
            transform.localPosition = newPosition;
        }
    }

    protected override void PerformUpdate()
    {
        base.PerformUpdate();
    }
}
