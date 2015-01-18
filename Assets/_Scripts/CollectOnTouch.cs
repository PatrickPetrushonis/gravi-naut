using UnityEngine;
using System.Collections;

public class CollectOnTouch : MonoBehaviour 
{
    private Collider2D thisCollider;

    void Start()
    {        
        thisCollider = gameObject.GetComponent<Collider2D>();

        if(!thisCollider.isTrigger)
            thisCollider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameData.data.collectibleCount++;
            Destroy(gameObject);
        }
    }
}
