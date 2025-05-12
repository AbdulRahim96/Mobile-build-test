using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "ScriptableObjects/CameraSettings")]
public class CameraObject : ScriptableObject
{
    [Header("Composer Settings")]
    public float lookaheadTime = 0f;
    public float lookaheadSmoothing = 0f;
    public bool ignoreY = false;
    public float horizontalDamping = 0.5f;
    public float verticalDamping = 0.5f;

}
