using UnityEngine;
using System.Collections;

public class GravityController : MonoBehaviour 
{
    private GameObject player;
    private GameObject[] dynamic;
    private Gyroscope gyro;    
    //gravity
    public float gravity = 3f;
    public float threshold = 0.95f;
    public Vector2 direction;
    private Vector2 initial = new Vector2(0, -1);
    //velocity
    private float speedLimit = 2;
    private Vector2 playerVel;
    private Vector2 oppDirect;
    private Vector2 totalForce;
    //accelerometer
    private Vector2 currentAcc;
    private float sensitivityH = 5f;
    private float sensitivityV = 5f;
    private float smooth = 2.5f;
    private float getAxisH = 0f;
    private float getAxisV = 0f;
    //guistyle
    private GUIStyle thisStyle = new GUIStyle();

    void Start()
    {
        //GUIStyle for gravity direction notification
        thisStyle.fontSize = 20;
        thisStyle.normal.textColor = Color.white;   

        player = GameObject.Find("Player");
        dynamic = GameObject.FindGameObjectsWithTag("Dynamic");

        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            ResetAcceleration();
        }

        InitialGravity();
    }

    void FixedUpdate()
    {
        //if(gyro.enabled)
        {
            DetermineDirection();
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 25, 300, 25), "Gravity Direction (X, Y): " + direction, thisStyle);
    }

    void DetermineDirection()
    {
        currentAcc = Vector2.Lerp(currentAcc, Input.acceleration, smooth * Time.fixedDeltaTime);
        getAxisH = Mathf.Clamp(currentAcc.x * sensitivityH, -1, 1);
        getAxisV = Mathf.Clamp(currentAcc.y * sensitivityV, -1, 1);
        direction = new Vector2(getAxisH, getAxisV);

        float horizontal = Mathf.Abs(direction.x);
        float vertical = Mathf.Abs(direction.y);

        //define range in which device can be oriented without affecting gravity
        //portrait
        if(direction.y == -1 && horizontal < threshold)
        {
            direction = new Vector2(0, -1);
        }
        //upsidedown portrait
        else if(direction.y == 1 && horizontal < threshold)
        {
            direction = new Vector2(0, 1);
        }
        //left landscape
        else if(direction.x == -1 && vertical < threshold)
        {
            direction = new Vector2(-1, 0);
        }
        //right landscape
        else if(direction.x == 1 && vertical < threshold)
        {
            direction = new Vector2(1, 0);
        }

        ApplyGravity(new Vector2(direction.x, direction.y));
    }

    void ApplyGravity(Vector2 direction)
    {
        playerVel = player.rigidbody2D.velocity;
        oppDirect = Vector2.zero;

        float playerMag = playerVel.magnitude;        
        float oppForce = 0;

        if(playerMag > speedLimit)
        { 
            //add force in opposing direction equal to exceeding value over speedLimit
            oppDirect = -direction;
            oppForce = playerMag - speedLimit;
        }

        totalForce = direction * gravity + oppDirect * oppForce;
        player.rigidbody2D.AddForce(totalForce);

        foreach(GameObject dynamicObj in dynamic)
        {
            dynamicObj.rigidbody2D.AddForce(direction * gravity);
        }                
    }

    void ResetAcceleration()
    {
        currentAcc = Input.acceleration;
    }

    void InitialGravity()
    {
        player.rigidbody2D.AddForce(initial * gravity);
    }
}