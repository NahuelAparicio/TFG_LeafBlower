
public class Chest : BreakableObject
{
    private ObjectInstancier _instancer;

    protected override void Awake()
    {
        base.Awake();
        _instancer = GetComponent<ObjectInstancier>();
    }
    public override void ActivateBreak()
    {
        _instancer.InstanceObjects();
        rigidbodies.AddRange(_instancer.rigidBodies);
        base.ActivateBreak();
    }
}
