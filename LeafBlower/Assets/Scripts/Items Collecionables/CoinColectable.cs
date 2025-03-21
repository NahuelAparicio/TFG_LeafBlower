
public class CoinColectable : ItemColectable
{
    protected override void OnCollect()
    {
        GameEventManager.Instance.collectingEvents.CollectCoin(data.GetAmount());
        base.OnCollect();
    }
}
