using System.Collections;
using UnityEngine;

public class LoadingScreenUI : MonoBehaviour
{
    public RectTransform firstPanel;          
    public RectTransform[] otherPanels;

    public void ShowRandomPanel() 
    {
        firstPanel.gameObject.SetActive(false);
        if (otherPanels.Length > 0)
        {
            int randomIndex = Random.Range(0, otherPanels.Length);
            otherPanels[randomIndex].gameObject.SetActive(true);
        }
    }

}