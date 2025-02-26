using UnityEngine;
public class PushButtonQuestStep : QuestStep
{
    private int buttonsPushed = 0;
    public int buttonsToPush = 0;

    private void OnEnable()
    {
        GameEventManager.Instance.triggerEvents.onTriggerButton += ButtonPushed;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.triggerEvents.onTriggerButton -= ButtonPushed;
    }

    private void Start()
    {
        UpdateState();
    }

    private void ButtonPushed()
    {
        if (isFinished) return;

        if(buttonsPushed < buttonsToPush)
        {
            buttonsPushed++;
            UpdateState();
        }
        if(buttonsPushed >= buttonsToPush)
        {
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        string state = buttonsPushed.ToString();
        string status = "(" + buttonsPushed + " / " + buttonsToPush + ")";
        ChangeState(state, status);
    }

    protected override void SetQuestStepState(string state)
    {
        buttonsPushed = System.Int32.Parse(state);
        UpdateState();
    }
}
