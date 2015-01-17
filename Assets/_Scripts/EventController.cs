using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour 
{
    private GameObject[] collectibles;
    private GameObject eventObject;
    private string objectName;    

    void Start()
    {
        collectibles = GameObject.FindGameObjectsWithTag("Collectible");
        GameController.gameControl.totalCollectibles = collectibles.Length;

        if(Application.loadedLevel == 0)
            objectName = "Cage";

        eventObject = GameObject.Find(objectName);
    }

    void Update()
    {
        if(GameController.gameControl.currentCollectible >= GameController.gameControl.totalCollectibles)
        {
            if(Application.loadedLevel == 0)
                Destroy(eventObject);
        } 
    }
}
