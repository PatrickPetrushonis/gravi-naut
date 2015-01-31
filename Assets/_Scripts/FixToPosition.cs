using UnityEngine;
using System.Collections;

public class FixToPosition : MonoBehaviour 
{
    private SpriteRenderer sprite;
    private GameObject placed;
    private Vector2 desiredPos;
    private Quaternion desiredRot;
    private const float precision = 0.01f;

    void Start()
    {
        sprite = transform.gameObject.GetComponent<SpriteRenderer>();
        desiredPos = transform.position;
        desiredRot = transform.rotation;
    }
    
    void FixedUpdate()
    {
        if(placed)
        {
            Vector2 currentPos = placed.transform.position;
            Vector2 moveDirection = (desiredPos - currentPos).normalized;
            Quaternion currentRot = placed.transform.rotation;

            if(Quaternion.Angle(currentRot, desiredRot) >= precision)
                placed.transform.rotation = Quaternion.Lerp(currentRot, desiredRot, Time.deltaTime);

            if(Vector3.Distance(currentPos, desiredPos) >= precision)
                placed.rigidbody2D.velocity = moveDirection;
            else 
                placed.rigidbody2D.velocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Dynamic")
        {
            if(placed == null)
            {
                placed = other.gameObject;
                placed.tag = "Untagged";
                placed.gameObject.rigidbody2D.isKinematic = true;
                if(transform.parent) sprite.enabled = false;
                GameData.data.eventCount++;
            }
        }
    }
}
