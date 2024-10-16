using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Stick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent<Vector2> onThumbstickValueChanged;
    public virtual void OnDrag(PointerEventData eventData)
    {
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
    }
}
