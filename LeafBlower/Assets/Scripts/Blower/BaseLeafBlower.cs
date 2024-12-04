using UnityEngine;

public class BaseLeafBlower : MonoBehaviour
{
    protected BlowerController _blower;

    protected virtual void Awake()
    {
        _blower = transform.parent.GetComponent<BlowerController>();
    }
    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
        
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        var outlineable = other.GetComponent<IOutlineable>();
        if (_blower.IsAspirating() || outlineable == null) return;
        outlineable.EnableOutline();
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        var outlineable = other.GetComponent<IOutlineable>();
        if (_blower.IsAspirating() || outlineable == null) return;
        outlineable.DisableOutline();
    }
}
