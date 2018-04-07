using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.PlayerController
{
    public class PlayerBoundsCheck
    {
        private Player player;
        private Rigidbody2D body;
        private Collider2D coll;

        public PlayerBoundsCheck(Player player, Rigidbody2D body)
        {
            this.player = player;
            this.body = body;
            coll = player.GetComponent<Collider2D>();
        }

        public void CheckBounds(float deltaTime)
        {
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, body.velocity, body.velocity.magnitude * Time.fixedDeltaTime, 1 << LayerMask.NameToLayer("Terrain"));

            bool positionCorrected = false;
            float xVelocity = body.velocity.x;
            float yVelocity = body.velocity.y;
            float xPos = body.position.x;
            float yPos = body.position.y;

            if (hit.collider != null)
            {
                if (Mathf.Abs(hit.point.y - body.position.y) > coll.bounds.extents.y)
                {
                    yPos = hit.point.y - Mathf.Sign(yVelocity) * coll.bounds.extents.y;
                    positionCorrected = true;
                    yVelocity = 0f;
                }

                if (Mathf.Abs(hit.point.x - body.position.x) > coll.bounds.extents.x)
                {
                    xPos = hit.point.x - Mathf.Sign(xVelocity) * coll.bounds.extents.x;
                    positionCorrected = true;
                    xVelocity = 0f;
                }

                if (positionCorrected)
                {
                    body.position = new Vector2(xPos, yPos);
                    body.velocity = new Vector2(xVelocity, yVelocity);
                }
            }
        }
    }
}
