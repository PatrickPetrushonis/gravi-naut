using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    private InteractCheck interactCheck;

    public Transform groundCheck;                       //location of overlap circle for ground check    
    public LayerMask groundIdentity;                    //which layers are considered ground 

    private Vector2 direction;                          //direction of gravity upon movement request
    private Vector2 start;                              //starting position during a movement request
    private Vector2 end;                                //destination for player movement

    private bool moving = false;                        //whether a movement request is being fulfilled
    private bool grounded = false;                      //whether player is in contact with the ground
    private bool interacting = false;                   //whether an object is being interacted with
    private bool facingRight = false;                   //whether player is facing right or left

    private const float moveSpeed = 2f;                 //speed of player movement    
    private const float precision = 0.1f;               //maximum allowable offset from exact destination
    private const float rotationSpeed = 3f;             //speed of reorientation
    private const float groundCheckRadius = 0.2f;       //distance to check for ground
    private const float interactCheckRadius = 0.75f;    //distance an object can be interacted with
    private const float interactDownTime = 0.25f;       //minimum time between interactions

    private float timeSinceInteract;                    //in game time since last interaction

    void Start()
    {
        interactCheck = transform.FindChild("InteractCheck").GetComponent<InteractCheck>();
        timeSinceInteract = Time.time;

        if(transform.localScale.x < 0) facingRight = true;
    }

    void Update()
    {
        direction = GameData.data.direction;
        OrientPlayer();

        if(grounded) DetermineAction();
        else moving = false;
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundIdentity);
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

            if((Vector2.Distance(interactCheck.transform.position, end) < interactCheckRadius) & interactCheck.canInteract)
            {
                Interact();
            }
            else 
            {
                moving = true;
            }
        }
    }

    void Interact()
    {
        if(Time.time > timeSinceInteract + interactDownTime)
        {
            if(interacting)
            {
                //return object to former state, thus dropping it
                interacting = false;
                interactCheck.interactiveObject.rigidbody2D.isKinematic = false;
                interactCheck.interactiveObject.transform.parent = null;
            }
            else
            {
                //fix the object to the player
                interacting = true;
                interactCheck.interactiveObject.rigidbody2D.isKinematic = true;
                interactCheck.interactiveObject.transform.parent = transform;
                interactCheck.interactiveObject.transform.position = new Vector2(interactCheck.transform.position.x + 0.3f, interactCheck.transform.position.y);
            }

            timeSinceInteract = Time.time;
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