using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

namespace Misc
{
    public class Collectible : MonoBehaviour
    {
        public CollectibleType type;
        // Start is called before the first frame update


        // Update is called once per frame
        private void OnCollisionEnter2D(Collision2D other)
        {
            switch (type)
            {
                case CollectibleType.MECHANICAL:
                    CollectibleManager.instance.IncrementMechanical();        
                    break;
                case CollectibleType.NATURE:
                    CollectibleManager.instance.IncrementNature();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            gameObject.SetActive(false);
        }
    }
}