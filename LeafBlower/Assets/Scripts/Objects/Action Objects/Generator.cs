using UnityEngine;

public class Generator : NormalObject
{
    public GameObject prefab;
    public Transform instantiationPoint;

    public GameObject instantiatedPrefab;

    public override void StartAspiring(Transform target, Transform firePoint)
    {
        Destroy(instantiatedPrefab);
        instantiatedPrefab = Instantiate(prefab, instantiationPoint.position, Quaternion.identity);
    }
}
