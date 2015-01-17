using UnityEngine;
using System.Collections;

public class UIData : MonoBehaviour 
{
    public static UIData ui;

    public static float width = 400;
    public static float height = 100;
    public static float margin = 10;
    public static float leftIndent = (Screen.width - width) / 2;
    public static float topIndent = (Screen.height - height) / 2;
    public static Rect pauseButton = new Rect(margin, margin, 75, 75);
    
    void Awake()
    {
        if(ui == null)
        {
            DontDestroyOnLoad(gameObject);
            ui = this;
        }
        else if(ui != this)
        {
            Destroy(gameObject);
        }
    }
}
