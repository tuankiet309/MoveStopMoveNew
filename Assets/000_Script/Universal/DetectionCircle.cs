using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionCircle : MonoBehaviour
{
    private float circleRadius;
    private float circleRadiusIncreaser;

    [SerializeField] private Transform centerOfCircle;
    [SerializeField] private SphereCollider colliderToChanged;

    public float CircleRadius { get => circleRadius; }

    public delegate void OnTriggerContact(GameObject attacker, bool isIn);
    public event OnTriggerContact onTriggerContact;


    private void Awake()
    {
        circleRadius = CONSTANT_VALUE.FIRST_CIRCLE_RADIUS;
        circleRadiusIncreaser = CONSTANT_VALUE.CIRCLE_RADIUS_INCREASER;
        colliderToChanged.radius = circleRadius;
    }

    private void Start() { }

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
    }

  
}