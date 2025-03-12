using UnityEngine;

public class HUDStickerRotation : MonoBehaviour
{
    private enum WaveType { Sin, Cos }

    [Header("Rotación en X")]
    [SerializeField] private float amplitudeX = 10f;
    [SerializeField] private float frequencyX = 1f;
    [SerializeField] private float offsetX = 0f;
    [SerializeField] private WaveType waveTypeX = WaveType.Sin;

    [Header("Rotación en Y")]
    [SerializeField] private float amplitudeY = 10f;
    [SerializeField] private float frequencyY = 1f;
    [SerializeField] private float offsetY = 0f;
    [SerializeField] private WaveType waveTypeY = WaveType.Sin;

    [Header("Rotación en Z")]
    [SerializeField] private float amplitudeZ = 10f;
    [SerializeField] private float frequencyZ = 1f;
    [SerializeField] private float offsetZ = 0f;
    [SerializeField] private WaveType waveTypeZ = WaveType.Sin;

    private RectTransform rectTransform;
    private Material material;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Asegura que el pivote esté en el centro (0.5, 0.5)
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        // Obtiene el material del CanvasRenderer
        material = GetComponent<CanvasRenderer>().GetMaterial();
    }

    void Update()
    {
        float time = Time.time;

        // Calcula la rotación en cada eje, usando Sin o Cos según el WaveType
        float rotationX = GetWaveValue(waveTypeX, time * frequencyX + offsetX) * amplitudeX;
        float rotationY = GetWaveValue(waveTypeY, time * frequencyY + offsetY) * amplitudeY;
        float rotationZ = GetWaveValue(waveTypeZ, time * frequencyZ + offsetZ) * amplitudeZ;

        // Aplica la rotación al RectTransform
        rectTransform.localEulerAngles = new Vector3(rotationX, rotationY, rotationZ);

        // Actualiza la variable "rotation" en el Shader Graph
        if (material != null)
        {
            material.SetVector("_Rotation", new Vector2(rotationY, rotationZ));
        }
    }

    private float GetWaveValue(WaveType waveType, float value)
    {
        return waveType == WaveType.Sin ? Mathf.Sin(value) : Mathf.Cos(value);
    }
}