
public class JordanColectable : ItemColectable
{
    protected override void OnCollect()
    {
        //GameEventManager.Instance.collectingEvents.InvokeCollectCollectionable(data.id);
        base.OnCollect();
    }
}
