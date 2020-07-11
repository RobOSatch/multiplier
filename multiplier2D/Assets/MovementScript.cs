using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // Update is called once per frame
    void Update()
    {
        transform.position += _currentMoveVector * currentSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, _currentLookRotation, 0.99f * Time.deltaTime * lerpSpeed);

        if (Time.time - timeSinceDash >= 0.15f)
        {
            currentSpeed = speed;
            dash = false;
            dashTrail.StopTrail();
        }
    }

    void performDash()
    {
        dash = true;
        currentSpeed *= 4;
        dashTrail.StartTrail();
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
