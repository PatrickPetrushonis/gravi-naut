using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    private Camera thisCamera;
    private GameObject player;
    private Transform target;

    private Vector3 velocity = Vector3.zero;
    public float dampTime = 0.25f;
    public float orthoSize = 2.75f;

    void Start()
    {
        thisCamera = Camera.main;
        thisCamera.orthographicSize = orthoSize;

        player = GameObject.Find("Player");
        target = player.transform;
    }

    void LateUpdate()
    {
        if(target)
        {
            Vector3 point = thisCamera.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - thisCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }
}