using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveActivable : MonoBehaviour
{
    [SerializeField] private IActivable _activable;
    

    public void ActivateActivable()
    {
        _activable.DoAction();
    }
}
