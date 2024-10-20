using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealMoneyUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button closeButton;
    private void Start()
    {
        closeButton.onClick.AddListener(CloseSelf);
    }
    private void CloseSelf()
    {
        gameObject.SetActive(false);
    }
}
