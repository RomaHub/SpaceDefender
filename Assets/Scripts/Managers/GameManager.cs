using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;

public class GameManager : NetworkManager
{
    static public GameManager instance;

    [Header("Audio components")]
    [SerializeField]
    private AudioMixerSnapshot _gameSnapshot;
    [SerializeField]
    private AudioMixerSnapshot _pauseSnapshot;
    [SerializeField]
    private AudioClip _click;
    [SerializeField]
    private AudioSource _musicAudio;
    [SerializeField]
    private AudioSource _sfxAudio;


    private void Start()
    {
        instance = this;
    }

    public void ClickSound()
    {
        _sfxAudio.PlayOneShot(_click);
    }

    public void ToggleMusicSnapshot()
    {
        if (Time.timeScale == 0)
        {
            _pauseSnapshot.TransitionTo(0f);
        }
        else
        {
            _gameSnapshot.TransitionTo(0f);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
