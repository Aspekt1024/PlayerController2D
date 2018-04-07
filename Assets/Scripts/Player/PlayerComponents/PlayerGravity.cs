﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.PlayerController
{
    public class PlayerGravity : MonoBehaviour
    {
        public float FallVelocity = 15f;
        public float MaxFallVelocity = 25f;

        private float targetVerticalVelocity;

        private Player player;
        private Rigidbody2D body;
        private Animator anim;

        private PlayerBoundsCheck boundsCheck;

        private float groundDetectedTimer;
        private float groundSetDelay = 0.1f;

        private enum States
        {
            Grounded, Jumping, Falling
        }
        private States state;

        private void Start()
        {
            player = GetComponent<Player>();
            anim = GetComponent<Animator>();
            body = GetComponent<Rigidbody2D>();
            boundsCheck = new PlayerBoundsCheck(player, body);
        }

        public void SetTargetVelocity(float velocity)
        {
            targetVerticalVelocity = velocity;
        }

        public void ApplyNormalGravity()
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 3f - FallVelocity / 3f);
            targetVerticalVelocity = -FallVelocity;
        }

        public void DetachedFromWall()
        {
            targetVerticalVelocity = -MaxFallVelocity;
        }

        private void FixedUpdate()
        {
            if (player.CheckState(StateLabels.IsAttachedToWall))
            {
                body.velocity = Vector2.zero;
                targetVerticalVelocity = 0;
                return;
            }
            CheckState();
            body.velocity = new Vector2(body.velocity.x, Mathf.Lerp(body.velocity.y, targetVerticalVelocity, 2f * Time.deltaTime));
        }

        private void LateUpdate()
        {
            boundsCheck.CheckBounds(Time.deltaTime);
        }

        private void CheckState()
        {
            if (player.CheckState(StateLabels.IsGrounded))
            {
                if (state == States.Falling)
                {
                    SetGrounded();
                }
                else if (state != States.Grounded)
                {
                    groundDetectedTimer += Time.deltaTime;
                    if (groundDetectedTimer >= groundSetDelay)
                    {
                        SetGrounded();
                    }
                }
            }
            else
            {
                if (player.CheckState(StateLabels.IsJumping))
                {
                    SetJumping();
                }
                else if (state == States.Jumping)
                {
                    targetVerticalVelocity = -FallVelocity;
                    if (body.velocity.y < 0)
                    {
                        SetFalling();
                    }
                }
            }
        }

        private void SetGrounded()
        {
            if (state == States.Grounded) return;
            state = States.Grounded;
            groundDetectedTimer = 0f;
            targetVerticalVelocity = -MaxFallVelocity;
            anim.SetBool("falling", false);
            anim.SetBool("grounded", true);
        }

        private void SetFalling()
        {
            if (state == States.Falling) return;
            state = States.Falling;
            groundDetectedTimer = 0f;
            targetVerticalVelocity = -MaxFallVelocity;
            anim.SetBool("falling", true);
            anim.SetBool("grounded", false);
        }

        private void SetJumping()
        {
            if (state == States.Jumping) return;
            state = States.Jumping;
            groundDetectedTimer = 0f;
            anim.SetBool("falling", false);
            anim.SetBool("grounded", false);
        }
    }
}

