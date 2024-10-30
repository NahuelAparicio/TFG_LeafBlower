using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowerController : MonoBehaviour
{
    // en X de 90 a -40º

    private BlowerInputs _inputs;
    private BlowerStats _stats;
    [SerializeField] private BlowerForce _blowerForce;
    [SerializeField] private AspirerForce _aspirerForce;

    public BlowerStats Stats => _stats;
    public BlowerInputs Inputs => _inputs;
    public BlowerForce BlowerForce => _blowerForce;
    public AspirerForce AspirerForce => _aspirerForce;

    private void Awake()
    {
        _inputs = GetComponent<BlowerInputs>();
        _stats = GetComponent<BlowerStats>();
    }

    
}
