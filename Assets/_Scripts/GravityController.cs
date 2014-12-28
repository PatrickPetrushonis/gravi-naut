using UnityEngine;
using System.Collections;

public class GravityController : MonoBehaviour 
{
    private GameObject player;
    private GameObject[] dynamic;
    private Gyroscope gyro;    
    //gravity
    public float gravity = 10f;
    public float threshold = 0.6f;
    public Vector2 direction;
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
        player = GameObject.Find("Player");
        dynamic = GameObject.FindGameObjectsWithTag("Dynamic");

        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            ResetAcceleration();
        }

        //GUIStyle for gravity direction notification
        thisStyle.fontSize = 20;
        thisStyle.normal.textColor = Color.white;
    }

    void FixedUpdate()
    {
        if(gyro.enabled)
        {
            currentAcc = Vector2.Lerp(currentAcc, Input.acceleration, smooth * Time.deltaTime);
            getAxisH = Mathf.Clamp(currentAcc.x * sensitivityH, -1, 1);
            getAxisV = Mathf.Clamp(currentAcc.y * sensitivityV, -1, 1);
            direction = new Vector2(getAxisH, getAxisV);

            ApplyGravity();
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 25, 300, 25), "Gravity Direction (X, Y): " + direction, thisStyle);
    }

    void ApplyGravity()
    {        
        float horizontal = Mathf.Abs(direction.x);
        float vertical = Mathf.Abs(direction.y);

        //define range in which device can be oriented without affecting gravity
        if((horizontal < threshold && vertical == 1) || (vertical < threshold && horizontal == 1))
        {
            return;
        }
        else 
        {
            player.rigidbody2D.AddForce(direction * gravity);

            foreach(GameObject dynamicObj in dynamic)
            {
                dynamicObj.rigidbody2D.AddForce(direction * gravity);
            }
        }        
    }

    void ResetAcceleration()
    {
        currentAcc = Input.acceleration;
    }
}