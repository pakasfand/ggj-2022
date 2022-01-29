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
        private void OnTriggerEnter(Collider other)
        {
        }
    }
}