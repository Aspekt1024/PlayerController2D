using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.PlayerController
{
    public class WallJumpAbility : PlayerAbility
    {
        public float WallAttachDuration = 0.5f;

        private enum States
        {
            None, OnWall
        }
        private States state;

        private float wallAttachTime;
        private float prevXPos;

        private Player player;
        private Rigidbody2D body;
        private PlayerGravity gravity;

        private MoveComponent move;

        private WallAttachEffect attachEffect;

        private float wallDirection;

        private void Start()
        {
            player = Player.Instance;
            body = player.GetComponent<Rigidbody2D>();
            gravity = player.GetComponent<PlayerGravity>();
            state = States.None;
            prevXPos = player.transform.position.x;
            attachEffect = player.GetEffect<WallAttachEffect>();

            move = player.GetAbility<MoveComponent>();
        }
        
        private void FixedUpdate()
        {
            if (!player.HasTrait(PlayerTraits.Traits.CanWallJump) || player.CheckState(StateLabels.IsInGravityField)) return;

            if (state == States.OnWall)
            {
                if (Time.time > wallAttachTime + WallAttachDuration || !player.CheckState(StateLabels.IsAgainstWall))
                {
                    player.GetPlayerState().Set(StateLabels.IsAttachedToWall, false);
                    state = States.None;

                    if (!player.CheckState(StateLabels.IsJumping))
                    {
                        gravity.ApplyNormalGravity();
                    }
                }
            }

            if (state == States.None && !player.CheckState(StateLabels.IsGrounded))
            {
                if (player.CheckState(StateLabels.IsAgainstWall) && !player.CheckState(StateLabels.IsStomping) && Mathf.Abs(prevXPos - player.transform.position.x) > 0.1f)
                {
                    state = States.OnWall;
                    wallDirection = player.IsFacingRight() ? 1 : -1;
                    player.GetPlayerState().Set(StateLabels.IsAttachedToWall, true);
                    wallAttachTime = Time.time;

                    RaycastHit2D hit = Physics2D.Raycast(player.transform.position, Vector2.right * wallDirection, 3f, 1 << LayerMask.NameToLayer("Terrain"));
                    if (hit.collider != null)
                    {
                        attachEffect.transform.position = hit.point;
                    }
                    attachEffect.Play();
                }
            }

            prevXPos = player.transform.position.x;
        }
        
        public void JumpFromWall()
        {
            player.SetState(StateLabels.IsAttachedToWall, false);
            move.PropelJump(-wallDirection);
        }
    }
}

