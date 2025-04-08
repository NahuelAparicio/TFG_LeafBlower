using UnityEngine;
public class BringObject : QuestStep
{
    [SerializeField] private string nameObject;
    private void Start()
    {
        string status = "Traer " + nameObject + ".";
        ChangeState("", status);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(nameObject))
        {
            if(other.GetComponent<NormalObject>().HasBeenShoot)
            {
                string status = "" + nameObject + " entregado.";
                ChangeState("", status);
                Destroy(other.gameObject);
                FinishQuestStep();
            }
        }
    }
    protected override void SetQuestStepState(string state)
    {
        //No needed
    }
}
