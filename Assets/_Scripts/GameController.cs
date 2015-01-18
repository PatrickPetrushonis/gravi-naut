using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    private GameObject gravityManager;
    private GameObject player;    
    private GUIStyle titleStyle = new GUIStyle();

    private bool paused = false;
    private bool setting = false;
    private bool quitting = false;

    void Start()
    {
        GameData.data.currentLevel = Application.loadedLevel;
        gravityManager = GameObject.Find("GravityManager");
        if(player) player = GameObject.Find("Player");

        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontSize = 48;
        titleStyle.normal.textColor = Color.white;
    }

    void OnGUI()
    {
        if(GameData.data.currentLevel == 0)
        {
            if(setting)
            {
                GUI.Label(new Rect(GameData.leftIndent, GameData.topIndent - (GameData.height + GameData.margin), GameData.width, GameData.height), "Volume", titleStyle);

                GameData.data.volume = GUI.HorizontalSlider(new Rect(GameData.leftIndent, GameData.topIndent, GameData.width, GameData.height), GameData.data.volume, 0, 1);

                if(GUI.Button(new Rect(GameData.leftIndent, GameData.topIndent + (GameData.height + GameData.margin), GameData.width, GameData.height), "Return"))
                    setting = !setting;
            }
            else if(quitting)
            {
                GUI.Label(new Rect(GameData.leftIndent, GameData.topIndent - (GameData.height + GameData.margin), GameData.width, GameData.height), "Close Application?", titleStyle);

                if(GUI.Button(new Rect(GameData.leftIndent, GameData.topIndent, GameData.width, GameData.height), "Yes"))
                    CloseGame();
                if(GUI.Button(new Rect(GameData.leftIndent, GameData.topIndent + (GameData.height + GameData.margin), GameData.width, GameData.height), "No"))
                    quitting = false;
            }
            else
            {
                if(GUI.Button(new Rect(GameData.leftIndent, GameData.topIndent - (GameData.height + GameData.margin), GameData.width, GameData.height), "Start"))
                    LoadGame(1);
                if(GUI.Button(new Rect(GameData.leftIndent, GameData.topIndent, GameData.width, GameData.height), "Settings"))
                    setting = !setting;
                if(GUI.Button(new Rect(GameData.leftIndent, GameData.topIndent + (GameData.height + GameData.margin), GameData.width, GameData.height), "Quit"))
                    quitting = true;
            }
        }
        else 
        {
            if(!paused)
            {
                if(GUI.Button(GameData.pauseButton, ""))
                    PauseGame();
            }
            else
            {
                if(GUI.Button(new Rect(GameData.leftIndent, GameData.topIndent - (GameData.height + GameData.margin), GameData.width, GameData.height), "Resume"))
                    ResumeGame();
                if(GUI.Button(new Rect(GameData.leftIndent, GameData.topIndent, GameData.width, GameData.height), "Restart"))
                    ResetLevel();
                if(GUI.Button(new Rect(GameData.leftIndent, GameData.topIndent + (GameData.height + GameData.margin), GameData.width, GameData.height), "Main Menu"))
                    LoadGame(0);
            }
        }
    }

    void PauseGame()
    {
        paused = true;
        Time.timeScale = 0;
        gravityManager.SetActive(false);
        Input.gyro.enabled = false;
    }

    void ResumeGame()
    {
        paused = false;
        Time.timeScale = 1;
        gravityManager.SetActive(true);
        Input.gyro.enabled = true;
        if(player) player.rigidbody2D.velocity = Vector2.zero;
    }

    void ResetLevel()
    {
        Application.LoadLevel(GameData.data.currentLevel);
    }

    void LoadGame(int level)
    {
        Application.LoadLevel(level);
    }

    void CloseGame()
    {
        Application.Quit();
    }
}
