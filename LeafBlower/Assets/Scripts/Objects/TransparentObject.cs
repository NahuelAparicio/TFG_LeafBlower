using System.Collections.Generic;
using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_PlayerPos");
    public static int IsTransparentID = Shader.PropertyToID("_IsTransparent");

    private Material _material;
    private Transform _player;
    private bool _isTransparent = false;
    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>().transform;
        _material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        var dir = _player.position - Camera.main.transform.position;
        var ray = new Ray(Camera.main.transform.position, dir);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, dir.magnitude))
        {
            if(hit.collider.gameObject == gameObject)
            {
                if(!_isTransparent)
                {
                    SetTransparency(true);
                }
            }
            else
            {
                if(_isTransparent)
                {
                    SetTransparency(false);
                }
            }
        }
        
        if (!_isTransparent) return;

        SetPlayerPos();

    }

    private void SetTransparency(bool makeTransparent)
    {
        _material.SetInt("_IsTransparent", makeTransparent ? 1 : 0);
        _isTransparent = makeTransparent;
    }

    private void SetPlayerPos()
    {
        var view = Camera.main.WorldToViewportPoint(_player.position);
        _material.SetVector(PosID, view);
    }
}
