using UnityEngine;
public class BringMango : QuestStep
{
    [SerializeField] private string nameObject;
    private void Start()
    {
        string status = "Encuentra mi " + nameObject + ".";
        ChangeState("", status);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(nameObject))
        {
            string status = "El " + nameObject + " ha sido entregado.";
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
