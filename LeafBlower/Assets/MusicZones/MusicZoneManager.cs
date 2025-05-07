using FMOD.Studio;

public static class MusicZoneManager
{
    private static EventInstance currentMusicInstance;

    public static void PlayNewMusic(EventInstance newInstance)
    {
        StopCurrentMusic();
        currentMusicInstance = newInstance;
        currentMusicInstance.start();
    }

    public static void StopCurrentMusic()
    {
        if (currentMusicInstance.isValid())
        {
            currentMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusicInstance.release();
            currentMusicInstance.clearHandle();
        }
    }

    public static EventInstance GetCurrentInstance()
    {
        return currentMusicInstance;
    }
}
