using System;

public class CollectingEvents
{
    public event Action<int> onCollectCoin;
    public event Action<string> onCollectColectionable;

    public void CollectCoin(int coin)
    {
        onCollectCoin?.Invoke(coin);
    }

    public void CollectCollectionable(string name)
    {
        onCollectColectionable?.Invoke(name);
    }

}
