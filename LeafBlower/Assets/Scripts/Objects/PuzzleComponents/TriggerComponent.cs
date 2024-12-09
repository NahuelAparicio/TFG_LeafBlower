using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerComponent : MonoBehaviour
{
    [SerializeField] private int _impactsToComplete;

    [SerializeField] private bool _isCompleted;

    private int _currentImpacts = 0;

    public event System.Action OnComplete;

    private void Awake()
    {
        _isCompleted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        IShooteable shooteable = other.GetComponent<IShooteable>();
        if (shooteable == null && !IsCompleted()) return;
        _currentImpacts++;

        if(IsCompleted())
            OnComplete?.Invoke();
    }

    private bool IsCompleted() => _currentImpacts == _impactsToComplete;

}
