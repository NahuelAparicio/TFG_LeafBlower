using UnityEngine;

public class ActivableComponent : MonoBehaviour
{
    [SerializeField]private TriggerComponent _trigger;

    private void Awake()
    {
        _trigger.OnComplete += ExecuteMovement;
    }

    public void ExecuteMovement()
    {
        Debug.Log("Is Completed");
    }
}
