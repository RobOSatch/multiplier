using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;

    Vector3 _currentMoveVector;
    Quaternion _currentLookRotation;
    bool _isLooking = false;

    // Start is called before the first frame update
    void Start()
    {
        _currentMoveVector = new Vector3(0.0f, 0.0f, 0.0f);
        _currentLookRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _currentMoveVector * speed * Time.deltaTime;
        transform.rotation = _currentLookRotation;
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.action.ReadValue<Vector2>();

        Vector3 movement = new Vector3(input.x, 0.0f, input.y);
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
        } else
        {
            _isLooking = true;
        }

        _currentLookRotation = GetLookQuaternion(input);
        //Quaternion targetRotation = Quaternion.LookRotation(new Vector3(input.x, 0.0f, input.y), Vector3.back);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
    }

    Quaternion GetLookQuaternion(Vector2 input)
    {
        float heading = Mathf.Atan2(input.x, input.y);
        return Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0f);
    }
}
