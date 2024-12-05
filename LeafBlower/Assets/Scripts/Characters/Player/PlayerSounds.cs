using FMODUnity;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public void PlayFootSteps() => RuntimeManager.PlayOneShot("event:/Character/FootSteps/FootSteps_Concrete", transform.position);
}
