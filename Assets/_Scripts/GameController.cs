using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    private GameObject gravityManager;
    private GameObject player;
    private bool paused = false;

    void Start()
    {
        GameData.data.currentLevel = Application.loadedLevel;
        gravityManager = GameObject.Find("GravityManager");
        player = GameObject.Find("Player");
    }

    void OnGUI()
    {
        if(paused)
        {
            if(GUI.Button(new Rect(UIData.leftIndent, UIData.topIndent - (UIData.height + UIData.margin), UIData.width, UIData.height), "Resume"))
            {
                ResumeGame();
            }
            if(GUI.Button(new Rect(UIData.leftIndent, UIData.topIndent, UIData.width, UIData.height), "Restart"))
            {
                ResetLevel();
            }
            if(GUI.Button(new Rect(UIData.leftIndent, UIData.topIndent + (UIData.height + UIData.margin), UIData.width, UIData.height), "Quit"))
            {
                CloseGame();
            }
        }
        else 
        {
            if(GUI.Button(UIData.pauseButton, ""))
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        paused = true;
        Time.timeScale = 0;
        gravityManager.SetActive(false);
        Input.gyro.enabled = false;
    }

    public void ResumeGame()
    {
        paused = false;
        Time.timeScale = 1;
        gravityManager.SetActive(true);
        Input.gyro.enabled = true;
        player.rigidbody2D.velocity = Vector2.zero;
    }

    public void ResetLevel()
    {
        Application.LoadLevel(GameData.data.currentLevel);
    }

    public void LoadNextLevel()
    {
        Application.LoadLevel(GameData.data.currentLevel++);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
