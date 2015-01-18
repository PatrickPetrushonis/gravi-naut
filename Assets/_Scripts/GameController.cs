using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    public GameObject gravityManager;
    private GameObject player;

    public bool paused = false;
    public bool setting = false;
    public bool quitting = false;
    public bool complete = false;

    void Start()
    {
        GameData.data.currentLevel = Application.loadedLevel;        
        if(player) player = GameObject.Find("Player");
    }

    public void Setting()
    {
        setting = !setting;
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
        if(player) player.rigidbody2D.velocity = Vector2.zero;
    }

    public void ResetLevel()
    {
        Application.LoadLevel(GameData.data.currentLevel);
    }

    public void LoadGame(int level)
    {
        GameData.data.currentLevel = level;
        Application.LoadLevel(level);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
