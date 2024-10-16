using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUIController : MonoBehaviour
{
    [SerializeField] Button PlayZone2;

    private void Start()
    {
        PlayZone2.onClick.AddListener(GoToZone2);
    }
    private void GoToZone2()
    {
        GameManager.Instance.SetGameState(Enum.GameState.Zone2);
    }
}
