using UnityEngine;

public interface IAspirable : IOutlineable
{
    public void OnAspiratableInteracts(Vector3 force);
}
