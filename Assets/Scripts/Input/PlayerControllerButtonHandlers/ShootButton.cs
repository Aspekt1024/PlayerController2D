using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aspekt.TestObjects;

namespace Aspekt.PlayerController
{
    public class ShootButton : PlayerControllerButtonHandler
    {
        private bool buttonPressed;

        private CrosshairEffect crosshair;
        private IO.PlayerController playerController;

        public override void Pressed()
        {
            buttonPressed = true;
            crosshair.Play();
            crosshair.SetPosition(Player.Instance.transform.position);
            Time.timeScale = 0.2f;
        }

        public override void Released()
        {
            buttonPressed = false;
            crosshair.Stop();
            Time.timeScale = 1f;
            Player.Instance.transform.position = crosshair.transform.position;
        }

        private void Start()
        {
            crosshair = Player.Instance.GetEffect<CrosshairEffect>();
            playerController = IO.PlayerController.Get();
        }

        private void Update()
        {
            if (!buttonPressed) return;

            crosshair.Move(playerController.GetMoveDirection());
        }
    }
}
