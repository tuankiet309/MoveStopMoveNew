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

    public void ChangeTransparentValue(bool check)
    {
        Color color = _renderer.material.color;

        if (check)
        {
            color = new Color(originalColor.r, originalColor.g, originalColor.b, CONSTANT_VALUE.DECORATION_TRANPARENT_VALUE);
            SetMaterialToFade(); 
        }
        else
        {
            color = originalColor;
            SetMaterialToOpaque(); 
        }

        _renderer.material.color = color;
    }

    private void SetMaterialToFade()
    {
        _renderer.material.SetFloat("_Mode", 2);
        _renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _renderer.material.SetInt("_ZWrite", 0);
        _renderer.material.DisableKeyword("_ALPHATEST_ON");
        _renderer.material.EnableKeyword("_ALPHABLEND_ON");
        _renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        _renderer.material.renderQueue = 3000;
    }

    private void SetMaterialToOpaque()
    {
        _renderer.material.SetFloat("_Mode", 0);
        _renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        _renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        _renderer.material.SetInt("_ZWrite", 1);
        _renderer.material.EnableKeyword("_ALPHATEST_ON");
        _renderer.material.DisableKeyword("_ALPHABLEND_ON");
        _renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        _renderer.material.renderQueue = -1;
    }
}
