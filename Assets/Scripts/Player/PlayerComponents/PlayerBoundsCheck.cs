using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.PlayerController
{
    public class PlayerBoundsCheck
    {
        private Player player;
        private Rigidbody2D body;
        private float colliderExtentY;

        public PlayerBoundsCheck(Player player, Rigidbody2D body)
        {
            this.player = player;
            this.body = body;
            colliderExtentY = player.GetComponent<Collider2D>().bounds.extents.y;
        }

        public void CheckBounds(float deltaTime)
        {
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, body.velocity, body.velocity.magnitude * Time.fixedDeltaTime, 1 << LayerMask.NameToLayer("Terrain"));

            if (hit.collider != null)
            {
                player.transform.position = hit.point - Mathf.Sign(body.velocity.y) * colliderExtentY * Vector2.up;
                body.velocity = Vector2.zero;
            }
        }
    }
}
