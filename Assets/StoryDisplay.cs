using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryDisplay : MonoBehaviour
{
    public GameObject panel;
    void Awake()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        string storyKey = "Story" + currentLevel.ToString();
        if(!PlayerPrefs.HasKey(storyKey))
        {
            panel.SetActive(true);
            PlayerPrefs.SetInt(storyKey, 1);
        }
    }

}
