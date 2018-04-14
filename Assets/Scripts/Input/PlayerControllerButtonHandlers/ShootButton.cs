using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.PlayerController
{
    public class ShootButton : PlayerControllerButtonHandler
    {
        private bool buttonPressed;
        private BlinkAbility blink;

        public override void Pressed()
        {
            buttonPressed = true;
            blink.Activate();
        }

        public override void Released()
        {
            buttonPressed = false;
            blink.Engage();
        }

        private void Start()
        {
            blink = player.GetAbility<BlinkAbility>();
            blink.Init(player, controller);
        }
    }
}
