using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour 
{
    private GameController control;
    private GUIStyle titleStyle = new GUIStyle();
    private GUIStyle labelStyle = new GUIStyle();

    private const float width = 400;
    private const float height = 100;
    private const float margin = 10;
    private float leftIndent = (Screen.width - width) / 2;
    private float topIndent = (Screen.height - height) / 2;
    private Rect pauseButton = new Rect(margin, margin, 75, 75);

    private bool setting = false;

    void Start()
    {
        control = GameObject.Find("GameManager").GetComponent<GameController>();

        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontSize = 48;
        titleStyle.normal.textColor = Color.white;

        labelStyle.alignment = TextAnchor.MiddleLeft;
        labelStyle.fontSize = 36;
        labelStyle.normal.textColor = Color.white;
    }

    void OnGUI()
    {
        if(GameData.data.currentLevel == 0)
            MainMenu();
        else
        {
            if(GameData.data.pause) 
                PauseMenu();                
            else
            {
                if(GUI.Button(pauseButton, "")) 
                    GameData.data.pause = true;

                if(GameData.data.complete)
                    GUI.Label(new Rect(leftIndent, topIndent - (height + margin), width, height), "You Win!", titleStyle);
            }
        }
    }

    void MainMenu()
    {
        if(setting)
        {
            GUI.Label(new Rect(leftIndent, topIndent - (height + margin), width, height), "Settings", titleStyle);

            GUI.Label(new Rect(leftIndent, topIndent, width, height), "Volume", labelStyle);

            GameData.data.volume = GUI.HorizontalSlider(new Rect(leftIndent + width, topIndent, width, height), GameData.data.volume, 0, 1);

            if(GUI.Button(new Rect(leftIndent, topIndent + (height + margin), width, height), "Return"))
                setting = false;
        }
        else if(GameData.data.quit)
        {
            GUI.Label(new Rect(leftIndent, topIndent - (height + margin), width, height), "Close Application?", titleStyle);

            if(GUI.Button(new Rect(leftIndent, topIndent, width, height), "Yes"))
                control.LoadGame(-1);
            if(GUI.Button(new Rect(leftIndent, topIndent + (height + margin), width, height), "No"))
                GameData.data.quit = false;
        }
        else
        {
            GUI.Label(new Rect(leftIndent, topIndent - (height + margin) * 2, width, height), "Gravinaut", titleStyle);

            if(GUI.Button(new Rect(leftIndent, topIndent - (height + margin), width, height), "Start"))
                control.LoadGame(1);
            if(GUI.Button(new Rect(leftIndent, topIndent, width, height), "Settings"))
                setting = true;
            if(GUI.Button(new Rect(leftIndent, topIndent + (height + margin), width, height), "Quit"))
                GameData.data.quit = true;
        }
    }

    void PauseMenu()
    {
        if(GameData.data.quit)
        {
            GUI.Label(new Rect(leftIndent, topIndent - (height + margin), width, height), "Are You Sure?", titleStyle);

            if(GUI.Button(new Rect(leftIndent, topIndent, width, height), "Yes", labelStyle))
                control.LoadGame(0);
            if(GUI.Button(new Rect(leftIndent, topIndent + (height + margin), width, height), "No"))
                GameData.data.quit = false;
        }
        else 
        {
            if(GUI.Button(new Rect(leftIndent, topIndent - (height + margin), width, height), "Resume"))
                GameData.data.pause = false;
            if(GUI.Button(new Rect(leftIndent, topIndent, width, height), "Restart"))
                control.LoadGame(GameData.data.currentLevel);
            if(GUI.Button(new Rect(leftIndent, topIndent + (height + margin), width, height), "Main Menu"))
                GameData.data.quit = true;
        }
    }
}
