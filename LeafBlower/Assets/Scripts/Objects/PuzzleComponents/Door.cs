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

    public bool toZero = true;

    public bool hasBeenActive = false;


    private void Start()
    {

        originalRotationLeft = doorLeft.localRotation;
        originalRotationRight = doorRight.localRotation;
        if(toZero)
        {
            targetRotationLeft = Quaternion.identity;
            targetRotationRight = Quaternion.identity;
        }
        LoadData();
    }

    public override void DoAction()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveDoors(targetRotationLeft, targetRotationRight));
        hasBeenActive = true;
    }

    public override void UndoAction()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveDoors(originalRotationLeft, originalRotationRight));
        hasBeenActive = false;
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

    private void OnDisable()
    {
        SaveData();
    }

    private void SaveData()
    {
        if(hasBeenActive)
        {
            PlayerPrefs.SetInt("Door_" + gameObject.name, 1);
        }
        else
        {
            PlayerPrefs.SetInt("Door_" + gameObject.name, 0);

        }
    }

    private void LoadData()
    {
        if(PlayerPrefs.GetInt("Door_" + gameObject.name) == 1)
        {
            DoAction();
        }
        else
        {
            UndoAction();
        }
    }
}
