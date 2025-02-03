using System.Collections.Generic;
using UnityEngine;
public class Utils 
{
    public const float Epsilon = 0.001f;
    
    public static Vector3 GetCameraForwardNormalized(Camera cam)
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    public static Vector3 GetCameraRightNormalized(Camera cam)
    {
        Vector3 right = cam.transform.right;
        right.y = 0;
        return right.normalized;
    }

    public static void AddToList<T>(List<T> list, T obj)
    {
        if(!list.Contains(obj))
        {
            list.Add(obj);
        }
    }

    public static void RemoveFromList<T>(List<T> list, T obj)
    {
        if (list.Contains(obj))
        {
            list.Remove(obj);
        }
    }
}

