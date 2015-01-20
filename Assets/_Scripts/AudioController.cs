using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour 
{
    private Camera mainCamera;
    private AudioListener listener;
    private AudioSource[] sources;
    public AudioClip music;

    void Start()
    {
        sources = AudioSource.FindObjectsOfType<AudioSource>();

        GameData.data.volume = 0.5f;        //Temporary solution until Save & Load are implemented
    }

    void Update()
    {
        if(sources.Length > 0)              //Only necessary until audio files are included
        {
            if(GameData.data.currentLevel == 0)
            {
                int source = 0;
                sources[source].audio.volume = GameData.data.volume;
                sources[source].audio.clip = music;
            }
        }
    }
}
