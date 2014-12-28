using UnityEngine;
using System.Collections;

public class GravityController : MonoBehaviour 
{
    private GameObject player;
    private Gyroscope gyro;    
    //gravity
    public float gravity = 10f;
    public Vector2 direction;
    //accelerometer
    private Vector2 currentAcc;
    private float sensitivityH = 5f;
    private float sensitivityV = 5f;
    private float smooth = 0.5f;
    private float getAxisH = 0f;
    private float getAxisV = 0f;
    //guistyle
    private GUIStyle thisStyle = new GUIStyle();

    void Start()
    {
        player = GameObject.Find("Player");

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
        if (gyro.enabled)
        {
            currentAcc = Vector2.Lerp(currentAcc, Input.acceleration, Time.deltaTime / smooth);
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
        //need certain range in which player can freely move without changing gravity

        player.rigidbody2D.AddForce(direction * gravity);
    }

    void ResetAcceleration()
    {
        currentAcc = Input.acceleration;
    }
}