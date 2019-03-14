using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTruck : MonoBehaviour
{
    public GameObject gameHandler;
    private Pose playerPointer;
    private Rigidbody truckRB;
    public float standardSpeed = 2f;
    private float speed = 0;
    public float standardTurnSpeed = 8f;
    private float turnSpeed = 0;
    private int turnDirection = 0;
    private int reverseValue = 1;
    private float waitTime = 1.5f;
    private bool hasStarted = false;

    void Awake()
    {
        truckRB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        gameHandler = GameObject.Find("Game Handler");
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPointer = gameHandler.GetComponent<MovePointer>().getPlayerPointer();
        calculatePath();

        if(!hasStarted)
        {
            waitTime -= Time.deltaTime;
        }
        if (waitTime < 0)
        {
            hasStarted = true;
        }

        if (Input.touchCount > 0) {
            reverse(true);
        } else
        {
            reverse(false);
        }
    }

    private void FixedUpdate()
    {
        if(!hasStarted)
        {
            return;
        }
        truckRB.AddRelativeForce(0f, 0f, speed);
        truckRB.AddRelativeTorque(transform.up*turnSpeed * turnDirection, ForceMode.Force);
    }

    void calculatePath()
    {
        var heading = gameObject.transform.position - playerPointer.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        var angleBetween = 180 - Vector3.Angle(heading, gameObject.transform.forward);
        if (distance > 0.1f)
        {
            speed = standardSpeed * reverseValue;
        }
        else
        {
            speed = 0f;
        }

        turnDirection = determineTurnDirection();
        if(angleBetween > 30f)
        {
            turnSpeed = standardTurnSpeed;
        } else if (angleBetween > 15f)
        {
            turnSpeed = standardTurnSpeed * angleBetween / 30f;
        } else
        {
            turnSpeed = 0f;
        }

    }

    private int determineTurnDirection()
    {
        var relativePoint = transform.InverseTransformPoint(playerPointer.position);
        if (relativePoint.x < 0.0)
        {
            //print("Object is to the left");
            return -1;
        }
        else if (relativePoint.x > 0.0)
        {
            //print("Object is to the right");
            return 1;
        }
        else
        {
            //print("Object is directly ahead");
            return 0;
        }
    }
    void reverse(bool reverse)
    {
        if (reverse)
        {
            reverseValue = -1;
        } else
        {
            reverseValue = 1;
        }
    }
}
