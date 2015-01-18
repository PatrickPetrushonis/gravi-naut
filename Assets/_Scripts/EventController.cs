using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour 
{
    public GameObject gameManager;
    private GameController control;

    public GameObject[] eventObjects = new GameObject[2];

    void Start()
    {
        control = gameManager.GetComponent<GameController>();

        GameData.data.collectibleTotal = GameObject.FindGameObjectsWithTag("Collectible").Length;
        GameData.data.eventTotal = GameObject.FindGameObjectsWithTag("Event").Length;

        GameData.data.collectibleCount = 0;
        GameData.data.eventCount = 0;
    }

    void Update()
    {
        if(GameData.data.collectibleCount >= GameData.data.collectibleTotal)
        {
            if(GameData.data.currentLevel == 1)
            {
                Destroy(eventObjects[0]);
            }                
        }

        if(GameData.data.eventCount >= GameData.data.eventTotal)
        {
            control.complete = true;
        }
        else
        {
            control.complete = false;
        }
    }
}
