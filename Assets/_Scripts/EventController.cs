using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour 
{
    public GameObject[] eventObjects;

    private PoolController pool;
    private bool eventTrigger = false;

    void Start()
    {
        pool = GameObject.Find("PoolManager").GetComponent<PoolController>();

        GameData.data.collectibleTotal = GameObject.FindGameObjectsWithTag("Collectible").Length;
        GameData.data.eventTotal = GameObject.FindGameObjectsWithTag("Event").Length;

        GameData.data.collectibleCount = 0;
        GameData.data.eventCount = 0;
    }

    void Update()
    {
        if(GameData.data.collectibleCount >= GameData.data.collectibleTotal)
        {
            if(GameData.data.currentLevel == 1 && !eventTrigger)
            {
                eventTrigger = true;
                StartCoroutine(Spawn());
            }                
        }

        if(GameData.data.eventCount >= GameData.data.eventTotal)
            GameData.data.complete = true;
        else
            GameData.data.complete = false;
    }

    IEnumerator Spawn()
    {
        for(int x = 0; x < pool.pooledAmount; x++)
        {
            pool.Spawn();
            yield return new WaitForSeconds(1.5f);
        }

        yield return null;
    }
}
