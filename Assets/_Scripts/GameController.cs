using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    public GameObject gravityManager;
    public GameObject player;

    void Start()
    {
        GameData.data.currentLevel = Application.loadedLevel;
    }

    void Update()
    {
        if(GameData.data.pause) PauseGame();
        else ResumeGame();
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        gravityManager.SetActive(false);
        Input.gyro.enabled = false;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        gravityManager.SetActive(true);
        Input.gyro.enabled = true;
        if(player) player.rigidbody2D.velocity = Vector2.zero;
    }

    public void LoadGame(int level)
    {
        GameData.data.quit = false;
        GameData.data.pause = false;
        GameData.data.complete = false;

        if(level == -1) Application.Quit();
        else Application.LoadLevel(level);
    }
}
