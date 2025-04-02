using UnityEngine;
using UnityEngine.Events;

public class ActionerObject : NormalObject
{
    private bool _hasBeenActived = false;
    public UnityEvent OnAction; 

    protected override void Awake()
    {
        base.Awake();
        _canBeAspired = false;
    }

    public override void OnBlow(Vector3 force, Vector3 point)
    {
        if(!_hasBeenActived)
        {
            OnAction?.Invoke();
            _hasBeenActived = true;
        }
    }
}
