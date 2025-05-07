using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class CollisionAudioParameterChanger : MonoBehaviour
{
    [Header("Objeto a detectar")]
    public GameObject targetObject;

    [Header("Valores a cambiar")]
    public float footStepsParameterValue;
    public float jumpParameterValue;
    public float landParameterValue;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == targetObject)
        {
            SetFMODParameter("event:/Character/FootSteps/FootSteps", "FootSteps", footStepsParameterValue);
            SetFMODParameter("event:/Character/Jump/Jump", "Jump", jumpParameterValue);
            SetFMODParameter("event:/Character/Land/Land", "Land", landParameterValue);
        }
    }

    private void SetFMODParameter(string eventPath, string parameterName, float value)
    {
        if (string.IsNullOrEmpty(eventPath)) return;

        EventInstance instance = RuntimeManager.CreateInstance(eventPath);
        instance.setParameterByName(parameterName, value);
        instance.start();
        instance.release();
    }
}
