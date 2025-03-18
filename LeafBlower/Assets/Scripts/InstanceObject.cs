using UnityEngine;

public class InstanceObject : MonoBehaviour
{
    [SerializeField] private GameObject _objectToInstance;
    [SerializeField] private Vector3 _offset;

    private GameObject generatedGo;
    public void OnInstanceObject()
    {
        if(generatedGo != null) 
            Destroy(generatedGo);

        generatedGo = Instantiate(_objectToInstance, transform.position + _offset, Quaternion.identity);
    }
}
