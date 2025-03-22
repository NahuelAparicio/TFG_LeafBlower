using System;

public class CollectingEvents
{
    public event Action<Enums.CollectionableType, int> OnCollectColectionable;


    public void InvokeCollectCollectionable(Enums.CollectionableType type, int amount) => OnCollectColectionable?.Invoke(type, amount);

}
