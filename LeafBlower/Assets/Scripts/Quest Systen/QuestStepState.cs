
[System.Serializable]
public class QuestStepState 
{
    public string state;
    public string status;

    public QuestStepState(string _state, string _status)
    {
        state = _state;
        status = _status;
    }

    public QuestStepState()
    {
        state = "";
        status = "";
    }
}
