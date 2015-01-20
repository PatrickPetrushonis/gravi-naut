using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour 
{
    private GameController control;

    public GameObject main;
    public GameObject select;
    public GameObject setting;
    public GameObject close;
    public GameObject pause;
    public GameObject hud;

    private Slider volume;
    private float newVolume;

    void Start()
    {
        control = GameObject.Find("GameManager").GetComponent<GameController>();

        if(!setting.activeSelf) setting.SetActive(true);

        volume = setting.transform.FindChild("VolumeSlider").GetComponent<Slider>();
        newVolume = volume.value;
        GameData.data.volume = newVolume;

        if(GameData.data.currentLevel != 0) main.SetActive(false);
        select.SetActive(false);
        setting.SetActive(false);
        close.SetActive(false);
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
        setting.SetActive(false);
        main.SetActive(true);

        if(apply)
        {
            newVolume = volume.value;
            GameData.data.volume = newVolume;
        }
        else
            volume.value = newVolume;
    }
}