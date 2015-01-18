using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour 
{
    public static GameData data;

    public int currentLevel;
    public int collectibleCount;
    public int collectibleTotal;
    public float volume;

    public static float width = 400;
    public static float height = 100;
    public static float margin = 10;
    public static float leftIndent = (Screen.width - width) / 2;
    public static float topIndent = (Screen.height - height) / 2;
    public static Rect pauseButton = new Rect(margin, margin, 75, 75);

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
