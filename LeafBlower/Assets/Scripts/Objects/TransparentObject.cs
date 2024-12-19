using System.Collections.Generic;
using UnityEngine;

public class TransparentObject : MonoBehaviour, ITransparencyHandler
{
    public static int PosID = Shader.PropertyToID("_PlayerPos");
    public static int SizeID = Shader.PropertyToID("_Size");

    public bool isTransparent = false;
    [SerializeField] private List<Material> materials = new List<Material>();
    private Transform _player;
    public float transparencyRadius = 5;
    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {

        var dir = _player.position - Camera.main.transform.position;
        var ray = new Ray(Camera.main.transform.position, dir);
        if(Physics.Raycast(ray, 3000))
        {
            foreach (Material mat in materials)
            {
                mat.SetFloat(SizeID, 1);
            }
        }
        else
        {
            foreach (Material mat in materials)
            {
                mat.SetFloat(SizeID, 0);
            }
        }
        var view = Camera.main.WorldToViewportPoint(_player.position);
        foreach (Material mat in materials)
        {
            mat.SetVector(PosID, view);
        }

    }
    public void DisableTransparent()
    {
        //if (!isTransparent) return;

        //foreach (Material mat in materials)
        //{
        //    Color color = mat.color;
        //    color.a = 1.0f; // Opacidad (completamente opaco)
        //    mat.color = color;

        //    // Restaura el material a opaco
        //    //mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        //    //mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        //    //mat.SetInt("_ZWrite", 1);
        //    //mat.DisableKeyword("_ALPHABLEND_ON");
        //    //mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
        //}
    }

    public void EnableTransparent()
    {
        //if (isTransparent) return;
        //isTransparent = true;
        //foreach (Material mat in materials)
        //{
        //    Color color = mat.color;
        //    color.a = 0.5f; // Opacidad (50%)
        //    mat.color = color;
        //    // Asegura que el material sea transparente
        //    //mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        //    //mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        //    //mat.SetInt("_ZWrite", 0);
        //    //mat.EnableKeyword("_ALPHABLEND_ON");
        //    //mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        //}
    }
}
