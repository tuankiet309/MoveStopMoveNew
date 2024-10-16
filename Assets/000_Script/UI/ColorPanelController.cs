using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPanelController : MonoBehaviour
{
    private List<Button> buttonMat = new List<Button>(); 
    [SerializeField] private RectTransform materialHolder;
    [SerializeField] private Button materialPickButtonPref;
    [SerializeField] private GameObject[] ButtonColorHolder;
    private GameObject displayWeapon;
    private GameObject customVarientWeapon;

    private List<Button> buttonColor = new List<Button>(); 

    private void Start()
    {
        buttonColor.Clear();
        foreach (GameObject holder in ButtonColorHolder)
        {
            Button[] childButtons = holder.GetComponentsInChildren<Button>();
            buttonColor.AddRange(childButtons); 
        }
    }

    public void InitializeMaterialPickers(GameObject displayWeapon, GameObject customVarient)
    {   buttonMat.Clear();
        this.displayWeapon = displayWeapon;
        this.customVarientWeapon = customVarient;
        Material[] materials = displayWeapon.GetComponent<MeshRenderer>().sharedMaterials;

        foreach (Transform child in materialHolder)
        {
            Button materialButton = child.GetComponent<Button>();
            if (materialButton != null)
            {
                materialButton.onClick.RemoveAllListeners(); 
            }
            Destroy(child.gameObject); 
        }

        for (int i = 0; i < materials.Length; i++)
        {
            int currentIndex = i; 
            Button materialPicker = Instantiate(materialPickButtonPref, materialHolder);

            materialPicker.transform.GetChild(0).GetComponent<Image>().color = materials[currentIndex].color;

            materialPicker.onClick.AddListener(() => SelectMaterialForColorChange(currentIndex));

            materialPicker.onClick.AddListener(() => ChangeColor(materialPicker));

            buttonMat.Add(materialPicker);
        }
    }

    private void SelectMaterialForColorChange(int materialIndex)
    {
        foreach (Button btn in buttonColor)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => ApplyColorToMaterial(btn, materialIndex));
        }
    }

    private void ApplyColorToMaterial(Button btn, int materialIndex)
    {
        Image buttonImage = btn.GetComponent<Image>();
        Color selectedColor = buttonImage.color;

        Material[] previewMaterials = displayWeapon.GetComponent<MeshRenderer>().sharedMaterials;
        Material[] customVarientMaterials = customVarientWeapon.GetComponent<MeshRenderer>().sharedMaterials;

        previewMaterials[materialIndex].color = selectedColor;
        customVarientMaterials[materialIndex].color = selectedColor;

        displayWeapon.GetComponent<MeshRenderer>().materials = previewMaterials;
        customVarientWeapon.GetComponent<MeshRenderer>().materials = customVarientMaterials;

        Transform childImage = materialHolder.GetChild(materialIndex).GetChild(0);
        Image pickerImage = childImage.GetComponent<Image>();
        pickerImage.color = selectedColor;
    }

    private void ChangeColor(Button btn)
    {
        foreach (Button materialBtn in buttonMat)
        {
            materialBtn.GetComponent<Image>().color = Color.gray;
        }

        btn.image.color = Color.white;
    }
}