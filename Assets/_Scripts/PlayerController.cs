using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    private GameObject gravityManager;
    private GravityController gravityController;

    private Vector2 direction;                      //direction of gravity upon movement request
    private Vector2 start;                          //starting position during a movement request
    private Vector2 end;                            //destination for player movement
    private bool moving = false;                    //whether a movement request is being fulfilled

    private float distanceToGround;                 //distance to check if player is grounded
    private const float moveSpeed = 1.5f;           //speed of player movement    
    private const float precision = 0.1f;           //maximum allowable offset from exact destination
    private const float rotationSpeed = 12.5f;      //speed of reorientation

    void Start()
    {
        gravityManager = GameObject.Find("GravityManager");
        gravityController = gravityManager.GetComponent<GravityController>();

        end = transform.position;
        distanceToGround = collider2D.bounds.extents.y + 0.1f;
    }

    void FixedUpdate()
    {
        direction = gravityController.direction;

        if(IsGrounded())
        {
            DetermineDestination();
            if(moving) MovePlayer();
        }
        else moving = false;

        OrientPlayer();
        ApplyGravity();
    }    

    bool IsGrounded()
    {
        //cast ray down from current position to slightly beneath player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, distanceToGround);
        if(hit.collider != null) return true;         
        else return false;
    }

    void DetermineDestination()
    {
        start = transform.position;

        //Set target movement position to last touch position
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            end = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            moving = true;

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

    void OrientPlayer()
    {
        Quaternion desiredOrient = DesiredOrientation();

        //rotate player from current rotation to new orientation
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredOrient, Time.deltaTime * rotationSpeed);
    }

    Quaternion DesiredOrientation()
    {
        Quaternion orientation = new Quaternion();

        //determine orientation by direction of gravity
        if(direction.x == 0 && direction.y == -1)           //portrait
            orientation = Quaternion.Euler(0, 0, 0);
        else if(direction.x == 0 && direction.y == 1)       //upside-down portrait
            orientation = Quaternion.Euler(0, 0, 180);

        else if(direction.x == -1 && direction.y == -1)     //between portrait & left landscape
            orientation = Quaternion.Euler(0, 0, -45);
        else if(direction.x == 1 && direction.y == -1)      //between portrait & right landscape 
            orientation = Quaternion.Euler(0, 0, 45);

        else if(direction.x == -1 && direction.y == 0)      //left landscape
            orientation = Quaternion.Euler(0, 0, -90);
        else if(direction.x == 1 && direction.y == 0)       //right landscape
            orientation = Quaternion.Euler(0, 0, 90);

        else if(direction.x == -1 && direction.y == 1)      //between upside-down portrait & left landscape
            orientation = Quaternion.Euler(0, 0, -135);
        else if(direction.x == 1 && direction.y == 1)       //between upside-down portrait & right landscape
            orientation = Quaternion.Euler(0, 0, 135);

        return orientation;
    }

    void ApplyGravity()
    {
        rigidbody2D.AddForce(gravityController.totalForce);

        //sudden change in gravity will prevent further movement
        if(moving)
            if(gravityController.direction.x != direction.x || gravityController.direction.y != direction.y)
                EndMovement();
    }    

    void EndMovement()
    {
        end = start;
        rigidbody2D.velocity = Vector2.zero;
        moving = false;
    }
}