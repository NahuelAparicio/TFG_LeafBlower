using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stat
{
    public float baseValue;
    private float _currentValue;
    [SerializeField] private bool _reCalculate = true;

    public List<StatModifier> modifiers;

    public float Value
    {
        get
        {
            if(_reCalculate)
            {
                CalculateValue();
                _reCalculate = false;
            }
            return _currentValue;
        }
    }

    private void CalculateValue()
    {
        _currentValue = baseValue;
        foreach (var mod in modifiers)
        {
            if(mod.type == Enums.ModifierType.Flat)
            {
                _currentValue += mod.value;
            }
            else if(mod.type == Enums.ModifierType.PercentualToBase)
            {
                _currentValue += (baseValue * (mod.value / 100));
            }
        }
    }

    public void AddModifier(StatModifier mod)
    {
        modifiers.Add(mod);
        _reCalculate = true;
    }
    public void RemoveModifier(StatModifier mod)
    {
        if(modifiers.Contains(mod))
        {
            modifiers.Remove(mod);
            _reCalculate = true;
        }
    }
}