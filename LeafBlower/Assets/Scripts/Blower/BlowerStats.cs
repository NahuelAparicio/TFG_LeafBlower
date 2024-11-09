using UnityEngine;


public class BlowerStats : MonoBehaviour
{
    public Stat stamina;
    public Stat blowForce;
    public Stat aspireForce;

    public bool HasStamina() => stamina.Value > 0;
}
