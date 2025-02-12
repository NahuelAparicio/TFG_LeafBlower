using System;

public class CameraEvents
{
    public event Action<int> onZoom;
    public event Action onResetZoom;

    public void ResetZoom()
    {
        onResetZoom?.Invoke();
    }

    public void Zoom(int zoom)
    {
        onZoom?.Invoke(zoom);
    }
}
