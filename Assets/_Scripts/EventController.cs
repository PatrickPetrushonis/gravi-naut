using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour 
{
    private GameObject[] collectibles;
    private GameObject eventObject;
    private string objectName;    

    void Start()
    {
        if(Application.loadedLevel == 0)
            objectName = "Cage";

        eventObject = GameObject.Find(objectName);
    }

    void Update()
    {
        if(GameData.data.collectibleCount >= GameData.data.collectibleTotal)
        {
            if(Application.loadedLevel == 0)
                Destroy(eventObject);
        } 
    }
}
