using UnityEngine;
using System.Collections;

public class CollectionEvent : MonoBehaviour 
{
    private GameObject[] collectibles;
    private GameObject eventObj;

    public int totalCount;
    public int currentCount;    

    void Start()
    {        
        collectibles = GameObject.FindGameObjectsWithTag("Collectible");        
        totalCount = collectibles.Length;

        eventObj = GameObject.Find("Cage");
    }

    void Update()
    {
        if(totalCount <= currentCount)
        {
            GameObject.Destroy(eventObj);
        }
    }
}
