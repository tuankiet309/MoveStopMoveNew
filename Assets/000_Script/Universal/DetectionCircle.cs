using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DetectionCircle : MonoBehaviour
{
    private float circleRadius = 0;

    [SerializeField] private Transform centerOfCircle;
    [SerializeField] private SphereCollider colliderToChanged;
    [SerializeField] protected RectTransform visualizeCircle;

    public float CircleRadius { get => circleRadius; }

    public UnityEvent<GameObject,bool> onTriggerContact;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(transform.tag)) return;

        DecorationObject decorationObject = other.GetComponent<DecorationObject>();
        IAttacker attacker = other.GetComponent<IAttacker>();

        if (decorationObject != null && transform.parent.CompareTag("Player"))
        {
            decorationObject.ChangeTransparentValue(true);
        }
        else if (attacker != null)
        {
            onTriggerContact?.Invoke(other.gameObject, true);
            Debug.Log("Enemy comming in");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(transform.tag)) return;

        DecorationObject decorationObject = other.GetComponent<DecorationObject>();
        IAttacker attacker = other.GetComponent<IAttacker>();

        if (decorationObject != null && transform.parent.CompareTag("Player"))
        {
            decorationObject.ChangeTransparentValue(false);
        }
        else if (attacker != null)
        {
            onTriggerContact?.Invoke(other.gameObject, false);
        }
    }
    public void UpdateCircleRadius(float howMuc)
    {
        circleRadius += howMuc;
        colliderToChanged.radius = circleRadius;
        if (visualizeCircle != null)
            visualizeCircle.sizeDelta = new Vector2(circleRadius * 2, circleRadius * 2);
    }

  
}