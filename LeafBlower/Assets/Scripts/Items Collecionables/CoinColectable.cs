
public class CoinColectable : ItemColectable
{
    protected override void OnCollect()
    {
        //GameEventManager.Instance.collectingEvents.InvokeCollectCoin(data.GetAmount());
        base.OnCollect();
    }
}
