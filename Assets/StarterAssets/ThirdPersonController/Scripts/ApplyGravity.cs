using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGravity : MonoBehaviour
{
    public float _verticalVelocity;
    private float _terminalVelocity = 53.0f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;
    public void ApplyGravityToObject(ThirdPersonController thirdPersonController)
    {
        if (thirdPersonController.GroundCheck.Grounded) StopFalling();

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    private void StopFalling()
    {
        // stop our velocity dropping infinitely when grounded
        if (_verticalVelocity < 0.0f)
        {
            _verticalVelocity = -2f;
        }
    }
}
