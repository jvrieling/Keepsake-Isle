using GBTemplate;
using UnityEngine;

public class GBSetup : MonoBehaviour
{
    private GBConsoleController gb;

    public AudioClip bgm;
    public float musicVolume = 0.5f;

    void Start()
    {
        gb = GBConsoleController.GetInstance();

        gb.Display.UpdateColorPalette(0);

        PlayMusic();
        gb.Sound.CurrentMusicVolume = musicVolume;
    }

    public void StopMusic()
    {
        gb.Sound.StopMusic();
    }

    public void PlayMusic()
    {
        gb.Sound.StopMusic();
        gb.Sound.PlayMusic(bgm);
    }
}
