using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    private GameObject gravityManager;
    private GravityController gravityController;

    private Vector2 direction;          //direction of gravity upon movement request
    private Vector2 start;              //starting position during a movement request
    private Vector2 end;                //destination for player movement
    private bool moving = false;        //whether a movement request is being fulfilled

    public float moveSpeed = 1.5f;      //speed of player movement
    public float distanceToGround;      //distance to check if player is grounded
    private float precision = 0.01f;    //maximum allowable offset from exact destination

    void Start()
    {
        gravityManager = GameObject.Find("GravityManager");
        gravityController = gravityManager.GetComponent<GravityController>();

        end = transform.position;
        distanceToGround = collider2D.bounds.extents.y + 0.1f;
    }

    void Update()
    {
        //if(IsGrounded())
        {
            DetermineDestination();
            if(moving)
                MovePlayer();
        }

        ApplyGravity();
    }

    bool IsGrounded()
    {
        LayerMask mask = LayerMask.NameToLayer("Static");

        //cast ray down from current position to slightly beneath player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, distanceToGround, mask);
        
        Debug.DrawRay(transform.position, -Vector2.up, Color.red);

        if(hit.collider != null)
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            Debug.Log(hit.transform.position.ToString());
            return true;
        }
        
        Debug.Log("Not grounded");
        return false;
    }

    void DetermineDestination()
    {
        start = transform.position;

        //Set target movement position to last touch position
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            end = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            moving = true;

            //determines gravity direction
            direction = gravityController.direction;

            //limits movement to prevent undesired levitation
            if(direction.y != 0 && direction.x < 1)
                end.y = transform.position.y;
            else if(direction.x != 0 && direction.y < 1)
                end.x = transform.position.x;
        }
    }

    void MovePlayer()
    {
        //to prevent jittering, check to see if current position is within acceptable bounds of end position
        if(start.x > end.x - precision && start.x < end.x + precision)
            if(start.y > end.y - precision && start.y < end.y + precision)
            {
                EndMovement();
                return;
            }

        Vector2 moveDirection = (end - start).normalized;
        rigidbody2D.velocity = moveDirection * moveSpeed;
    }

    void EndMovement()
    {
        end = start;
        rigidbody2D.velocity = Vector2.zero;
        moving = false;
    }

    void ApplyGravity()
    {
        rigidbody2D.AddForce(gravityController.totalForce);

        //sudden change in gravity will prevent further movement
        if(moving)
            if(gravityController.direction.x != direction.x || gravityController.direction.y != direction.y)
                EndMovement();
    }
}
