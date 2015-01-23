using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour 
{
    private PoolController pool;

    public GameObject[] collectibles;
    public GameObject[] eventTriggers;
    public bool triggered = false;

    void Start()
    {
        pool = GameObject.Find("PoolManager").GetComponent<PoolController>();

        if(collectibles != null) GameData.data.collectibleTotal = collectibles.Length;
        if(eventTriggers != null) GameData.data.eventTotal = eventTriggers.Length;

        GameData.data.collectibleCount = 0;
        GameData.data.eventCount = 0;        
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
