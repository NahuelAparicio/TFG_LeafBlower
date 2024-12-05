using UnityEngine;

public class ActivableComponent : MonoBehaviour
{
    [SerializeField] private TriggerComponent _trigger;

    protected virtual void Awake()
    {
        _trigger.OnComplete += ExecuteMovement;
    }

    public virtual void ExecuteMovement()
    {
        Debug.Log("Is Completed");
    }
}
