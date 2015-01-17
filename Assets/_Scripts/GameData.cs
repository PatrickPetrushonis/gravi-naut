using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour 
{
    public static GameData data;

    public int currentLevel;
    public int collectibleCount;
    public int collectibleTotal;

    void Awake()
    {
        if(data == null)
        {
            DontDestroyOnLoad(gameObject);
            data = this;
        }
        else if(data != this)
        {
            Destroy(gameObject);
        }
    }
}
