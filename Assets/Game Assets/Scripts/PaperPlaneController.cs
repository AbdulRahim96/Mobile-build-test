using Arikan;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class PaperPlaneController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Thrust Settings")]
    public float thrustForce = 10f;
    public float maxSpeed = 20;
    public float acceleration = 20f;
    [SerializeField] private float currentSpeed;
    public float audioPitchFactor = 20;
    public float audioPitchMultiplier = 1.2f;

    [Header("Tilt Settings")]
    public float tiltSpeed = 100f;
    public float maxTiltAngle = 60f;

    [Header("Pitch Settings")]
    public float pitchSpeed = 100f;
    public float maxPitchAngle = 45f;
    public float pitchReturnSpeed = 5f;

    private float currentPitch = 0f;
    private float targetPitch = 0f;
    private float currentTilt = 0f;
    private float baseYrotation;

    private AudioSource audioSource;

    public Vector2 _Input;
    public static Action<Vector2> inputCallback;

    public AudioClip hitSound;

    private Tween forceTween;
    [SerializeField] private float dragCoefficient = 0.1f;
    private bool canConrol = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        canConrol = true;
        rb.useGravity = true;
        baseYrotation = transform.eulerAngles.y;
        currentTilt = 0;
        // Launch the plane once
        // rb.AddForce(transform.forward * thrustForce, ForceMode.Impulse);

        inputCallback = HandleInput;

        GameManager.GameOverWithSuccessCallback += (isSuccess) =>
        {
            canConrol = false;
            GetComponent<PaperPlaneController>().enabled = false;
        };

    }

    void FixedUpdate()
    {
        if (!canConrol) return;

        ApplyThrust();
        HandleRotation();
    }


    void ApplyThrust()
    {
        Vector3 forward = transform.forward;
        Vector3 velocity = rb.linearVelocity;

        // 1. Apply continuous forward thrust (low force)
        rb.AddForce(forward * thrustForce, ForceMode.Force);

        Vector3 drag = -velocity.normalized * dragCoefficient * velocity.sqrMagnitude;
        rb.AddForce(drag, ForceMode.Force);

        // 3. Optional: update speed for UI or debugging
        currentSpeed = rb.linearVelocity.magnitude;
        audioSource.pitch = Mathf.Lerp(audioSource.pitch, (currentSpeed / audioPitchFactor) * audioPitchMultiplier, currentSpeed / maxSpeed);
    }

    void HandleInput(Vector2 input)
    {
        // INPUTS
       /* _Input.x = Input.GetAxis("Horizontal");
        _Input.y = Input.GetAxis("Vertical");*/
       _Input = input;
    }

    void HandleRotation()
    {

        // Accumulate tilt freely based on input
        if (Mathf.Abs(this._Input.x) > 0.01f)
        {
            currentTilt += this._Input.x * tiltSpeed;
        }
        

        float pitchTarget = Mathf.Abs(_Input.y) > 0.01f ? _Input.y * maxPitchAngle : 0f;

        // LERP toward targets
       // currentTilt = Mathf.Lerp(currentTilt, tiltTarget, Time.deltaTime * (horizontalInput == 0 ? returnSpeed : tiltSpeed));
        currentPitch = Mathf.Lerp(currentPitch, pitchTarget, Time.deltaTime * (_Input.y == 0 ? pitchReturnSpeed : pitchSpeed));

        // CLAMP
       // currentTilt = Mathf.Clamp(currentTilt, -maxTiltAngle, maxTiltAngle);
        currentPitch = Mathf.Clamp(currentPitch, -maxPitchAngle, maxPitchAngle);

        // APPLY: Construct new rotation relative to the starting direction
        Quaternion targetRotation = Quaternion.Euler(currentPitch, baseYrotation + currentTilt, transform.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);


        Transform child = transform.GetChild(0);
        Quaternion ChildtargetRotation = Quaternion.Euler(child.transform.eulerAngles.x, child.transform.eulerAngles.y, -_Input.x * maxTiltAngle);
        child.transform.rotation = Quaternion.Lerp(child.transform.rotation, ChildtargetRotation, Time.deltaTime * 5f);
    }

    public void ApplyForce(float drag)
    {
        float from = drag;
        dragCoefficient = drag;

        if (forceTween != null)
        {
            forceTween.Kill();
        }

        forceTween = DOTween.To(() => from, x => from = x, 0.1f, 5)
        .SetEase(Ease.Linear)
               .OnUpdate(() =>
               {
                   dragCoefficient = from;
               }
               );
    }

    void HandlePitch()
    {
        float verticalInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(verticalInput) > 0.01f)
        {
            targetPitch = verticalInput * maxPitchAngle;
        }
        else
        {
            targetPitch = 0f;
        }

        currentPitch = Mathf.Lerp(currentPitch, targetPitch, Time.deltaTime * pitchReturnSpeed);

        // Apply pitch on X axis
        Quaternion currentRotation = Quaternion.Euler(currentPitch, transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, Time.deltaTime * pitchSpeed);
    }

    void ApplyDrag()
    {
        Vector3 velocity = rb.linearVelocity;


        // Optional: update speed for display or UI
        currentSpeed = velocity.magnitude;
    }

    public void SetThrustForce(float thrust)
    {
        thrustForce = thrust;
    }
    public void SetTiltSpeed(float speed)
    {
        tiltSpeed = speed;
    }
    public void SetMaxTiltAngle(float angle)
    {
        maxTiltAngle = angle;
    }
    public void SetPitchSpeed(float speed)
    {
        pitchSpeed = speed;
    }

    private void OnDisable()
    {
        audioSource.clip = hitSound;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void SwitchMode(bool dartMode)
    {
        thrustForce = dartMode ? 150 : 50;
        tiltSpeed = dartMode ? 0.5f : 1.8f;
        pitchSpeed = dartMode ? 1.5f : 3.5f;
    }
}
