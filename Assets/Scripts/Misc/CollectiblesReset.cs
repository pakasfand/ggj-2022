using System;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class CollectiblesReset : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _reset;
        [SerializeField] private LayerMask _collisionMask;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_collisionMask.Contains(col.gameObject.layer))
            {
                foreach (var enableObject in _reset)
                {
                    enableObject.SetActive(true);
                }
            }
        }
    }
}
