using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.PlayerController
{
    public abstract class PlayerControllerButtonHandler : MonoBehaviour
    {
        protected Player player;
        protected IO.PlayerController controller;

        private void Awake()
        {
            player = GetComponentInParent<Player>();
            controller = player.GetComponent<IO.PlayerController>();
        }

        public abstract void Pressed();
        public abstract void Released();
    }
}

