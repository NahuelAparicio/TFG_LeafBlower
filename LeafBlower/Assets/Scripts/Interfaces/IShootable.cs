using UnityEngine;

public interface IShooteable 
{    
    public void OnShoot(float force, Vector3 direction);
    public void AttachObject();
}
