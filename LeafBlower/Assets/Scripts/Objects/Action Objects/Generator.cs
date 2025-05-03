using UnityEngine;

public class Generator : NormalObject
{
    public GameObject prefab;
    public Transform instantiationPoint;

    public GameObject instantiatedPrefab;
    bool _canInstantiateNewObject = true;
    protected override void Update()
    {
        base.Update();
        if(instantiatedPrefab != null)
        {
            if (Vector3.Distance(instantiatedPrefab.transform.position, transform.position) >= 5f)
            {
                _canInstantiateNewObject = true;
            }
            else
            {
                _canInstantiateNewObject = false;
            }
        }

    }

    public override void StartAspiring(Transform target, Transform firePoint)
    {
        if (!_canInstantiateNewObject) return;
        if(instantiatedPrefab != null)
        {
            Destroy(instantiatedPrefab);
        }
        instantiatedPrefab = Instantiate(prefab, instantiationPoint.position, Quaternion.identity);
    }
}
