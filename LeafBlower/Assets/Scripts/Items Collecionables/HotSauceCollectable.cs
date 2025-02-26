
public class HotSauceCollectable : ItemColectable
{
    protected override void OnCollect()
    {
        GameEventManager.Instance.collectingEvents.CollectCollectionable("HotSauce");
    }
}
