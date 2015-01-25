using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour 
{
    private GameController control;

    public Transform gameMenu;
    public Text objectiveText;
    public Text progressText;
 
    public Button pause;
    public Slider volume;

    private float newVolume;
    private string objective;
    private string progress;
    private int currentCount = 0;

    void Start()
    {
        control = GameObject.Find("GameManager").GetComponent<GameController>();

        newVolume = volume.value;
        GameData.data.volume = newVolume;
    }

    void Update()
    {
        if(GameData.data.currentLevel != 0)
        {
            if(objective != GameData.data.objective)
            {
                objective = GameData.data.objective;
                objectiveText.text = objective;
            }

            if(GameData.data.collectibleCount < GameData.data.collectibleTotal)
                DetermineProgress(ref GameData.data.collectibleTotal);
            else
                DetermineProgress(ref GameData.data.eventCount);
        }
    }

    void DetermineProgress(ref int total)
    {
        if(currentCount != total)
        {
            currentCount = total;
            progress = currentCount + "/" + total;
            progressText.text = progress;
        }
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