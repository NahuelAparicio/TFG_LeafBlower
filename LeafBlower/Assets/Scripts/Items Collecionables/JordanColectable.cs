
public class JordanColectable : ItemColectable
{
    protected override void OnCollect()
    {
        GameEventManager.Instance.collectingEvents.CollectCollectionable(data.id);
        base.OnCollect();
    }
}
