using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class NonVisualJoyStick : Stick
{
    Vector2 touchPos = Vector2.zero;
    Vector2 currentPos = Vector2.zero;
    public override void OnDrag(PointerEventData eventData)
    {
        
        currentPos = eventData.position;
        Vector2 inputVal = currentPos - touchPos;
        onThumbstickValueChanged?.Invoke(inputVal);

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        touchPos = eventData.position;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        onThumbstickValueChanged?.Invoke(Vector2.zero);
    }
}
