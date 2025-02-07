
public class PushButtonQuestStep : QuestStep
{
    private int buttonsPushed = 0;
    public int buttonsToPush = 0;

    private void OnEnable()
    {
        GameEventManager.Instance.onButtonPushed += Instance_onButtonPushed;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.onButtonPushed -= Instance_onButtonPushed;
    }

    private void Instance_onButtonPushed()
    {
        buttonsPushed++;
        if(buttonsPushed >= buttonsToPush)
        {
            FinishQuestStep();
        }
    }
}
