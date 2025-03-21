using UnityEngine;

public class VisitQuest : QuestStep
{
    [SerializeField] private string nameToVisit = "first";
    [SerializeField] private int goldReward;

    private void Start()
    {
        string status = "Visit the " + nameToVisit + ".";
        ChangeState("", status);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            string status = "The " + nameToVisit + " has been visited.";
            ChangeState("", status);
            FinishQuestStep();
            GameEventManager.Instance.collectingEvents.CollectCoin(goldReward);
        }
    }

    protected override void SetQuestStepState(string state)
    {
        // No needed
    }

}
