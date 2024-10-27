using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionCircle : MonoBehaviour
{
    private float circleRadius = 0;

    [SerializeField] private Transform centerOfCircle;
    [SerializeField] private SphereCollider colliderToChanged;

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
    }

  
}