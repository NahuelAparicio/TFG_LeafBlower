using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private GameObject requirementsNotMetToStartIcon;
    [SerializeField] private GameObject canStartIcon;
    [SerializeField] private GameObject requirementsNotMetToFinishIcon;
    [SerializeField] private GameObject canFinishIcon;

   public void SetState(Enums.QuestState newState, bool startPoint, bool finisPoint)
    {
        requirementsNotMetToStartIcon.SetActive(false);
        canStartIcon.SetActive(false);
        requirementsNotMetToFinishIcon.SetActive(false);
        canFinishIcon.SetActive(false);

        switch (newState)
        {
            case Enums.QuestState.RequirementNotMet:
                if(startPoint)
                {
                    requirementsNotMetToStartIcon.SetActive(true);
                }
                break;
            case Enums.QuestState.CanStart:
                if(startPoint)
                {
                    canStartIcon.SetActive(true);
                }
                break;
            case Enums.QuestState.InProgress:
                if(finisPoint)
                {
                    requirementsNotMetToFinishIcon.SetActive(true);
                }
                break;
            case Enums.QuestState.CanFinish:
                if(finisPoint)
                {
                    canFinishIcon.SetActive(true);
                }
                break;
            case Enums.QuestState.Finished:
                break;
            default:
                break;
        }
    }
}
