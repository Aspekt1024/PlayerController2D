using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aspekt.PlayerController;

namespace Aspekt.IO
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject ButtonHandlerObject;

        private VirtualController controller;

        private Player player;
        private MoveComponent move;

        private JumpButton jumpButton;

        private bool gamePaused;

        private List<PlayerControllerButtonHandler> buttons = new List<PlayerControllerButtonHandler>();

        private enum States
        {
            Idle, 
            Jumping,
            Moving,
            Falling,
            Sliding
        }
        
        private void Start()
        {
            GetButtonHandlers();

            controller = new VirtualController(this);
            player = GetComponent<Player>();
            jumpButton = (JumpButton)buttons.Find(x => x.GetType().Equals(typeof(JumpButton)));
            move = player.GetAbility<MoveComponent>();
        }
        
        private void Update()
        {
            controller.CheckForInput();
        }

        public Vector2 GetMoveDirection()
        {
            return controller.GetMoveDirection();
        }

        public void OnInputReceived(InputLabels input)
        {
            if (InMenus())
            {
                ActionInputsInMenu(input);
            }
            else if (InGame())
            {
                if (player.IsIncapacitated)
                {
                    ActionInputsInGameIncapacitated(input);
                }
                else
                {
                    ActionInputsInGame(input);
                }
            }

            ActionInputsAlways(input);

        }

        private void ActionInputsInMenu(InputLabels input)
        {
        }

        private void ActionInputsInGame(InputLabels input)
        {
            switch (input)
            {
                case InputLabels.MoveLeftPressed:
                    move.MoveLeft();
                    break;
                case InputLabels.MoveRightPressed:
                    move.MoveRight();
                    break;
                case InputLabels.JumpPressed:
                    jumpButton.Pressed();
                    break;
                case InputLabels.StompPressed:
                    jumpButton.StompPressed();
                    break;
                case InputLabels.MeleePressed:
                    break;
                case InputLabels.ShieldPressed:
                    break;
                case InputLabels.ShootPressed:
                    break;
                case InputLabels.CycleShieldColourPressed:
                    break;
                default:
                    break;
            }
        }

        private void ActionInputsInGameIncapacitated(InputLabels input)
        {

        }

        private void ActionInputsAlways(InputLabels input)
        {
            switch (input)
            {
                case InputLabels.ToggleMenu:
                    TogglePaused();
                    break;
                case InputLabels.JumpReleased:
                    jumpButton.Released();
                    break;
                case InputLabels.MoveReleased:
                    move.MoveReleased();
                    break;
                case InputLabels.ShootReleased:
                    break;
                case InputLabels.ShieldReleased:
                    break;
                case InputLabels.MeleeReleased:
                    break;
                default:
                    break;
            }
        }

        private bool InMenus()
        {
            return gamePaused;
        }

        private bool InGame()
        {
            return !gamePaused;
        }

        private void TogglePaused()
        {
            gamePaused = !gamePaused;
            if (gamePaused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        private void GetButtonHandlers()
        {
            PlayerControllerButtonHandler[] buttonHandlers = ButtonHandlerObject.GetComponents<PlayerControllerButtonHandler>();
            for (int i = 0; i < buttonHandlers.Length; i++)
            {
                buttons.Add(buttonHandlers[i]);
            }
        }
    }
}
