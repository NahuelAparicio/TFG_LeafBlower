using UnityEngine;
public class BringMango : QuestStep
{
    [SerializeField] private string nameObject;
    private void Start()
    {
        string status = "Bring me " + nameObject + ".";
        ChangeState("", status);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(nameObject))
        {
            string status = "The " + nameObject + " has been given.";
            ChangeState("", status);
            Destroy(other.gameObject.transform.parent.gameObject);
            FinishQuestStep();
        }
    }
    protected override void SetQuestStepState(string state)
    {
        //No needed
    }
}
