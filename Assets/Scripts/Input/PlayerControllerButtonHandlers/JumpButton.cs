using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.PlayerController
{
    public class JumpButton : PlayerControllerButtonHandler
    {
        private float lateButtonGrace = 0.04f;
        private float earlyButtonGrace = 0.1f;
        private float wallDetachGrace = 0.1f;
        private float doubleJumpDelay = 0.03f;

        private bool jumpPressed;
        private float timeJumpPressed;
        private float timeNotGrounded;
        private bool checkDoubleJump;
        private float timeDetachedFromWall;

        private PlayerJumpComponent jumpComponent;
        private WallJumpAbility wallJump;

        private bool isGrounded;
        private bool stomped;
        private bool isAttachedToWall;

        private void Start()
        {
            jumpComponent = player.GetAbility<PlayerJumpComponent>();
            wallJump = player.GetAbility<WallJumpAbility>();
        }

        private void Update()
        {
            player.GetPlayerState().Set(StateLabels.CanAttachToWall, Time.time > timeJumpPressed + 0.03f);
            
            if (player.CheckState(StateLabels.IsTouchingCeiling))
            {
                Released();
                return;
            }

            if (!isAttachedToWall && player.CheckState(StateLabels.IsAttachedToWall))
            {
                SetAttachedToWall();
                return;
            }
            else if (isAttachedToWall && !player.CheckState(StateLabels.IsAttachedToWall))
            {
                timeDetachedFromWall = Time.time;
                isAttachedToWall = false;
            }

            if (player.CheckState(StateLabels.IsGrounded))
            {
                SetGrounded();
            }
            else
            {
                SetNotGrounded();
            }

            if (checkDoubleJump)
            {
                CheckDoubleJump();
            }
        }

        public override void Pressed()
        {
            jumpPressed = true;
            timeJumpPressed = Time.time;
            checkDoubleJump = false;
            
            if (isGrounded)
            {
                jumpComponent.Jump();
            }
            else if (isAttachedToWall)
            {
                jumpComponent.Jump();
                wallJump.JumpFromWall();
            }
            else
            {
                if (controller.GetMoveDirection().y < -0.5f && player.HasTrait(PlayerTraits.Traits.CanStomp))
                {
                    stomped = true;
                    jumpComponent.Stomp();
                }
                else if (Time.time < timeNotGrounded + lateButtonGrace)
                {
                    jumpComponent.Jump();
                }
                else if (Time.time < timeDetachedFromWall + wallDetachGrace)
                {
                    jumpComponent.Jump();
                    wallJump.JumpFromWall();
                }
                else
                {
                    checkDoubleJump = true;
                }
            }
        }

        public override void Released()
        {
            jumpPressed = false;
            jumpComponent.JumpReleased();
            timeJumpPressed = Time.time + earlyButtonGrace + 1f;
        }

        private void SetGrounded()
        {
            checkDoubleJump = false;
            if (isGrounded) return;
            
            isGrounded = true;
            jumpComponent.Grounded();

            if (stomped)
            {
                stomped = false;
            }
            else if (jumpPressed && Time.time < timeJumpPressed + earlyButtonGrace)
            {
                jumpComponent.Jump();
            }
        }

        private void SetNotGrounded()
        {
            if (!isGrounded) return;
            isGrounded = false;
            timeNotGrounded = Time.time;
        }
        
        private void CheckDoubleJump()
        {
            if (Time.time > timeJumpPressed + doubleJumpDelay)
            {
                checkDoubleJump = false;
                jumpComponent.DoubleJump();
            }
        }

        private void SetAttachedToWall()
        {
            jumpComponent.Stop();
            isAttachedToWall = true;
            isGrounded = true;

            if (jumpPressed && Time.time < timeJumpPressed + earlyButtonGrace)
            {
                jumpComponent.Jump();
                wallJump.JumpFromWall();
            }
        }
    }
}
