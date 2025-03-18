using System.Collections;
using UnityEngine;

public class Platform : IActivable
{
    public Transform platform;
    public float speed;
    public float distance;

    private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private Coroutine _moveCoroutine;

    private void Start()
    {
        _initialPosition = platform.position;
        _targetPosition = _initialPosition + new Vector3(0, distance, 0);
    }

    public override void DoAction()
    {
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
        _moveCoroutine = StartCoroutine(MovePlatform(_targetPosition));
    }

    public override void UndoAction()
    {
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
        _moveCoroutine = StartCoroutine(MovePlatform(_initialPosition));
    }

    private IEnumerator MovePlatform(Vector3 target)
    {
        Vector3 start = platform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            platform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        platform.position = target;
    }
}