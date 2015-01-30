using UnityEngine;
using System.Collections;

public class ReactToGravity : MonoBehaviour 
{
    void FixedUpdate()
    {
        if(transform.parent == null)
            rigidbody2D.AddForce(GameData.data.gravity * GameData.data.direction);
    }
}
