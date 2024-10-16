using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIWorldSpace : MonoBehaviour
{
    private void Start()
    {
    }

    private void LateUpdate()
    {
        Vector3 lookAtPosition = Camera.main.transform.position - transform.position;

        Vector3 lookDirection = new Vector3(0, -lookAtPosition.y, -lookAtPosition.z);

        transform.rotation = Quaternion.LookRotation(lookDirection);
    }
}