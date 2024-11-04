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
        UpdateVisual();
    }

    private void UpdateVisual()
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
        if(gameData.level == sprites.Length-1)
        {
            level = gameData.level;
            currentExp = gameData.currentExp;
            return;
        }
        if(gameData.currentExp >100)
        {
            gameData.level++;
            gameData.currentExp -= 100;
        }
        level = gameData.level;
        currentExp = gameData.currentExp;
        UpdateVisual();
    }

    public void SaveData(ref GameData gameData)
    {
        //if (level == sprites.Length - 1)
        //    return;
        //if (currentExp > 100)
        //{
        //    level++;
        //    currentExp -= 100;
        //}
        //gameData.level = level;
        //gameData.currentExp = currentExp;
    }
}
