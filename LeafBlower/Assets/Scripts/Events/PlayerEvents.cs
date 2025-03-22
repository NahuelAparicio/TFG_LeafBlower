using System;
public class PlayerEvents 
{
    public event Action onDetachObject;
    public event Action<int> onLevelUp;
    public event Action<MovableObject> OnAttach;
    public event Action<IMovable> OnDestroy;

    public void InvokeDestroy(IMovable movable) => OnDestroy?.Invoke(movable);

    public void InvokeAttach(MovableObject movObj) => OnAttach?.Invoke(movObj);

    public void PlayerLevelChange(int level) => onLevelUp?.Invoke(level);

    public void DetachObject() => onDetachObject?.Invoke();

}
