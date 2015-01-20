using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    public Transform groundCheck;                   //location of overlap circle for ground check
    public LayerMask groundIdentity;                //which layers are considered ground 

    private Vector2 direction;                      //direction of gravity upon movement request
    private Vector2 start;                          //starting position during a movement request
    private Vector2 end;                            //destination for player movement

    private bool moving = false;                    //whether a movement request is being fulfilled
    private bool grounded = false;                  //whether player is in contact with the ground
    private bool facingRight = true;                //whether player is facing right or left

    private const float moveSpeed = 1.5f;           //speed of player movement    
    private const float precision = 0.1f;           //maximum allowable offset from exact destination
    private const float rotationSpeed = 3f;         //speed of reorientation
    private const float checkRadius = 0.2f;         //distance to check for ground

    void Update()
    {
        direction = GameData.data.direction;
        OrientPlayer();

        if(grounded) 
            DetermineAction();
        else 
            moving = false;
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundIdentity);
        rigidbody2D.AddForce(GameData.data.gravity * GameData.data.direction);
        if(moving) Move();
    } 

    void DetermineAction()
    {
        start = transform.position;

        //Set target movement position to last touch position
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            end = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            moving = true;        
        }
    }

    void Move()
    {
        Vector2 moveDirection = MoveDirection();
        
        //to prevent jittering, check to see if current position is within acceptable bounds of end position
        if(Vector2.Distance(start, end) <= precision)
        {
            EndMovement();
            return;
        }

        if(facingRight)
        {
            if((direction.y == -1 && moveDirection.x == -1) || (direction.y == 1 && moveDirection.x == 1) || (direction.x == -1 && moveDirection.y == 1) || (direction.x == 1 && moveDirection.y == -1))
                Flip();
        }
        else if(!facingRight)
        {
            if((direction.y == -1 && moveDirection.x == 1) || (direction.y == 1 && moveDirection.x == -1) || (direction.x == -1 && moveDirection.y == -1) || (direction.x == 1 && moveDirection.y == 1))
                Flip();
        }

        rigidbody2D.velocity = moveDirection * moveSpeed;
    }

    Vector2 MoveDirection()
    {
        //float horizontal = Mathf.Abs(direction.x);
        //float vertical = Mathf.Abs(direction.y);

        //limits movement to prevent undesired levitation
        if(direction.y != 0 && direction.x < 1) end.y = transform.position.y;
        if(direction.x != 0 && direction.y < 1) end.x = transform.position.x;

        Vector2 moveDirection = (end - start).normalized;
        return moveDirection;
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x = -theScale.x;
        transform.localScale = theScale;
    }

    void OrientPlayer()
    {
        //calculate the difference between global up and desired direction
        float angle = Vector3.Angle(Vector3.up, -direction);

        //invert angle when gravity along x-axis is negative
        if(direction.x < 0) angle = -angle;

        Quaternion orientation = Quaternion.Euler(0, 0, angle);

        //rotate player from current rotation to new orientation
        transform.rotation = Quaternion.Lerp(transform.rotation, orientation, Time.deltaTime * rotationSpeed);
    } 

    void EndMovement()
    {
        end = start;
        rigidbody2D.velocity = Vector2.zero;
        moving = false;
    }
}