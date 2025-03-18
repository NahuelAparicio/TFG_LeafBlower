using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Graphic))]
public class UIPerspectiveDeformation : BaseMeshEffect
{
    // Desplazamientos para cada esquina
    public Vector3 topLeftOffset;
    public Vector3 topRightOffset;
    public Vector3 bottomLeftOffset;
    public Vector3 bottomRightOffset;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;

        List<UIVertex> verts = new List<UIVertex>();
        vh.GetUIVertexStream(verts);

        if (verts.Count == 0) return;

        // Obtener el rect�ngulo del elemento UI
        Rect rect = graphic.rectTransform.rect;

        for (int i = 0; i < verts.Count; i++)
        {
            UIVertex v = verts[i];

            // Normalizar la posici�n del v�rtice en funci�n del rect�ngulo
            float normalizedX = (v.position.x - rect.x) / rect.width;
            float normalizedY = (v.position.y - rect.y) / rect.height;

            // Interpolar entre los desplazamientos de las esquinas seg�n la posici�n normalizada
            Vector3 topOffset = Vector3.Lerp(topLeftOffset, topRightOffset, normalizedX);
            Vector3 bottomOffset = Vector3.Lerp(bottomLeftOffset, bottomRightOffset, normalizedX);
            Vector3 finalOffset = Vector3.Lerp(bottomOffset, topOffset, normalizedY);

            v.position += finalOffset;
            verts[i] = v;
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(verts);
    }
}
