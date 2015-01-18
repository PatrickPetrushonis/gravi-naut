using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour 
{
    public GameObject gameManager;
    private GameController control;
    public GUIStyle titleStyle = new GUIStyle();

    private const float width = 400;
    private const float height = 100;
    private const float margin = 10;
    private float leftIndent = (Screen.width - width) / 2;
    private float topIndent = (Screen.height - height) / 2;
    private Rect pauseButton = new Rect(margin, margin, 75, 75);

    void Start()
    {
        control = gameManager.GetComponent<GameController>();

        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontSize = 48;
        titleStyle.normal.textColor = Color.white;
    }

    void OnGUI()
    {
        if(GameData.data.currentLevel == 0)
            MainMenu();
        else
        {
            if(control.paused)
                PauseMenu();
            else
            {
                if(GUI.Button(pauseButton, ""))
                    control.PauseGame();

                if(control.complete)
                {
                    GUI.Label(new Rect(leftIndent, topIndent - (height + margin), width, height), "You Win!", titleStyle);
                }
                    
            }
        }
    }

    void MainMenu()
    {
        if(control.setting)
        {
            GUI.Label(new Rect(leftIndent, topIndent - (height + margin), width, height), "Volume", titleStyle);

            GameData.data.volume = GUI.HorizontalSlider(new Rect(leftIndent, topIndent, width, height), GameData.data.volume, 0, 1);

            if(GUI.Button(new Rect(leftIndent, topIndent + (height + margin), width, height), "Return"))
                control.Setting();
        }
        else if(control.quitting)
        {
            GUI.Label(new Rect(leftIndent, topIndent - (height + margin), width, height), "Close Application?", titleStyle);

            if(GUI.Button(new Rect(leftIndent, topIndent, width, height), "Yes"))
                control.CloseGame();
            if(GUI.Button(new Rect(leftIndent, topIndent + (height + margin), width, height), "No"))
                control.quitting = false;
        }
        else
        {
            if(GUI.Button(new Rect(leftIndent, topIndent - (height + margin), width, height), "Start"))
                control.LoadGame(1);
            if(GUI.Button(new Rect(leftIndent, topIndent, width, height), "Settings"))
                control.Setting();
            if(GUI.Button(new Rect(leftIndent, topIndent + (height + margin), width, height), "Quit"))
                control.quitting = true;
        }
    }

    void PauseMenu()
    {
        if(GUI.Button(new Rect(leftIndent, topIndent - (height + margin), width, height), "Resume"))
            control.ResumeGame();
        if(GUI.Button(new Rect(leftIndent, topIndent, width, height), "Restart"))
            control.ResetLevel();
        if(GUI.Button(new Rect(leftIndent, topIndent + (height + margin), width, height), "Main Menu"))
            control.LoadGame(0);
    }
}
