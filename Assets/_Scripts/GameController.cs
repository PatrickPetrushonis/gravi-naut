using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    //Controls vital non-specific features of the game:
    //Pause Game
    //Edit Game Options
    //Reset Current Level
    //Load Next Level
    //Close Game
    //Tracks Progress

    public static GameController gameControl;

    public int currentCollectible;
    public int totalCollectibles;

    void Awake()
    {
        if(gameControl == null)
        {
            DontDestroyOnLoad(gameObject);
            gameControl = this;
        }
        else if(gameControl != this)
        {
            Destroy(gameObject);
        }
    }
}
