using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using EZCameraShake;
using UnityEngine.Rendering.PostProcessing;

public class MovementScript : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    public float lerpSpeed = 10.0f;

    Vector3 _currentMoveVector;
    Quaternion _currentLookRotation;
    bool _isLooking = false;

    public float dashCooldown = 2.0f;
    private float timeSinceDash;
    private bool dash = false;

    private float dashSpeed;
    private float currentSpeed;

    public GameObject model;
    private DashTrail dashTrail;

    public Camera mainCamera;
    private PostProcessVolume volume;
    private LensDistortion lensLayer;

    private float currentDistortion = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        _currentMoveVector = new Vector3(0.0f, 0.0f, 0.0f);
        _currentLookRotation = transform.rotation;

        timeSinceDash = Time.time;
        dashSpeed = speed * 2;
        currentSpeed = speed;

        dashTrail = model.GetComponent<DashTrail>();
    }

    private void Awake()
    {
        volume = mainCamera.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out lensLayer);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _currentMoveVector * currentSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, _currentLookRotation, 0.99f * Time.deltaTime * lerpSpeed);

        currentDistortion = Mathf.Lerp(currentDistortion, 15.0f, 0.99f * Time.deltaTime * 5.0f);
        SetLensDistortion(currentDistortion);

        if (Time.time - timeSinceDash >= 0.15f)
        {
            currentSpeed = speed;
            dash = false;
            dashTrail.StopTrail();
        }
    }

    void RumbleController()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0.25f, 0.75f);
            StartCoroutine(StopRumble());
        }
    }

    private IEnumerator StopRumble()
    {
        yield return new WaitForSeconds(0.05f);
        Gamepad.current.PauseHaptics();
    }

    private void ShakeCamera()
    {
        CameraShaker.Instance.ShakeOnce(4f, 2f, 0.1f, 1.0f);
    }

    void performDash()
    {
        currentDistortion = 40.0f;

        dash = true;
        currentSpeed *= 4;
        dashTrail.StartTrail();
    }

    void SetLensDistortion(float distortion)
    {
        lensLayer.intensity.value = distortion;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.action.triggered && Time.time - timeSinceDash >= dashCooldown)
        {
            timeSinceDash = Time.time;
            performDash();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.action.ReadValue<Vector2>();

        Vector3 movement = new Vector3(input.x, input.y, 0f);
        _currentMoveVector = movement;

        if (!_isLooking && input.x != 0 && input.y != 0) _currentLookRotation = GetLookQuaternion(input);
    }

    public void Look(InputAction.CallbackContext context)
    {
        Vector2 input = context.action.ReadValue<Vector2>();

        if (input.x == 0 && input.y == 0)
        {
            _isLooking = false;
            return;
        }
        else
        {
            _isLooking = true;
        }

        _currentLookRotation = GetLookQuaternion(input);
        //Quaternion targetRotation = Quaternion.LookRotation(new Vector3(input.x, 0.0f, input.y), Vector3.back);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
    }

    Quaternion GetLookQuaternion(Vector2 input)
    {
        //var dir = new Vector3(input.x, input.y, 0f) - transform.position;
        Debug.DrawRay(transform.position, input);
        var angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);

        //float heading = Mathf.Atan2(input.x, input.y);
        //return Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
    }
}
