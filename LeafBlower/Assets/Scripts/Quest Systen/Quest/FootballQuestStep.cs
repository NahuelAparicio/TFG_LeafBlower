using UnityEngine;

public class FootballQuestStep : QuestStep
{
    [TextArea][SerializeField] private string _description;
    private int _goalsDone = 0;
    public int goalToMake = 1;
    private void Start()
    {
        string state = _goalsDone.ToString();
        string status = "" + _description + " (" + _goalsDone + "/" + goalToMake + ")";
        ChangeState(state, status);
        GameEventManager.Instance.triggerEvents.onTriggerBall += BallTriggered;
    }

    private void BallTriggered(string id)
    {
        if (id != _questId) return;

        if (_goalsDone < goalToMake)
        {
            _goalsDone++;
            UpdateState();

            FMODUnity.RuntimeManager.PlayOneShot("event:/Interactables/Ball/Ball_Celebration", transform.position);
        }

        if (_goalsDone >= goalToMake)
        {
            FinishQuestStep();
        }
    }



    private void UpdateState()
    {
        string state = _goalsDone.ToString();
        string status = "" + _description + " (" + _goalsDone + "/" + goalToMake + ")";
        if (state == null) state = "";
        ChangeState(state, status);
    }

    protected override void SetQuestStepState(string state)
    {
        goalToMake = System.Int32.Parse(state);
        UpdateState();
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.triggerEvents.onTriggerBall -= BallTriggered;
    }
}
