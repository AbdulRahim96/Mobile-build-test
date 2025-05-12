using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    public static CameraSetup instance;
    public CameraObject cameraObject;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineComposer composer;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
    }

    private void Start()
    {
        composer.m_LookaheadTime = cameraObject.lookaheadTime;
        composer.m_LookaheadSmoothing = cameraObject.lookaheadSmoothing;
        composer.m_LookaheadIgnoreY = cameraObject.ignoreY;
        composer.m_HorizontalDamping = cameraObject.horizontalDamping;
        composer.m_VerticalDamping = cameraObject.verticalDamping;

        GameManager.GameOverWithSuccessCallback += Complete;
    }


    public void SetLookaheadTime(float time)
    {
        cameraObject.lookaheadTime = time;
        composer.m_LookaheadTime = time;
    }
    public void SetLookaheadSmoothing(float smoothing)
    {
        cameraObject.lookaheadSmoothing = smoothing;
        composer.m_LookaheadSmoothing = smoothing;
    }
    public void SetIgnoreY(bool ignore)
    {
        cameraObject.ignoreY = ignore;
        composer.m_LookaheadIgnoreY = ignore;
    }
    public void SetHorizontalDamping(float damping)
    {
        cameraObject.horizontalDamping = damping;
        composer.m_HorizontalDamping = damping;
    }
    public void SetVerticalDamping(float damping)
    {
        cameraObject.verticalDamping = damping;
        composer.m_VerticalDamping = damping;
    }

    public void SetCameraObject(CameraObject newCameraObject)
    {
        cameraObject = newCameraObject;
        composer.m_LookaheadTime = cameraObject.lookaheadTime;
        composer.m_LookaheadSmoothing = cameraObject.lookaheadSmoothing;
        composer.m_LookaheadIgnoreY = cameraObject.ignoreY;
        composer.m_HorizontalDamping = cameraObject.horizontalDamping;
        composer.m_VerticalDamping = cameraObject.verticalDamping;
    }

    public void SetTarget(Transform target)
    {
        virtualCamera.m_LookAt = target;
        virtualCamera.m_Follow = target;
    }

    void Complete(bool isSuccess)
    {
        virtualCamera.m_LookAt = null;
        virtualCamera.m_Follow = null;
      /*  if (isSuccess)
        {
            virtualCamera.transform.DORotate(new Vector3(-25, virtualCamera.transform.eulerAngles.y, 0), 2).SetDelay(1).SetEase(Ease.InOutQuad);
        }*/
    }
}
