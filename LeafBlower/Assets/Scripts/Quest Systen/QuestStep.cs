using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string _questId;

    public void InitializeQuestStep(string id)
    {
        _questId = id;
    }
    protected void FinishQuestStep()
    {
        if(!isFinished)
        {
            isFinished = true;

            GameEventManager.Instance.questEvents.AdvanceQuest(_questId);

            Destroy(gameObject);
        }
    }
}
