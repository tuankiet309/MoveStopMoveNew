using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SettingButton : MonoBehaviour
{
    [SerializeField] protected RectTransform onImage;
    [SerializeField] protected RectTransform offImage;
    [SerializeField] protected Button button;

    private void OnEnable()
    {
        button.onClick.AddListener(ButtonDo);
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(ButtonDo);
    }
    protected abstract void ButtonDo();

}
