using System.Collections.Generic;
using UnityEngine;

public class BolosQuestStep : QuestStep
{
    private ObjectInstancier _instancier;

    public List<Transform> bolosTransforms = new List<Transform>();

    private int _numPoints;

    private void Awake()
    {
        _instancier = GetComponent<ObjectInstancier>();
        _instancier.InstanceObjects();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void CheckBolosStatus()
    {

        foreach (Transform boloTransform in bolosTransforms)
        {
            if(boloTransform.localRotation.eulerAngles.x < 250f || boloTransform.localRotation.eulerAngles.x > 282f)
            {
                _numPoints++;
            }
        }
    }

    private void ResetPoints()
    {
        for (int i = 0; i < bolosTransforms.Count; i++)
        {
            Destroy(bolosTransforms[i]);
        }
        bolosTransforms.Clear();
    }

    protected override void SetQuestStepState(string state)
    {
        throw new System.NotImplementedException();
    }
}
