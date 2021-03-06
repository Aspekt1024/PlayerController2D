﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.PlayerController
{
    public class MoveComponent : PlayerAbility
    {
        public float MoveSpeed = 10f;
        public float Acceleration = 8f;

        private Player player;
        private Rigidbody2D body;
        private Animator playerAnim;

        private float targetSpeed;
        private float timeSinceSpeedChange;
        private const float timeToChange = 0.3f;
        
        private float forceMoveTimer;
        private bool propelFromWall;

        private void Start()
        {
            player = GetComponentInParent<Player>();
            body = player.GetComponent<Rigidbody2D>();
            playerAnim = player.GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (player.IsIncapacitated)
            {
                MoveReleased();
            }

            playerAnim.SetFloat("MoveSpeed", Mathf.Abs(body.velocity.x));

            if (forceMoveTimer > 0)
            {
                forceMoveTimer -= Time.fixedDeltaTime;
                return;
            }
            else if (player.CheckState(StateLabels.IsKnockedBack))
            {
                return;
            }
            else if (propelFromWall)
            {
                if (player.CheckState(StateLabels.IsGrounded))
                {
                    propelFromWall = false;
                }
                else
                {
                    body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, 0, Time.fixedDeltaTime * Acceleration), body.velocity.y);
                    return;
                }
            }
            
            timeSinceSpeedChange += Time.fixedDeltaTime * Acceleration;
            if (player.CheckState(StateLabels.IsAgainstWall))
            {
                body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, 0, Time.fixedDeltaTime * Acceleration), body.velocity.y);
            }
            else
            {
                body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, targetSpeed, timeSinceSpeedChange / timeToChange), body.velocity.y);
            }

            float slopeGradient = player.GetPlayerState().GetFloat(StateLabels.SlopeGradient);
            if (player.CheckState(StateLabels.IsGrounded) && targetSpeed == 0f)
            {
                if (Mathf.Abs(slopeGradient) < 0.5f && Mathf.Abs(slopeGradient) > 0.05f)
                {
                    body.velocity = new Vector2(body.velocity.x - slopeGradient, body.velocity.y + Mathf.Abs(slopeGradient));
                }
            }

            if (player.CheckState(StateLabels.IsOnSlope) && targetSpeed > 0f)
            {
                if (Mathf.Abs(slopeGradient) > 0.5f)
                {
                    body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, 0, Time.fixedDeltaTime * Acceleration), Mathf.Lerp(body.velocity.y, -15, Time.fixedDeltaTime * Acceleration));
                }
            }
        }

        public void PropelJump(float direction)
        {
            propelFromWall = true;
            forceMoveTimer = 0.1f;
            body.velocity = new Vector2(direction * MoveSpeed, body.velocity.y);
        }

        public void MoveRight()
        {
            SetMoving();
            targetSpeed = MoveSpeed;
            player.FaceDirection(1);
        }

        public void MoveLeft()
        {
            SetMoving();
            targetSpeed = -MoveSpeed;
            player.FaceDirection(-1);
        }

        public void MoveReleased()
        {
            playerAnim.SetBool("isRunning", false);
            timeSinceSpeedChange = 0f;
            targetSpeed = 0f;
        }

        private void SetMoving()
        {
            if (forceMoveTimer <= 0 && propelFromWall)
            {
                propelFromWall = false;
            }
            playerAnim.SetBool("isRunning", true);
            timeSinceSpeedChange = 0f;
        }
    }
}