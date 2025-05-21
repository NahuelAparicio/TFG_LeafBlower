using System.Collections.Generic;
using UnityEngine;

public class ObjectTransparencyHandler : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> _meshes = new();
    private List<Material> _materials = new();
    [SerializeField][Range(0,1f)] private float _matTransparency;
    private Dictionary<Material, float> _originalAlphas = new();


    private void Awake()
    {
        if(_materials.Count != 0)
        {
            foreach (var mesh in _meshes)
            {
                if(mesh != null)
                    _materials.AddRange(mesh.materials);
            }

        }
    }
    public void EnableTransparency()
    {
        foreach (var mat in _materials)
        {
            if(!_originalAlphas.ContainsKey(mat))
            {
                _originalAlphas[mat] = mat.color.a;
            }

            Color c = mat.color;
            c.a = _matTransparency;
            mat.color = c;
        }
    }

    public void DisableTransparency()
    {
        foreach (var mat in _materials)
        {
            Color c = mat.color;
            if (_originalAlphas.ContainsKey(mat))
            {
                c.a = _originalAlphas[mat];
            }
            else
            {
                c.a = 1;
            }
            mat.color = c;
        }
    }
}
