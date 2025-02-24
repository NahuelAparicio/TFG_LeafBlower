

public class BasketMade : QuestStep
{
    private int basketsDone = 0;
    public int basketsToMade = 0;

    private void Start()
    {
        GameEventManager.Instance.triggerEvents.onTriggerBasket += OnBasketMade;
        UpdateState();
    }

    private void OnDisable()
    {
        GameEventManager.Instance.triggerEvents.onTriggerBasket -= OnBasketMade;
    }

    private void OnBasketMade(string id)
    {
        if (id != _questId) return;

        if(basketsDone < basketsToMade)
        {
            basketsDone++;
            UpdateState();
        }
        if(basketsDone >= basketsToMade)
        {
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        string state = basketsDone.ToString();
        string status = "Pushed: " + basketsDone + " / " + basketsToMade;
        ChangeState(state, status);
    }
    protected override void SetQuestStepState(string state)
    {
        basketsDone = System.Int32.Parse(state);
        UpdateState();
    }
}
