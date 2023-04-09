using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerMove : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;

    public void Move(ThirdPersonController thirdPersonController)
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = thirdPersonController.GetInput().sprint ? SprintSpeed : MoveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (thirdPersonController.GetInput().move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(thirdPersonController.GetController().velocity.x, 0.0f, thirdPersonController.GetController().velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = thirdPersonController.GetInput().analogMovement ? thirdPersonController.GetInput().move.magnitude : 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(thirdPersonController.GetInput().move.x, 0.0f, thirdPersonController.GetInput().move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (thirdPersonController.GetInput().move != Vector2.zero)
        {
            _targetRotation = thirdPersonController.GetCamera().transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * inputDirection;

        // move the player
        thirdPersonController.GetController().Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, thirdPersonController.ApplyGravity._verticalVelocity, 0.0f) * Time.deltaTime);

        // update animator if using character
        if (thirdPersonController.HasAnimator())
        {
            thirdPersonController.GetAnimator().SetFloat(thirdPersonController._animIDSpeed, _animationBlend);
            thirdPersonController.GetAnimator().SetFloat(thirdPersonController._animIDMotionSpeed, inputMagnitude);
        }
    }

}
