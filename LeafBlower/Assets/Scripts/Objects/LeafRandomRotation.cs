using UnityEngine;

// Hace que el script se ejecute en el editor y en tiempo de juego
[ExecuteInEditMode]
public class LeafRandomRotation : MonoBehaviour
{
    void Start()
    {
        // Aplica una rotaci�n aleatoria en el eje Y entre 0 y 360 grados
        transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

        // Deshabilita el script despu�s de ejecutarse una vez para no repetir la rotaci�n
        this.enabled = false;
    }

    // Para asegurar que se actualice tambi�n en el editor
    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        }
    }
}