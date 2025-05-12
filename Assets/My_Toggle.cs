using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class My_Toggle : MonoBehaviour
{
    private Toggle toggle;
    public GameObject isOn, isOff;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    public void ToggleOn()
    {
        isOn.SetActive(toggle.isOn);
        isOff.SetActive(!toggle.isOn);

        if(PlayerPrefs.HasKey("unlocked"))
            FindObjectOfType<PaperPlaneController>().SwitchMode(toggle.isOn);
        else
            AdMobManager.Instance.ShowRewardedAd();

    }

    public void SwitchMode()
    {
        FindObjectOfType<PaperPlaneController>().SwitchMode(toggle.isOn);
    }
}
