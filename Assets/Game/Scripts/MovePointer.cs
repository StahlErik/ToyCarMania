using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using System;

public class MovePointer : MonoBehaviour
{
    public GameObject placementIndicator;
    private Frame Frame;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private List<DetectedPlane> m_DetectedPlanes = new List<DetectedPlane>();
    private Vector3 screenCenter;

    // Update is called once per frame
    void Update()
    {
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }
        //Check ARCore session status
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    public Pose getPlayerPointer()
    {
        return placementPose;
    }

    private void UpdatePlacementIndicator()
    {
        if(placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        } else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hit = new TrackableHit();
        Frame.Raycast(screenCenter.x,screenCenter.y, TrackableHitFlags.PlaneWithinPolygon, out hit);

        var hitList = new List<TrackableHit>();
        hitList.Add(hit);
        placementPoseIsValid = hitList.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hitList[0].Pose;
        }
    }
}
