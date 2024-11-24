using UnityEngine;

public interface IBlowable :IOutlineable
{
    public void OnBlowableInteracts(Vector3 force, Vector3 point);
}
