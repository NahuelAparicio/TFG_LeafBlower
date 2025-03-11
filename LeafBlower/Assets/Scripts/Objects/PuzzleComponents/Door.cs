using System.Collections;
using UnityEngine;

public class Door : IActivable
{
    public Transform doorLeft, doorRight;
    public float rotationSpeed;

    public Quaternion targetRotationLeft;
    public Quaternion targetRotationRight;

    private Quaternion originalRotationLeft;
    private Quaternion originalRotationRight;
    private Coroutine moveCoroutine;

    private void Start()
    {
        originalRotationLeft = doorLeft.localRotation;
        originalRotationRight = doorRight.localRotation;
    }

    public override void DoAction()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveDoors(targetRotationLeft, targetRotationRight));
    }

    public override void UndoAction()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveDoors(originalRotationLeft, originalRotationRight));
    }

    private IEnumerator MoveDoors(Quaternion targetLeft, Quaternion targetRight)
    {
        Quaternion initialRotationLeft = doorLeft.localRotation;
        Quaternion initialRotationRight = doorRight.localRotation;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;
            doorLeft.localRotation = Quaternion.Slerp(initialRotationLeft, targetLeft, t);
            doorRight.localRotation = Quaternion.Slerp(initialRotationRight, targetRight, t);
            yield return null;
        }

        // Ensure exact final rotation
        doorLeft.localRotation = targetLeft;
        doorRight.localRotation = targetRight;
    }
}
