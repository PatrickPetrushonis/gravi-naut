using UnityEngine;
using System.Collections;

public class GravityController : MonoBehaviour 
{
    private Gyroscope gyro;

    private Vector2 direction;              //current direction of gravity
    private Vector2 currentAcc;             //current acceleration of device calculated with Lerp 

    private float getAxisH = 0f;            //current horizontal and vertical axes input
    private float getAxisV = 0f;

    private const float gravity = 5f;       //force of gravity
    private const float threshold = 0.9f;   //minimum input required to effect gravity
    private const float sensitivityH = 5f;  //sensitivity of horizontal and vertical axes
    private const float sensitivityV = 5f;
    private const float smooth = 2.5f;      //smoothing factor in Lerp calculation

    public bool down = true;                //directions in which gravity can be applied
    public bool up = true;                              
    public bool left = true;
    public bool right = true;

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
        LimitDirection();
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
        if(direction.y == -1 && horizontal < threshold)         direction = new Vector2(0, -1);     //left landscape
        else if(direction.y == 1 && horizontal < threshold)     direction = new Vector2(0, 1);      //right landscape
        else if(direction.x == -1 && vertical < threshold)      direction = new Vector2(-1, 0);     //portrait
        else if(direction.x == 1 && vertical < threshold)       direction = new Vector2(1, 0);      //upside-down portrait
        else direction = new Vector2(direction.x, direction.y);
    }

    void LimitDirection()
    {
        if(down && up && left && right)             //gravity in all directions
            return;
        else if(!down && !up && !left && !right)    //no gravity
            direction = Vector2.zero;
        else
        {
            if(!down)   //down direction or Vector2(0, -1)
            {
                if(direction.y < 0 && !up) direction.y = 0;
                else direction.y = 1;
            }
            if(!up)     //up direction or Vector2(0, 1)
            {
                if(direction.y > 0 && !down) direction.y = 0;
                else direction.y = -1;
            }
            if(!left)   //left direction or Vector2(-1, 0)
            {
                if(direction.x < 0 && !right) direction.x = 0;
                else direction.x = 1;
            }
            if(!right)  //right direction or Vector2(1, 0)
            {
                if(direction.x > 0 && !left) direction.x = 0;
                else direction.x = -1;
            }
        } 
    }
}