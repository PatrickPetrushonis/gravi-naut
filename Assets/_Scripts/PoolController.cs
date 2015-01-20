using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolController : MonoBehaviour 
{    
    public GameObject spawner;
    public GameObject pooledObject;    
    public int pooledAmount = 3;

    private List<GameObject> pool;

    void Start()
    {
        pool = new List<GameObject>();
        for(int x = 0; x < pooledAmount; x++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public void Spawn()
    {
        for(int x = 0; x < pool.Count; x++)
        {
            if(!pool[x].activeInHierarchy)
            {
                pool[x].transform.position = spawner.transform.position;
                pool[x].transform.rotation = spawner.transform.rotation;
                pool[x].SetActive(true);
                break;
            }
        }
    }
}
