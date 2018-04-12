using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.PlayerController
{
    public class ShootButton : PlayerControllerButtonHandler
    {
        public override void Pressed()
        {
            Time.timeScale = 0.4f;
        }

        public override void Released()
        {
            Time.timeScale = 1f;
        }
    }
}
