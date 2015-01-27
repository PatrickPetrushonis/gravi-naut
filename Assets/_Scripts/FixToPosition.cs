using UnityEngine;
using System.Collections;

public class FixToPosition : MonoBehaviour 
{
    private GameObject placed;
    private Vector2 end;
    private const float precision = 0.1f;

    void Start()
    {
        end = transform.position;  
    }
    
    void FixedUpdate()
    {
        if(placed)
        {
            Vector2 start = placed.transform.position;
            Vector2 moveDirection = (end - start).normalized;

            if(Vector3.Distance(start, end) >= precision)
            {
                placed.rigidbody2D.velocity = moveDirection;
                //placed.transform.rotation = Quaternion.Lerp(placed.transform.rotation, transform.rotation, Time.deltaTime);
            }
            else placed.rigidbody2D.velocity = Vector2.zero;
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
                GameData.data.eventCount++;
            }
        }
    }
}
