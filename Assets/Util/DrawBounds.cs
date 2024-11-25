using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))] [ExecuteAlways]
public class DrawBounds : MonoBehaviour
{
    public enum DrawType
    {
        Fill,
        Wire
    }

    public Color color = Color.yellow;
    public DrawType drawType = DrawType.Wire;

    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    void OnDrawGizmos()
    {
        if(_renderer == null) return;

        Gizmos.color = color;

        switch (drawType)
        {
            case DrawType.Fill:
                Gizmos.DrawCube(_renderer.bounds.center, _renderer.bounds.size);
                break;
            case DrawType.Wire:
                Gizmos.DrawWireCube(_renderer.bounds.center, _renderer.bounds.size);
                break;
        }
    }
}
