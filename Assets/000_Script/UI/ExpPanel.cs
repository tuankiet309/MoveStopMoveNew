using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpPanel : MonoBehaviour,IDataPersistence
{
    [SerializeField] int level = 0;
    [SerializeField] Image starImage;
    [SerializeField] int currentExp =0;
    [SerializeField] Slider slider;
    [SerializeField] Sprite[] sprites;
    private void Start()
    {
        CheckLevelUp();
        starImage.sprite = sprites[level];
        slider.value = (float)currentExp / 100;
    }
    private void CheckLevelUp()
    {
        if (level == sprites.Length - 1)
            return;
        if(currentExp>100)
        {
            level++;
            currentExp -= 100;
        }
    }

    public void LoadData(GameData gameData)
    {
        level = gameData.level;
        currentExp = gameData.currentExp;

    }

    public void SaveData(ref GameData gameData)
    {
        gameData.level = level;
    }
}
