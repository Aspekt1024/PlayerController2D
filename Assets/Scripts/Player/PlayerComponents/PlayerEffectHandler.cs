using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.PlayerController
{
    public class PlayerEffectHandler : MonoBehaviour
    {
        private PlayerEffect[] effects;

        public PlayerEffectHandler(Transform effectsTf)
        {
            effects = effectsTf.GetComponentsInChildren<PlayerEffect>();
        }

        public T GetEffect<T>() where T : PlayerEffect
        {
            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i].GetType().Equals(typeof(T)))
                {
                    return (T)effects[i];
                }
            }
            return null;
        }
    }
}


