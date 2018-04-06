﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aspekt.PlayerController;

namespace Aspekt.Items
{
    public class ObtainableItem : MonoBehaviour
    {
        public PlayerTraits.Traits TraitUnlock;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                Player.Instance.AddTrait(TraitUnlock);
                Destroy(gameObject);
            }
        }
    }
}
