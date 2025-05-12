using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public CameraSettings cameraSettings;
    public FlyingSettings flyingSettings;
    public static bool isJoystick = false;
    public GameObject joystick, draggingPanel;
    public TMP_Dropdown controlsDropdown;

    void Start()
    {
        joystick.SetActive(isJoystick);
        draggingPanel.SetActive(!isJoystick);

        controlsDropdown.value = isJoystick ? 0 : 1;

        cameraSettings.Initialize();

        CameraObject cameraObject = CameraSetup.instance.cameraObject;
        cameraSettings.SetCameraSettings(cameraObject);

        flyingSettings.Initialize();
    }

    public void SetControls(int val)
    {
        isJoystick = val == 0 ? true : false;
        joystick.SetActive(isJoystick);
        draggingPanel.SetActive(!isJoystick);
    }

}

[Serializable]
public class CameraSettings
{
    public MySlider lookaheadTime;
    public MySlider lookaheadSmoothing;
    public Toggle ignoreY;
    public MySlider horizontalDamping;
    public MySlider verticalDamping;

    public void Initialize()
    {
        lookaheadTime.Initialize();
        lookaheadSmoothing.Initialize();
        horizontalDamping.Initialize();
        verticalDamping.Initialize();
    }

    public void SetCameraSettings(CameraObject cameraObject)
    {
        lookaheadTime.SetValue(cameraObject.lookaheadTime);
        lookaheadSmoothing.SetValue(cameraObject.lookaheadSmoothing);
        ignoreY.isOn = cameraObject.ignoreY;
        horizontalDamping.SetValue(cameraObject.horizontalDamping);
        verticalDamping.SetValue(cameraObject.verticalDamping);
    }
}

[Serializable]
public class MySlider
{
    public Slider slider;
    public TextMeshProUGUI text;

    public void Initialize()
    {
        // Subscribe to the slider's value change event
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void SetValue(float value)
    {
        slider.value = value;
        text.text = value.ToString("F2");
    }

    private void OnSliderValueChanged(float value)
    {
        text.text = value.ToString("F2");
    }
}

[Serializable]
public class FlyingSettings
{
    public MySlider thrust;
    public MySlider turnSpeed;
    public MySlider maxTiltAngle;
    public MySlider maxPitchSpeed;

    public void Initialize()
    {
        thrust.Initialize();
        turnSpeed.Initialize();
        maxTiltAngle.Initialize();
        maxPitchSpeed.Initialize();
    }
}
