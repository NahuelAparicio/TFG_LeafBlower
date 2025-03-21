
[System.Serializable]
public class QuestData
{
    public Enums.QuestState state;
    public int questStepIndex;
    public QuestStepState[] questStepStates;

    public QuestData(Enums.QuestState _state, int index, QuestStepState[] _questStepStates)
    {
        state = _state;
        questStepIndex = index;
        questStepStates = _questStepStates;
    }
}
