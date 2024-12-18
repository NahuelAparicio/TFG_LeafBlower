using UnityEngine;

public interface IShooteable : IOutlineable
{    
    public void OnShoot(Vector3 force);
    public void OnRotate(Vector3 axis);
}
