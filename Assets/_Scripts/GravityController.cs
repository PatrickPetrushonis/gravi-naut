using UnityEngine;
using System.Collections;

public class GravityController : MonoBehaviour 
{
    private GameObject player;
    private GameObject[] dynamic;       //all non-player objects affected by gravity
    private Gyroscope gyro;

    public float gravity = 3f;          //force of gravity
    private float threshold = 0.95f;    //minimum input required to effect gravity

    public Vector2 direction;           //current direction of gravity
    private float horizontal;           //absolute value for x and y axes of direction
    private float vertical;              

    private Vector2 playerVel;          //current player velocity
    private float playerMag;            //magnitude of current velocity
    private float speedLimit = 2;       //maximum total velocity
    private float opposeForce;          //difference between current velocity magnitude and speed limit
    private Vector2 opposeDir;          //opposite direction of current force of gravity
    public Vector2 totalForce;          //total combination of all forces to act on player

    private Vector2 currentAcc;         //current acceleration of device calculated with Lerp
    private float sensitivityH = 5f;    //sensitivity of horizontal and vertical axes
    private float sensitivityV = 5f;
    private float smooth = 2.5f;        //smoothing factor in Lerp calculation
    private float getAxisH = 0f;        //current horizontal and vertical axes input
    private float getAxisV = 0f;
    
    void Start()
    {
        player = GameObject.Find("Player");
        dynamic = GameObject.FindGameObjectsWithTag("Dynamic");

        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            currentAcc = Input.acceleration;
        }

        //initial force of gravity
        if(player) player.rigidbody2D.AddForce(new Vector2(0, -1) * gravity);
    }

    void FixedUpdate()
    {
        DetermineDirection();
        ApplyGravity(new Vector2(direction.x, direction.y));
    }

    void DetermineDirection()
    {
        //device acceleration over time
        currentAcc = Vector2.Lerp(currentAcc, Input.acceleration, smooth * Time.fixedDeltaTime);
        getAxisH = Mathf.Clamp(currentAcc.x * sensitivityH, -1, 1);
        getAxisV = Mathf.Clamp(currentAcc.y * sensitivityV, -1, 1);

        //direction
        direction = new Vector2(getAxisH, getAxisV);
        horizontal = Mathf.Abs(direction.x);
        vertical = Mathf.Abs(direction.y);        

        //define range in which device can be oriented without affecting gravity
        //portrait
        if(direction.y == -1 && horizontal < threshold)
            direction = new Vector2(0, -1);
        //upsidedown portrait
        else if(direction.y == 1 && horizontal < threshold)
            direction = new Vector2(0, 1);
        //left landscape
        else if(direction.x == -1 && vertical < threshold)
            direction = new Vector2(-1, 0);
        //right landscape
        else if(direction.x == 1 && vertical < threshold)
            direction = new Vector2(1, 0);
    }

    void ApplyGravity(Vector2 direction)
    {
        if(player)
        {
            playerVel = player.rigidbody2D.velocity;
            playerMag = playerVel.magnitude;

            opposeDir = Vector2.zero;
            opposeForce = 0;

            if(playerMag > speedLimit)
            {
                //calculate difference between player magnitude and limit
                opposeDir = -direction;
                opposeForce = playerMag - speedLimit;
            }

            //overall force to be applied to character
            //actual application is performed in CharacterController
            totalForce = direction * gravity + opposeDir * opposeForce;
        }

        //to reduce calculations, non-player objects have no limit on velocity
        foreach(GameObject dynamicObj in dynamic)
            dynamicObj.rigidbody2D.AddForce(direction * gravity);
    }
}