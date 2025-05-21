using System.Collections;
using UnityEngine;

public class ActivableComponent : MonoBehaviour
{
    [SerializeField] private TriggerComponent _trigger;

    protected virtual void Awake()
    {
        _trigger.OnComplete += ExecuteMovement;
    }

    public virtual void ExecuteMovement()
    {
        StopAllCoroutines();
        StartCoroutine(MoveDoors(Quaternion.Euler(0, -90, 0), Quaternion.Euler(0, 90, 0)));
    }


    public Transform doorLeft, doorRight;
    public float rotationSpeed;

    private IEnumerator MoveDoors(Quaternion targetedLeft, Quaternion targetedRight)
    {

        Quaternion initialRotationLeft = doorLeft.localRotation;
        Quaternion initialRotationRight = doorRight.localRotation;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;

            doorLeft.localRotation = Quaternion.Slerp(initialRotationLeft, targetedLeft, t);
            doorRight.localRotation = Quaternion.Slerp(initialRotationRight, targetedRight, t);

            yield return null;
        }

    }
}
