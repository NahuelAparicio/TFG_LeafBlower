using System;
public class PlayerEvents 
{
    public event Action onDetachObject;
    public event Action<int> onLevelUp;

    public void PlayerLevelChange(int level) => onLevelUp?.Invoke(level);

    public void DetachObject() => onDetachObject?.Invoke();

}
