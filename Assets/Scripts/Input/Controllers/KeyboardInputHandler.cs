using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Aspekt.IO
{
    public class KeyboardInputHandler
    {
        // Can use system.enum.parse to serialize these
        private const KeyCode MOVE_LEFT = KeyCode.A;
        private const KeyCode MOVE_RIGHT = KeyCode.D;
        
        private Dictionary<KeyCode, InputLabels> getKeyDownBindings = new Dictionary<KeyCode, InputLabels>();
        private Dictionary<KeyCode, InputLabels> getKeyUpBindings = new Dictionary<KeyCode, InputLabels>();
        private Dictionary<KeyCode, InputLabels> getKeyBindings = new Dictionary<KeyCode, InputLabels>();
        
        private VirtualController virtualController;

        private bool movedByKey;

        public KeyboardInputHandler(VirtualController vc)
        {
            virtualController = vc;
            
            getKeyDownBindings.Add(KeyCode.W, InputLabels.JumpPressed);
            getKeyDownBindings.Add(KeyCode.Space, InputLabels.JumpPressed);
            getKeyDownBindings.Add(KeyCode.S, InputLabels.StompPressed);
            getKeyDownBindings.Add(KeyCode.F, InputLabels.MeleePressed);
            getKeyDownBindings.Add(KeyCode.Mouse0, InputLabels.ShootPressed);
            getKeyDownBindings.Add(KeyCode.Mouse1, InputLabels.ShieldPressed);
            getKeyDownBindings.Add(KeyCode.R, InputLabels.CycleShieldColourPressed);
            getKeyDownBindings.Add(KeyCode.Escape, InputLabels.ToggleMenu);
            
            getKeyUpBindings.Add(KeyCode.W, InputLabels.JumpReleased);
            getKeyUpBindings.Add(KeyCode.Space, InputLabels.JumpReleased);
            getKeyUpBindings.Add(KeyCode.Mouse0, InputLabels.ShootReleased);
            getKeyUpBindings.Add(KeyCode.Mouse1, InputLabels.ShieldReleased);
        }

        public bool ProcessInput()
        {
            bool inputReceived = false;
            
            foreach (var binding in getKeyDownBindings)
            {
                if (Input.GetKeyDown(binding.Key))
                {
                    inputReceived = true;
                    virtualController.InputReceived(binding.Value);
                }
            }

            foreach (var binding in getKeyUpBindings)
            {
                if (Input.GetKeyUp(binding.Key))
                {
                    inputReceived = true;
                    virtualController.InputReceived(binding.Value);
                }
            }

            foreach (var binding in getKeyBindings)
            {
                if (Input.GetKey(binding.Key))
                {
                    inputReceived = true;
                    virtualController.InputReceived(binding.Value);
                }
            }

            if (Input.GetKeyDown(MOVE_LEFT) || Input.GetKey(MOVE_LEFT))
            {
                virtualController.InputReceived(InputLabels.MoveLeftPressed);
                inputReceived = true;
                movedByKey = true;
            }
            else if (Input.GetKeyDown(MOVE_RIGHT) || (Input.GetKey(MOVE_RIGHT) && !Input.GetKeyUp(MOVE_RIGHT)))
            {
                virtualController.InputReceived(InputLabels.MoveRightPressed);
                inputReceived = true;
                movedByKey = true;
            }
            else if (movedByKey)
            {
                virtualController.InputReceived(InputLabels.MoveReleased);
                movedByKey = false;
            }
            
            return inputReceived;
        }

        public Vector2 GetAimDirection(Vector2 relativeToPoint)
        {
            return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - relativeToPoint;
        }

        public Vector2 GetMoveDirection()
        {
            Vector2 moveDirection = new Vector2();
            if (Input.GetKey(MOVE_LEFT))
            {
                moveDirection += Vector2.left;
            }
            if (Input.GetKey(MOVE_RIGHT))
            {
                moveDirection += Vector2.right;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection += Vector2.down;
            }
            if (Input.GetKey(KeyCode.W))
            {
                moveDirection += Vector2.up;
            }
            return moveDirection;
        }
    }
}

