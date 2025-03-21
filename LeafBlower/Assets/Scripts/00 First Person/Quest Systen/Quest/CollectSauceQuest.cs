
public class CollectSauce : QuestStep
{

    private void Start()
    {
        GameEventManager.Instance.collectingEvents.onCollectColectionable += CollectingEvents_onCollectColectionable;
    }

    private void CollectingEvents_onCollectColectionable(string id)
    {
        if(id == "HotSauce")
        {
            FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state)
    {
       //No need?
    }

    private void OnDisable()
    {
        GameEventManager.Instance.collectingEvents.onCollectColectionable -= CollectingEvents_onCollectColectionable;
    }

}
