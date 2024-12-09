using FMODUnity;
using UnityEngine;

// -- Singleton managing music, It can be called in any script for play music or whatever related with music
public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;

    private FMOD.Studio.EventInstance inGameMusicInstance;
    private FMOD.Studio.EventInstance dialogueMusicInstance;
    private FMOD.Studio.EventInstance menuMusicInstance;
    public static MusicManager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("Music Manager");
                go.AddComponent<MusicManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayExplorationMusic()
    {
        if (IsMusicPlaying()) return;

        StopAllMusic();
        inGameMusicInstance = RuntimeManager.CreateInstance(Constants.MUSIC_EXPLORATION);
        inGameMusicInstance.start();
    }
    public void PlayMenuMusic()
    {
        if (IsMenuMusicPlaying()) return;

        StopAllMusic();
        menuMusicInstance = RuntimeManager.CreateInstance(Constants.MUSIC_MENU);
        menuMusicInstance.start();
    }
    public void PlayDialogs()
    {
        if(IsDialogueMusicPlaying()) return;

        StopAllMusic();
        dialogueMusicInstance = RuntimeManager.CreateInstance(Constants.MUSIC_DIALOGS);
        dialogueMusicInstance.start();
    }

    public void PauseMusic(bool pause)
    {
        if(inGameMusicInstance.isValid())
        {
            inGameMusicInstance.setPaused(pause);
        }
        if (dialogueMusicInstance.isValid())
        {
            dialogueMusicInstance.setPaused(pause);
        }
        if (menuMusicInstance.isValid())
        {
            menuMusicInstance.setPaused(pause);
        }
    }

    public void StopAllMusic()
    {
        if(inGameMusicInstance.isValid())
        {
            inGameMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            inGameMusicInstance.release();
        }
        if (dialogueMusicInstance.isValid())
        {
            dialogueMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            dialogueMusicInstance.release();
        }
        if (menuMusicInstance.isValid())
        {
            menuMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            menuMusicInstance.release();
        }
    }
    private bool IsMusicPlaying()
    {
        if (inGameMusicInstance.isValid())
        {
            inGameMusicInstance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE state);
            return state == FMOD.Studio.PLAYBACK_STATE.PLAYING;
        }
        return false;
    }

    private bool IsMenuMusicPlaying()
    {
        if (menuMusicInstance.isValid())
        {
            menuMusicInstance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE state);
            return state == FMOD.Studio.PLAYBACK_STATE.PLAYING;
        }
        return false;
    }
    private bool IsDialogueMusicPlaying()
    {
        if (dialogueMusicInstance.isValid())
        {
            dialogueMusicInstance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE state);
            return state == FMOD.Studio.PLAYBACK_STATE.PLAYING;
        }
        return false;
    }


    private void OnDestroy()
    {
        StopAllMusic();
    }

}
