using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenTargetIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] targets;
    [SerializeField] private GameObject indicatorPreb;

    private SpriteRenderer spriteRenderer;
    private float _spriteWitdh;
    private float _spriteHitdh;

    private Camera _camera;
    private Dictionary<GameObject, GameObject> _targetIndicators = new Dictionary<GameObject, GameObject>();
    private void Start()
    {
        _camera = Camera.main;
        spriteRenderer = indicatorPreb.GetComponent<SpriteRenderer>();
        var bounds = spriteRenderer.bounds;
        _spriteHitdh = bounds.size.y / 2f;

    }
}
