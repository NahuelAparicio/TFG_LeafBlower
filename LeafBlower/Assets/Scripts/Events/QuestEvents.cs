using System;

public class QuestEvents 
{
    public event Action<string> onStartQuest;
    public event Action<string> onAdvanceQuest;
    public event Action<string> onFinishQuest;
    public event Action<Quest> onQuestStateChange;
    public event Action<string, int, QuestStepState> onQuestStepChange;

    public void StartQuest(string id)
    {
        if(onStartQuest != null)
        {
            onStartQuest(id);
        }
    }

    public void AdvanceQuest(string id)
    {
        if (onAdvanceQuest != null)
        {
            onAdvanceQuest(id);
        }
    }

    public void FinishQuest(string id)
    {
        if (onFinishQuest != null)
        {
            onFinishQuest(id);
        }
    }

    public void QuestStateChange(Quest q)
    {
        if (onQuestStateChange != null)
        {
            onQuestStateChange(q);
        }
    }

    public void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        if (onQuestStepChange != null)
        {
            onQuestStepChange(id, stepIndex, questStepState);
        }
    }
}
