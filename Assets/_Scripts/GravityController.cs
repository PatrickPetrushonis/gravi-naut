using UnityEngine;
using System.Collections;

public class GravityController : MonoBehaviour 
{
    private Gyroscope gyro;

    private Vector2 direction;              //current direction of gravity
    private Vector2 currentAcc;             //current acceleration of device calculated with Lerp 

    private float getAxisH = 0f;            //current horizontal and vertical axes input
    private float getAxisV = 0f;

    private const float gravity = 3f;       //force of gravity
    private const float threshold = 0.9f;  //minimum input required to effect gravity
    private const float sensitivityH = 5f;  //sensitivity of horizontal and vertical axes
    private const float sensitivityV = 5f;
    private const float smooth = 2.5f;      //smoothing factor in Lerp calculation
    
    void Start()
    {
        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            currentAcc = Input.acceleration;
        }
    }

    void FixedUpdate()
    {
        DetermineDirection();
        GameData.data.gravity = gravity;
        GameData.data.direction = direction;
    }

    void DetermineDirection()
    {
        //device acceleration over time
        currentAcc = Vector2.Lerp(currentAcc, Input.acceleration, smooth * Time.fixedDeltaTime);
        getAxisH = Mathf.Clamp(currentAcc.x * sensitivityH, -1, 1);
        getAxisV = Mathf.Clamp(currentAcc.y * sensitivityV, -1, 1);

        //direction
        direction = new Vector2(getAxisH, getAxisV);
        float horizontal = Mathf.Abs(direction.x);
        float vertical = Mathf.Abs(direction.y);        

        //define range in which device can be oriented without affecting gravity
        if(direction.y == -1 && horizontal < threshold)         direction = new Vector2(0, -1);     //portrait
        else if(direction.y == 1 && horizontal < threshold)     direction = new Vector2(0, 1);      //upside-down portrait
        else if(direction.x == -1 && vertical < threshold)      direction = new Vector2(-1, 0);     //left landscape
        else if(direction.x == 1 && vertical < threshold)       direction = new Vector2(1, 0);      //right landscape
        else direction = new Vector2(direction.x, direction.y);
    }
}