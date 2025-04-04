using System;

public class TriggerEvents 
{
    public event Action onTriggerButton;
    public event Action<string> onTriggerBall;

    public void TriggerButton() => onTriggerButton?.Invoke();
    public void InvokeBallTriggered(string id) => onTriggerBall?.Invoke(id);
}
