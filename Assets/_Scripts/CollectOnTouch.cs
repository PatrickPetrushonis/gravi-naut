using UnityEngine;
using System.Collections;

public class CollectOnTouch : MonoBehaviour 
{
    private Collider2D thisCollider;
    private GameObject eventManager;
    private CollectionEvent collectionEvent;

    void Start()
    {
        thisCollider = gameObject.GetComponent<Collider2D>();
        eventManager = GameObject.Find("EventManager");
        collectionEvent = eventManager.GetComponent<CollectionEvent>();
        
        if(!thisCollider.isTrigger)
        {
            thisCollider.isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            collectionEvent.currentCount++;
            Destroy(gameObject);
        }
    }
}
