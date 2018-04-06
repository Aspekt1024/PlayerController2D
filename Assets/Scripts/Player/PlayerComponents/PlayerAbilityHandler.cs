using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.PlayerController
{
    public class PlayerAbilityHandler
    {
        private PlayerAbility[] abilities;

        public PlayerAbilityHandler(Transform abilitiesTf)
        {
            abilities = abilitiesTf.GetComponentsInChildren<PlayerAbility>();
        }

        public T GetAbility<T>() where T : PlayerAbility
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                if (abilities[i].GetType().Equals(typeof(T)))
                {
                    return (T)abilities[i];
                }
            }
            return null;
        }
    }
}
