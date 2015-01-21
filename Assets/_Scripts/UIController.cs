using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour 
{
    private GameController control;

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
    }
}