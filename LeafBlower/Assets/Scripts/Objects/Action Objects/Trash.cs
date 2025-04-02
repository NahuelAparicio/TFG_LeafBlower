using System.Collections;
using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] private Vector3 _targetRotation;
    [SerializeField] private float _duration = 1f;    
    [SerializeField] private float _speed = 180f;      
    public void ActiveRotation()
    {
        StopAllCoroutines();
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        Quaternion startRotation = transform.localRotation;
        Quaternion endRotation = Quaternion.Euler(_targetRotation);

        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float step = _speed * Time.deltaTime;

            transform.localRotation = Quaternion.RotateTowards(
                transform.localRotation,
                endRotation,
                step
            );

            yield return null;
        }

        transform.localRotation = endRotation; 
    }
}
