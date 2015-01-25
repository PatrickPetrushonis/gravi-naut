using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour 
{
    private GameController control;

    public Transform gameMenu;
    public Button pause;
    public Slider volume;
    private float newVolume;

    void Start()
    {
        control = GameObject.Find("GameManager").GetComponent<GameController>();
        newVolume = volume.value;
        GameData.data.volume = newVolume;
    }

    public void DeactivateMenu(GameObject parent)
    {
        parent.SetActive(false);
    }

    public void ActivateMenu(GameObject menu)
    {
        menu.SetActive(true);
    }

    public void LoadLevel(int level)
    {
        control.LoadGame(level);
    }

    public void Apply(bool apply)
    {
        if(apply)
        {
            newVolume = volume.value;
            GameData.data.volume = newVolume;
        }
        else
            volume.value = newVolume;
    }

    public void Pause()
    {
        pause.interactable = !pause.interactable;
        GameData.data.pause = !GameData.data.pause;

        if(GameData.data.pause)
        {
            control.PauseGame();
            PivotUI();
        }
        else control.ResumeGame();
    }

    void PivotUI()
    {
        Vector2 direction = GameData.data.direction;
        float angle = 0;

        //pivot in-game menu with respect to current direction of gravity
        if(direction.y != -1)
        {
            if(direction.y == 1)        angle = 180;            
            else if(direction.x == 1)   angle = 90;
            else if(direction.x == -1)  angle = -90;
        }

        gameMenu.rotation = Quaternion.Euler(0, 0, angle);
    }
}