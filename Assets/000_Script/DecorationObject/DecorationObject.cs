using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationObject : MonoBehaviour
{
    [SerializeField] Renderer _renderer;
    Color originalColor;
    private void Start()
    {
        originalColor = _renderer.material.color;
    }

    public void ChangeTransparentValue(bool Check)
    {      
        Color color = _renderer.material.color;
        if (Check)
        {
            color = new Color(0,0,0,CONSTANT_VALUE.DECORATION_TRANPARENT_VALUE);
        }
        else
        {
            color = originalColor;
        }
        _renderer.material.color = color;
    }
}
