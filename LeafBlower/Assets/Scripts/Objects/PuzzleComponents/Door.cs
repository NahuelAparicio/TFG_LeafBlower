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
        targetRotationLeft = Quaternion.identity;
        targetRotationRight = Quaternion.identity;
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

        float duration = 1f / rotationSpeed; // Duración basada en la velocidad
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // Asegura un valor entre 0 y 1

            doorLeft.localRotation = Quaternion.Slerp(initialRotationLeft, targetLeft, t);
            doorRight.localRotation = Quaternion.Slerp(initialRotationRight, targetRight, t);

            yield return null;
        }

        // Asegurar la rotación final exacta
        doorLeft.localRotation = targetLeft;
        doorRight.localRotation = targetRight;
    }
}
