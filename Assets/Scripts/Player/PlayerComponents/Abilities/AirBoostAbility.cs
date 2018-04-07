using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.PlayerController
{
    public class AirBoostAbility : PlayerAbility
    {
        public float BoostPower = 7f;
        public float RechargeRate = 4f;
        public float MaxBoost = 10f;
        public float BoostUsageRate = 3f;

        private float boostAvailable;

        private bool isRecharging;

        private Player player;
        private PlayerGravity gravity;
        private Rigidbody2D body;

        private enum States
        {
            None, Boosting
        }
        private States state;

        public void StartBoost()
        {
            if (!player.HasTrait(PlayerTraits.Traits.CanBoost)) return;

            state = States.Boosting;
            body.velocity = new Vector2(body.velocity.x, BoostPower * 0.7f);
            gravity.SetTargetVelocity(BoostPower);
            player.SetState(StateLabels.IsBoosting, true);
            isRecharging = false;
        }

        public void StopBoost()
        {
            state = States.None;
            player.SetState(StateLabels.IsBoosting, false);
            gravity.ApplyNormalGravity();
        }

        public bool CanBoost { get { return player.HasTrait(PlayerTraits.Traits.CanBoost) && boostAvailable > 0; } }

        private void Start()
        {
            player = Player.Instance;
            gravity = player.GetComponent<PlayerGravity>();
            body = player.GetComponent<Rigidbody2D>();
            
            DebugUI.SetBoostIndicatorFill(boostAvailable / MaxBoost);
        }

        private void Update()
        {
            if (!player.HasTrait(PlayerTraits.Traits.CanBoost)) return;
            
            if (!isRecharging)
            {
                isRecharging = state == States.None && player.CheckState(StateLabels.IsGrounded);
            }

            if (isRecharging && boostAvailable < MaxBoost)
            {
                boostAvailable += RechargeRate * Time.deltaTime;
            }

            if (state == States.Boosting)
            {
                boostAvailable -= BoostUsageRate * Time.deltaTime;
                if (boostAvailable <= 0)
                {
                    StopBoost();
                }
            }

            Mathf.Clamp(boostAvailable, 0, MaxBoost);
            DebugUI.SetBoostIndicatorFill(boostAvailable / MaxBoost);
        }
    }
}

