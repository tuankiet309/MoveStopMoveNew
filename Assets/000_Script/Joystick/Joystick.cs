using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Joystick : Stick
{
    [SerializeField] private RectTransform joystickMoveArea;
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform thumbStick;

    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPos = eventData.position;
        Vector2 centerPos = background.position;
        Vector2 localOffset = Vector2.ClampMagnitude(touchPos - centerPos, background.sizeDelta.x / 2);
        thumbStick.position = centerPos + localOffset; 
        Vector2 inputVal = localOffset / (background.sizeDelta.x / 2);  
        onThumbstickValueChanged?.Invoke(inputVal);

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.position = eventData.position;
        thumbStick.position = background.position;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.position = joystickMoveArea.position;
        thumbStick.position = background.position;
        onThumbstickValueChanged?.Invoke(Vector2.zero);
    
    }
}
