using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerComponent : MonoBehaviour
{
    [SerializeField] private int _impactsToComplete;

    [SerializeField] private bool _isCompleted;

    private int _currentImpacts = 0;

    public event System.Action OnComplete;

    public UnityEvent eventToTrigger;

    private void Awake()
    {
        _isCompleted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        _currentImpacts++;

        if(IsCompleted())
        {
            OnComplete?.Invoke();
            eventToTrigger?.Invoke();
        }
    }

    private bool IsCompleted() => _currentImpacts == _impactsToComplete;

}
