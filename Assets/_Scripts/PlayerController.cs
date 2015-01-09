using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    private GameObject gravityManager;
    private GravityController gravityController;
    private Vector2 direction;
    //force detection
    private Vector2 previousVel;
    private Vector2 currentVel;
    private Vector2 currentForce;
    //orientation
    private float desiredAngle;
    private float rotationSpeed = 45;
    private float minChange = 0.1f;

    void Start()
    {
        gravityManager = GameObject.Find("GravityManager");
        gravityController = gravityManager.GetComponent<GravityController>();
    }

    void FixedUpdate()
    {
        GetAppliedForce();

        //OrientationByGravityDirection();

        //OrientationByAdjecentSurface();        
    }

    void GetAppliedForce()
    {
        currentVel = rigidbody2D.velocity;

        if(currentVel != previousVel)
        {
            currentForce = currentVel - previousVel;
        }
        else 
        {
            currentForce = Vector2.zero;
        }

        previousVel = currentVel;
    }

    void OrientationByGravityDirection()
    {
        //get direction of gravity
        direction = gravityController.direction;

        float horizontal = Mathf.Abs(direction.x);
        float vertical = Mathf.Abs(direction.y);
        float threshold = 1f;
        float desiredAngle = 0;

        //define range in which device can be oriented without affecting gravity
        //portrait
        if(direction.y == -1 && horizontal < threshold)
        {
            desiredAngle = 0;
        }
        //upsidedown portrait
        else if(direction.y == 1 && horizontal < threshold)
        {
            desiredAngle = 180;
        }
        //left landscape
        else if(direction.x == -1 && vertical < threshold)
        {
            desiredAngle = -90;
        }
        //right landscape
        else if(direction.x == 1 && vertical < threshold)
        {
            desiredAngle = 90;
        }

        //determine desired angle for player orientation
        desiredAngle = Vector2.Angle(transform.position, direction);
        Debug.Log(desiredAngle);

        if(Mathf.Abs(transform.rotation.z - desiredAngle) > minChange)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, desiredAngle), Time.deltaTime * rotationSpeed);
        }
    }

    void OrientationByAdjecentSurface()
    { 
        
    }
}
