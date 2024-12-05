using System.Collections;
using UnityEngine;

public class Door : IActivable
{

    public Transform doorLeft, doorRight;
    public float rotationSpeed;

    public override void DoAction()
    {
        StopAllCoroutines();
        StartCoroutine(OpenDoors());

    }

    public override void UndoAction()
    {
        //Close Door
        StopAllCoroutines();
        StartCoroutine(CloseDoors());
    }

    private IEnumerator OpenDoors()
    {

        // Get the initial and target rotations
        Quaternion initialRotationLeft = doorLeft.localRotation;
        Quaternion initialRotationRight = doorRight.localRotation;
        Quaternion targetRotationLeft = Quaternion.Euler(0, -90, 0);  // Target rotation for left door
        Quaternion targetRotationRight = Quaternion.Euler(0, 90, 0);  // Target rotation for right door

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;

            // Smoothly interpolate from initial to target rotations
            doorLeft.localRotation = Quaternion.Slerp(initialRotationLeft, targetRotationLeft, t);
            doorRight.localRotation = Quaternion.Slerp(initialRotationRight, targetRotationRight, t);

            yield return null; // Wait until the next frame
        }

    }

    private IEnumerator CloseDoors()
    {

        Quaternion initialRotationLeft = doorLeft.localRotation;
        Quaternion initialRotationRight = doorRight.localRotation;
        Quaternion targetRotationLeft = Quaternion.Euler(0, 0, 0);  
        Quaternion targetRotationRight = Quaternion.Euler(0, 0, 0);  

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;

            // Smoothly interpolate from initial to target rotations
            doorLeft.localRotation = Quaternion.Slerp(initialRotationLeft, targetRotationLeft, t);
            doorRight.localRotation = Quaternion.Slerp(initialRotationRight, targetRotationRight, t);

            yield return null; // Wait until the next frame
        }

    }
}
