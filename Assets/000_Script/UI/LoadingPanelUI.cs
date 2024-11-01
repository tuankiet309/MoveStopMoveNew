using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanelUI : MonoBehaviour
{
    [SerializeField] private RectTransform spinningImage;
    [SerializeField] private float rotationSpeed = 400f;

    private void Update()
    {
        spinningImage.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }
}