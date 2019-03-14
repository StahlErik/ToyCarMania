using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameHandler : MonoBehaviour

{
    public GameObject groundPlane;
    public GameObject Wall;
    public GameObject truck;
    public GameObject PointCube;
    public GameObject Grenade;
    private Vector3 centerPos;
    private readonly float planeThickness = 1f;
    public float cubeSpawnTime = 5f;
    public float grenadeSpawnTime = 8f;
    private bool gameHasStarted = false;
    private List<Vector3> planeCorners;
    public GameObject completeLevelUI;
    public Text finalScoreText;

    // Update is called once per frame
    void Update()
    {
        if(!gameHasStarted)
        {
            return;
        }

        cubeSpawnTime -= Time.deltaTime;
        grenadeSpawnTime -= Time.deltaTime;
        if (cubeSpawnTime < 0)
        {
            cubeSpawnTime = 5f;
            GameObject pointCube = Instantiate(PointCube, calculateRandomPoint(0.5f), Quaternion.identity);
        }
        if (grenadeSpawnTime < 0f)
        {
            grenadeSpawnTime = 8f;
            GameObject grenade = Instantiate(Grenade, calculateRandomPoint(0.5f), Quaternion.identity);
        }
    }

    public void setPlane(Vector3 planePosition, Vector2 planeExtents, List<Vector3> boundary)
    {
        centerPos = planePosition;
        groundPlane.transform.localScale = new Vector3(planeExtents.x+50, planeThickness, planeExtents.y+50);
        Instantiate(groundPlane, new Vector3(planePosition.x, planePosition.y-planeThickness/2, planePosition.z), Quaternion.identity);
        setPlaneWalls(boundary);
        gameObject.GetComponent<MovePointer>().enabled = true;
        Invoke("placeTruck", 0.6f);
    }

    void setPlaneWalls(List<Vector3> boundary)
    {
        planeCorners = boundary;
        var lastPoint = new Vector3();
        foreach (Vector3 boundaryPoint in boundary)
        {
            if (boundaryPoint != boundary[0])
            {
                var position = (boundaryPoint + lastPoint) / 2;
                var angleVector = boundaryPoint - lastPoint;
                var angle = Mathf.Atan2(angleVector.x, angleVector.z) * 180 / Mathf.PI + 90;
                var distance = angleVector.magnitude;
                GameObject wall = Instantiate(Wall, position, Quaternion.Euler(0f, angle, 0f));
                wall.transform.localScale += new Vector3(distance - 0.9f, 0f, 0f);
            } else
            {
                var position = (boundaryPoint + boundary[boundary.Count-1]) / 2;
                var angleVector = boundaryPoint - boundary[boundary.Count - 1];
                var angle = Mathf.Atan2(angleVector.x, angleVector.z) * 180 / Mathf.PI + 90;
                var distance = angleVector.magnitude;
                GameObject wall = Instantiate(Wall, position, Quaternion.Euler(0f, angle, 0f));
                wall.transform.localScale += new Vector3(distance - 0.9f, 0f, 0f);
            }
            lastPoint = boundaryPoint;            
        }
    }

    void placeTruck()
    {
        Instantiate(truck, new Vector3(centerPos.x, centerPos.y, centerPos.z), Quaternion.identity);
        gameHasStarted = true;
    }
    
    Vector3 calculateRandomPoint(float verticalOffset) {
        var rand = Mathf.RoundToInt(Random.Range(0, planeCorners.Count - 1));
        var A = planeCorners[rand];
        var B = planeCorners[(rand + planeCorners.Count / 2) % planeCorners.Count];

        Vector3 V = B - A;
        Vector3 point = A + Random.value * V + new Vector3(0f, verticalOffset, 0f);
        return point;
    }

    public void gameOver(int finalScore)
    {
        completeLevelUI.SetActive(true);
        finalScoreText = GameObject.Find("FinalScore").GetComponent<Text>();
        finalScoreText.text = "SCORE " + finalScore;
    }
}
