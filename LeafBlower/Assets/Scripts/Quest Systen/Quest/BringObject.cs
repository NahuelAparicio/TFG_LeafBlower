using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class BringObject : QuestStep
{
    [SerializeField] private string nameObject;
    [SerializeField] private string fmodEventPathAppear;  // Evento que suena mientras el objeto está presente
    [SerializeField] private string fmodEventPathDeliver; // Evento que suena una vez al entregarlo

    private EventInstance fmodAppearInstance;

    private void Start()
    {
        string status = "Traer " + nameObject + ".";
        ChangeState("", status);

        // Iniciar evento de aparición
        fmodAppearInstance = RuntimeManager.CreateInstance(fmodEventPathAppear);
        fmodAppearInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        fmodAppearInstance.start();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(nameObject))
        {
            if (other.GetComponent<NormalObject>().HasBeenShoot)
            {
                string status = nameObject + " entregado.";
                ChangeState("", status);

                // Reproducir evento de entrega
                RuntimeManager.PlayOneShot(fmodEventPathDeliver, transform.position);

                // Detener evento de aparición
                fmodAppearInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                fmodAppearInstance.release();

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
