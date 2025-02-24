using UnityEngine;

public class AttachableObject : MonoBehaviour
{
    public TrajectoryHandler trajectory;
    public Rigidbody Rigidbody { get; private set; }
    public ShootableObject Shootable { get; private set; }
    public bool IsAttached => Rigidbody != null;

    private void Awake()
    {
        trajectory = GetComponent<TrajectoryHandler>();
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
        trajectory.EnableLineRender();
        Rigidbody = rb;
        Shootable = rb.GetComponent<ShootableObject>();
        rb.GetComponent<IAttacheable>().Attach(attachPoint, attachPosition);
    }

    public void Detach()
    {
        trajectory.DisableLineRender();
        if (Rigidbody == null) return;
        Rigidbody.GetComponent<IAttacheable>().Detach();
        Rigidbody = null;
        Shootable = null;
    }

    public void DetachOnSave()
    {
        trajectory.DisableLineRender();
        if (Rigidbody == null) return;
        Rigidbody = null;
        Shootable = null;
    }
}
