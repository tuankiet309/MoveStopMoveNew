using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : SettingButton
{
    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (SoundManager.Instance.IsMuted)
        {
            onImage.gameObject.SetActive(false);
            offImage.gameObject.SetActive(true);
        }
        else
        {
            onImage.gameObject.SetActive(true);
            offImage.gameObject.SetActive(false);
        }
    }

    protected override void ButtonDo()
    {
        SoundManager.Instance.ToggleMute();
        UpdateVisual();
    }

}
