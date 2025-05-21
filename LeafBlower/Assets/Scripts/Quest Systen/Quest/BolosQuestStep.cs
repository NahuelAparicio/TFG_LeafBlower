using System.Collections.Generic;
using UnityEngine;

public class BolosQuestStep : QuestStep
{
    [SerializeField] private GameObject bolo;
    [SerializeField] private NormalObject boloBall;
    [SerializeField] private PositionToShoot _position;

    [SerializeField] private Transform[] posToInstantiateBolo;

    public List<Transform> bolosTransforms = new List<Transform>();

    private int _bolosDone = 0;
    public int bolosToMake = 10;

    public float timeToCheckBolos;
    private float _currentTime;


    private Vector3 ballPosition;
    private void Awake()
    {
        InstantiateBolos();
        ballPosition = boloBall.transform.position;
    }

    private void Start()
    {
        string state = _bolosDone.ToString();
        string status = "" + " (" + _bolosDone + "/" + bolosToMake + ")";
        ChangeState(state, status);
    }

    void Update()
    {
        _currentTime += Time.deltaTime;
        if(_currentTime >= timeToCheckBolos && boloBall.HasBeenShoot)
        {
            CheckBolosStatus();
        }
        else if(boloBall.HasBeenShoot && !_position.IsInPosition)
        {
            ResetBolos();
        }
    }

    private void InstantiateBolos()
    {
        foreach (var posTransform in posToInstantiateBolo)
        {
            GameObject go = Instantiate(bolo, posTransform.position, Quaternion.identity);
            bolosTransforms.Add(go.GetComponent<Transform>());
        }
    }

    private void CheckBolosStatus()
    {
        _currentTime = 0;

        foreach (Transform boloTransform in bolosTransforms)
        {
            if (IsBoloFallen(boloTransform))
            {
                _bolosDone++;
                UpdateState();
            }
        }

        if (_bolosDone == bolosTransforms.Count)
        {
            // ?? Reproducir sonido solo si se derribaron todos los bolos
            FMODUnity.RuntimeManager.PlayOneShot("event:/Interactables/Ball/Bowl_Celebration", transform.position);
            FinishQuestStep();
        }
        else
        {
            if (_bolosDone > 0)
            {
                ResetBolos();
            }
        }
    }


    private void ResetBolos()
    {
        _bolosDone = 0;
        boloBall.transform.position = ballPosition;
        foreach (Transform t in bolosTransforms)
        {
            Destroy(t.gameObject);
        }
        bolosTransforms.Clear();
        InstantiateBolos();
        UpdateState();
    }

    private void UpdateState()
    {
        string state = _bolosDone.ToString();
        string status = "" + " (" + _bolosDone + "/" + bolosToMake + ")";
        if (state == null) state = "";
        ChangeState(state, status);
    }
    private bool IsBoloFallen(Transform boloTransform)
    {
        float angle = Vector3.Angle(boloTransform.up, Vector3.up);
        return angle > 30f;
    }
    protected override void SetQuestStepState(string state)
    {
        _bolosDone = System.Int32.Parse(state);
        UpdateState();
    }
}
