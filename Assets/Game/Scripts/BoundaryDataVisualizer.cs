    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;
using UnityEngine.SceneManagement;
using System;

public class BoundaryDataVisualizer : MonoBehaviour
{

    private List<DetectedPlane> m_NewPlanes = new List<DetectedPlane>();
    private DetectedPlane m_detectedPlane;
    private DetectedPlane choosenPlane;
    public GameObject PlaneCenterMarker;
    public Button choosePlaneButton;
    private bool isSearching = true;
    public GameHandler gameHandler;

    // Update is called once per frame
    private void Start()
    {
        LineRenderer line = gameObject.AddComponent<LineRenderer>();
        line.widthMultiplier = 0.02f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.gray;
        line.endColor = Color.gray;
        choosePlaneButton.onClick.AddListener(ChoosePlane);
    }

    void Update()  
    {
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        } else if (isSearching)
        {
            IdentifyCenterPlane();
        }
    }

    void IdentifyCenterPlane()
    {
        Session.GetTrackables<DetectedPlane>(m_NewPlanes, TrackableQueryFilter.All);
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        TrackableHit hit;
        TrackableHitFlags raycastFilter =
            TrackableHitFlags.PlaneWithinBounds |
            TrackableHitFlags.PlaneWithinPolygon;        

        if (Frame.Raycast(screenCenter.x, screenCenter.y, raycastFilter, out hit))
        {
            for (int i = 0; i < m_NewPlanes.Count; i++)
            {
                if (hit.Trackable as DetectedPlane == m_NewPlanes[i])
                {
                    OutputPlaneLengths(m_NewPlanes[i]);
                    PutPlaneCenterMarker(m_NewPlanes[i]);
                    RenderLines(m_NewPlanes[i]);
                    if (isSearching)
                    {
                        choosePlaneButton.interactable = true;
                    }
                    m_detectedPlane = m_NewPlanes[i];
                } 
            }   
        }
        else
        {
            if (isSearching)
            {
                choosePlaneButton.interactable = false;
            }
            PlaneCenterMarker.SetActive(false);
            LineRenderer line = GetComponent<LineRenderer>();
            line.enabled = false;
            m_detectedPlane = null;
        }
    }

    private void RenderLines(DetectedPlane selectedPlane)
    {       
        List<Vector3> boundary = new List<Vector3>();
        selectedPlane.GetBoundaryPolygon(boundary);
        var boundaryPoints = new Vector3[boundary.Count + 1];
        for (int i = 0; i < boundary.Count; i++)
        {
            boundaryPoints[i] = boundary[i];
        }
        boundaryPoints[boundary.Count] = boundary[0];
        LineRenderer line = GetComponent<LineRenderer>();
        line.enabled = true;
        line.positionCount = boundary.Count;

        line.SetPositions(boundaryPoints);
        line.loop = true;
    }

    private void PutPlaneCenterMarker(DetectedPlane selectedPlane)
    {
        PlaneCenterMarker.SetActive(true);
        PlaneCenterMarker.transform.SetPositionAndRotation(selectedPlane.CenterPose.position, selectedPlane.CenterPose.rotation);
    }

    private void OutputPlaneLengths(DetectedPlane selectedPlane)
    {
    }

    private void ChoosePlane()
    {
        isSearching = !isSearching;
        gameObject.GetComponent<GoogleARCore.Examples.Common.DetectedPlaneGenerator>().setSearching(!gameObject.GetComponent<GoogleARCore.Examples.Common.DetectedPlaneGenerator>().isSearching);
        gameObject.GetComponent<GoogleARCore.Examples.Common.DetectedPlaneVisualizer>().setSearching(!gameObject.GetComponent<GoogleARCore.Examples.Common.DetectedPlaneVisualizer>().isSearching);


        if (isSearching)
        {
            choosePlaneButton.GetComponentInChildren<Text>().text = "Choose Plane";
            choosenPlane = null;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            choosenPlane = m_detectedPlane;
            choosePlaneButton.GetComponentInChildren<Text>().text = "Restart";
            choosePlaneButton.interactable = true;
            foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("PlaneVisualizer"))
            {
                if (fooObj.name == "DetectedPlaneVisualizer(Clone)")
                {
                    fooObj.SetActive(false);
                }
            }

            List<Vector3> boundary = new List<Vector3>();
            choosenPlane.GetBoundaryPolygon(boundary);

            gameHandler.setPlane(
                new Vector3(choosenPlane.CenterPose.position.x, choosenPlane.CenterPose.position.y, choosenPlane.CenterPose.position.z),
                new Vector2(choosenPlane.ExtentX, choosenPlane.ExtentZ),
                boundary
                );
        }

    }
}
