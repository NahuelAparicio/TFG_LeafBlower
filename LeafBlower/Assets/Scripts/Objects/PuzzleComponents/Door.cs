using System.Collections;
using UnityEngine;

public class Door : IActivable
{
    public Transform doorLeft, doorRight;
    public float rotationSpeed;

    public override void DoAction()
    {
        StopAllCoroutines();
        StartCoroutine(MoveDoors(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 0)));

    }

    public override void UndoAction()
    {
        //Close Door
        StopAllCoroutines();
        StartCoroutine(MoveDoors(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 0)));
    }

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
