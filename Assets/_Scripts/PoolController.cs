using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//single slot identifying how many of what object to pool
[System.Serializable]
public class PoolConstructor                    
{
    public GameObject slotObject;
    public int slotAmount;
}

public class PoolController : MonoBehaviour 
{
    //location to spawn pooled objects
    public Transform spawner;

    //pool construction blueprints
    public List<PoolConstructor> toPool;

    //pool of pooled instantiated objects
    private List<List<GameObject>> pools = new List<List<GameObject>>();
    private int poolAmount;

    void Start()
    {
        if(toPool != null)
        {
            poolAmount = toPool.Count;

            for(int x = 0; x < poolAmount; x++)
            {
                //create and add a new pool to pools
                pools.Add(new List<GameObject>());

                for(int y = 0; y < toPool[x].slotAmount; y++)
                {
                    GameObject obj = (GameObject)Instantiate(toPool[x].slotObject);
                    obj.SetActive(false);
                    pools[x].Add(obj);
                }
            }
        }        
    }
    
    public void Spawn(int index)
    {
        for(int x = 0; x < pools[index].Count; x++)
        {
            if(!pools[index][x].activeInHierarchy)
            {
                pools[index][x].transform.position = spawner.transform.position;
                pools[index][x].transform.rotation = spawner.transform.rotation;
                pools[index][x].SetActive(true);
                break;
            }
        }
    }
}