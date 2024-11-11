using UnityEngine;

public class Quest : MonoBehaviour
{
    public QuestData data;
    private Enums.QuestState _state;

    private void Awake()
    {
        _state = Enums.QuestState.Locked;   //Change in the future for save data system
    }

    void Update()
    {
        
    }
}
