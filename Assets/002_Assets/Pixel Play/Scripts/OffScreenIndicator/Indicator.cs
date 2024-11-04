using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Assign this script to the indicator prefabs.
/// </summary>
public class Indicator : MonoBehaviour
{
    [SerializeField] private IndicatorType indicatorType;
    private Image indicatorImage;
    private Text distanceText;
    public GameObject arrowHolder;
    public Image arrowImage;

    public bool Active
    {
        get
        {
            return transform.gameObject.activeInHierarchy;
        }
    }

    public IndicatorType Type
    {
        get
        {
            return indicatorType;
        }
    }
    public GameObject GetArrow()
    {
        return arrowHolder;
    }
    void Awake()
    {
        indicatorImage = transform.GetComponent<Image>();
        distanceText = transform.GetComponentInChildren<Text>();
    }


    public void SetImageColor(Color color)
    {
        indicatorImage.color = new Color(color.r,color.g,color.b,0.6f);
        if(indicatorType == IndicatorType.ARROW)
            arrowImage.color = indicatorImage.color;
    }


    public void SetDistanceText(string value)
    {
        distanceText.text = value;
    }


    public void SetTextRotation(Quaternion rotation)
    {
        distanceText.rectTransform.rotation = rotation;
    }

    public void Activate(bool value)
    {
        transform.gameObject.SetActive(value);
    }
}

public enum IndicatorType
{
    BOX,
    ARROW
}
