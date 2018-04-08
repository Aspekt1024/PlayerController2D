using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.IO
{
    public class VirtualController
    {
        private enum InputMode
        {
            Keyboard, Controller
        }
        private InputMode mode;

        private PlayerController playerController;

        private KeyboardInputHandler keyboardInput;
        private ControllerInputHandler controllerInput;

        public VirtualController(PlayerController pc)
        {
            playerController = pc;

            keyboardInput = new KeyboardInputHandler(this);
            controllerInput = new ControllerInputHandler(this);
        }

        public void CheckForInput()
        {
            bool receivedControllerInput = controllerInput.ProcessInput();
            bool receivedKeyboardMouseInput = keyboardInput.ProcessInput();

            if (receivedControllerInput)
            {
                mode = InputMode.Controller;
            }
            else if (receivedKeyboardMouseInput)
            {
                mode = InputMode.Keyboard;
            }
        }

        public void InputReceived(InputLabels input)
        {
            playerController.OnInputReceived(input);
        }

        public Vector2 GetAimDirection(Vector2 relativeToPoint)
        {
            switch (mode)
            {
                case InputMode.Keyboard:
                    return keyboardInput.GetAimDirection(relativeToPoint);
                case InputMode.Controller:
                    return controllerInput.GetAimDirection();
                default:
                    return Vector2.right;
            }
        }

        public Vector2 GetMoveDirection()
        {
            switch (mode)
            {
                case InputMode.Keyboard:
                    return keyboardInput.GetMoveDirection();
                case InputMode.Controller:
                    return controllerInput.GetMoveDirection();
                default:
                    return Vector2.right;
            }
        }

    }
}

