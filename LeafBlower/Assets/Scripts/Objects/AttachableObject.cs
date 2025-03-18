using UnityEngine;

public class AttachableObject : MonoBehaviour
{
    public TrajectoryHandler trajectory;
    public Rigidbody Rigidbody { get; private set; }
    public ShootableObject Shootable { get; private set; }
    public bool IsAttached => Rigidbody != null;

    [SerializeField] private Collider _collider;

    private void Awake()
    {
        trajectory = GetComponent<TrajectoryHandler>();
        _collider.enabled = false;
    }

    private void Start()
    {
        GameEventManager.Instance.playerEvents.onDetachObject += Detach;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.playerEvents.onDetachObject -= Detach;
    }

    public void Attach(Rigidbody rb, Vector3 attachPosition, Transform attachPoint)
    {
        if (rb == null) return;
        _collider.enabled = true;
        trajectory.EnableLineRender();
        Rigidbody = rb;
        Shootable = rb.GetComponent<ShootableObject>();
        rb.GetComponent<IAttacheable>().Attach(attachPoint, attachPosition);
    }

    public void Detach()
    {
        _collider.enabled = false;
        trajectory.DisableLineRender();
        if (Rigidbody == null) return;
        Rigidbody.GetComponent<IAttacheable>().Detach();
        Rigidbody = null;
        Shootable = null;
    }

    public void DetachOnSave()
    {
        _collider.enabled = false;
        trajectory.DisableLineRender();
        if (Rigidbody == null) return;
        Rigidbody = null;
        Shootable = null;
    }

}
