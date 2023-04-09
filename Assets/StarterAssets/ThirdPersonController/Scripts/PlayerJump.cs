using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerJump : MonoBehaviour
{
    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;



    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;



    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    private void Start()
    {
        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }
    public void JumpAndGravity(ThirdPersonController thirdPersonController)
    {


        if (thirdPersonController.GroundCheck.Grounded)
        {
           
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // update animator if using character
            if (thirdPersonController.HasAnimator())
            {
                thirdPersonController.GetAnimator().SetBool(thirdPersonController._animIDJump, false);
                thirdPersonController.GetAnimator().SetBool(thirdPersonController._animIDFreeFall, false);
            }

           

            // Jump
            if (thirdPersonController.GetInput().jump && _jumpTimeoutDelta <= 0.0f)
            {
                
               thirdPersonController.ApplyGravity._verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * thirdPersonController.ApplyGravity.Gravity);

                // update animator if using character
                if (thirdPersonController.HasAnimator())
                {
                    thirdPersonController.GetAnimator().SetBool(thirdPersonController._animIDJump, true);

                }
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }

        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                // update animator if using character
                if (thirdPersonController.HasAnimator())
                {
                    thirdPersonController.GetAnimator().SetBool(thirdPersonController._animIDFreeFall, true);

                }
            }

            // if we are not grounded, do not jump
            thirdPersonController.GetInput().jump = false;

        }

    }

  
}
