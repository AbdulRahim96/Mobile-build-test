using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Button[] levelButtons;
    public int currentUnlockedLevel;
    // Start is called before the first frame update
    void Start()
    {
        currentUnlockedLevel = PlayerPrefs.GetInt("CurrentUnlockedLevel", 1);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > currentUnlockedLevel)
            {
                levelButtons[i].interactable = false;
            }
            else
            {
                levelButtons[i].interactable = true;
            }
        }
    }

    [Button("Overwrite")]
    public void Unlock(int val)
    {
        PlayerPrefs.SetInt("CurrentSavedLevel", val);
        PlayerPrefs.Save();
    }

    public void EnableTest(bool flag)
    {
        if(flag)
        {
            for (int i = 0; i < levelButtons.Length; i++)
            {
                levelButtons[i].interactable = true;
            }
        }
        else
            Start();
    }
}
