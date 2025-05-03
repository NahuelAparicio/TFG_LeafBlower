using UnityEngine;
using FMODUnity; 
public class ColectionableObject : MovableObject
{
    [SerializeField] private float _localScaleSpeed;
    [SerializeField] private GameObject _objectToScale;
    [SerializeField][Range(0, 1)] private float percentageToScale;

    private Vector3 _originalScale;
    private Vector3 _targetScale;

    public float rotationSpeed;

    protected override void Awake()
    {
        base.Awake();
        _originalScale = _objectToScale.transform.localScale;
        _targetScale = _originalScale * percentageToScale;
    }
    protected override void Update()
    {
        base.Update();
        transform.Rotate(0f, (rotationSpeed * 10) * Time.deltaTime, 0f);
    }
    public override void OnBlow(Vector3 force, Vector3 point)
    {
    }

    protected override void UpdateCustom()
    {
        _objectToScale.transform.localScale = Vector3.MoveTowards(_objectToScale.transform.localScale, _targetScale, _localScaleSpeed * Time.deltaTime);
    }

    protected override void OnArriveToAttacher()
    {
        RuntimeManager.PlayOneShot("event:/Interactables/Coin/Coin_Collected", transform.position);
        GameEventManager.Instance.collectingEvents.InvokeCollectCollectionable(_data.GetCollectionableType(), _data.GetAmount());
        GameEventManager.Instance.playerEvents.InvokeDestroy(this);
        Destroy(gameObject);
    }
}
