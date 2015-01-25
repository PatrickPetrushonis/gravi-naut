using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour 
{
    private GameController control;
    private PoolController pool;
    private ReactToPlayer react;    

    public GameObject[] collectibles;
    public GameObject[] eventTriggers;
    public GameObject obstacle;
    public GameObject exit;
    public bool triggered = false;

    void Start()
    {
        control = GameObject.Find("GameManager").GetComponent<GameController>();
        pool = GameObject.Find("PoolManager").GetComponent<PoolController>();
        react = exit.GetComponent<ReactToPlayer>();

        if(collectibles != null) GameData.data.collectibleTotal = collectibles.Length;
        if(eventTriggers != null) GameData.data.eventTotal = eventTriggers.Length;

        GameData.data.collectibleCount = 0;
        GameData.data.eventCount = 0;

        exit.SetActive(false);
    }

    void Update()
    {
        if(GameData.data.collectibleCount >= GameData.data.collectibleTotal)
        {
            if(!triggered)
            {
                triggered = true;
                StartCoroutine(Spawn(0));
            }

            if(GameData.data.eventCount >= GameData.data.eventTotal)
            {
                if(obstacle.activeSelf == true) obstacle.SetActive(false);
                GameData.data.complete = true;
            }
            else GameData.data.objective = "Insert the power cube into the conduit.";
        }
        else GameData.data.objective = "Collect the rouge energy.";

        if(GameData.data.complete)
        {
            exit.SetActive(true);
            if(react.playerTrigger) control.LoadGame(GameData.data.currentLevel + 1);
            GameData.data.objective = "Proceed to next level.";
        }
    }

    IEnumerator Spawn(int index)
    {
        float amount = pool.toPool[index].slotAmount;

        for(int x = 0; x < amount; x++)
        {
            pool.Spawn(index);
            yield return new WaitForSeconds(1.5f);
        }

        yield return null;
    }
}
