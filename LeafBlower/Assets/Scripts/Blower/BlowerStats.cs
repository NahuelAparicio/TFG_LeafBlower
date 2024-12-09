using UnityEngine;


public class BlowerStats : MonoBehaviour
{
    public Stat maxStamina;

    [SerializeField] private Stat _blowForce;
    public float BlowForce => _blowForce.Value;

    [SerializeField] private Stat _aspireForce;
    public float AspireForce => _aspireForce.Value;

    [SerializeField] private Stat _shootForce;
    public float ShootForce => _shootForce.Value;
}
