using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.PlayerController
{
    public class Player : MonoBehaviour
    {
        public static Player Instance
        {
            get { return instance; }
        }

        private static Player instance;

        private PlayerTraits traits;
        private PlayerState playerState;
        private PlayerGravity gravity;

        private Transform model;
        private PlayerAbilityHandler abilityHandler;
        private PlayerEffectHandler effectHandler;
        
        private Rigidbody2D body;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                GetPlayerComponents();
            }
        }

        public T GetAbility<T>() where T : PlayerAbility
        {
            return abilityHandler.GetAbility<T>();
        }

        public T GetEffect<T>() where T : PlayerEffect
        {
            return effectHandler.GetEffect<T>();
        }

        #region Traits
        public bool HasTrait(PlayerTraits.Traits trait)
        {
            return traits.HasTrait(trait);
        }

        public void AddTrait(PlayerTraits.Traits trait)
        {
            traits.AddTrait(trait);
        }

        public void SetTrait(PlayerTraits.Traits trait, bool value)
        {
            traits.Set(trait, value);
        }
        #endregion

        public PlayerState GetPlayerState()
        {
            return playerState;
        }

        public void SetState(StateLabels label, object value)
        {
            playerState.Set(label, value);
        }

        public bool CheckState(StateLabels label)
        {
            return playerState.Check(label);
        }

        public void FaceDirection(float xDirection)
        {
            model.transform.localScale = new Vector3(
                (xDirection < 0 ? -1 : 1) * Mathf.Abs(model.transform.localScale.x),
                model.transform.localScale.y,
                model.transform.localScale.z);
        }

        public bool IsFacingRight() { return model.transform.localScale.x > 0; }

        public void Knockback(Vector2 direction, float force, float duration = 0.2f)
        {
            if (CheckState(StateLabels.IsKnockedBack)) return;

            playerState.Set(StateLabels.IsKnockedBack, true);
            StartCoroutine(KnockbackRoutine(direction.x, 7f, .1f));
        }

        private IEnumerator KnockbackRoutine(float direction, float force, float duration)
        {
            GetEffect<TestObjects.CollisionEffect>().Play();
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                body.velocity = new Vector2((direction >= 0 ? 1 : -1) * force, force);
                yield return null;
            }
            playerState.Set(StateLabels.IsKnockedBack, false);
        }

        private void GetPlayerComponents()
        {
            traits = new PlayerTraits();
            playerState = new PlayerState();

            body = GetComponent<Rigidbody2D>();
            gravity = GetComponent<PlayerGravity>();

            Transform[] childTransforms = GetComponentsInChildren<Transform>();
            for (int i = 0; i < childTransforms.Length; i++)
            {
                switch (childTransforms[i].name)
                {
                    case "Model":
                        model = childTransforms[i];
                        break;
                    case "Abilities":
                        abilityHandler = new PlayerAbilityHandler(childTransforms[i]);
                        break;
                    case "Effects":
                        effectHandler = new PlayerEffectHandler(childTransforms[i]);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
