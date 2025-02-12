using System;

public class CameraEvents
{
    public event Action<int> onZoomIn;
    public event Action<int> onZoomOut;

    public void ZoomIn(int zoom)
    {
        onZoomIn?.Invoke(zoom);
    }

    public void ZoomOut(int zoom) 
    {  
        onZoomOut?.Invoke(zoom); 
    }
}
