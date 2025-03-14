using UnityEngine;

[ExecuteAlways] // Ejecuta el script en el editor y en tiempo de ejecución
public class HouseColorPick : MonoBehaviour
{
    private Renderer rend;
    private MaterialPropertyBlock mpb;

    [SerializeField] private Color objectColor = Color.white; // Color ajustable desde el inspector

    void OnEnable()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
        UpdateColor();
    }

    // Aplica el color elegido al MaterialPropertyBlock
    private void UpdateColor()
    {
        if (rend == null || mpb == null) return;

        mpb.SetColor("_PlasterTintColor", objectColor);
        rend.SetPropertyBlock(mpb);
    }

    // Permite cambiar el color en tiempo de ejecución
    public void SetColor(Color newColor)
    {
        objectColor = newColor;
        UpdateColor();
    }

    // Actualiza el color en el editor cuando cambias la propiedad
    private void OnValidate()
    {
        if (rend == null)
            rend = GetComponent<Renderer>();

        if (mpb == null)
            mpb = new MaterialPropertyBlock();

        UpdateColor();
    }
}
