using System;
using UnityEngine;

namespace Misc
{
    public class AscendZone : MonoBehaviour
    {
        [SerializeField] private LayerMask _collisionMask;
        
        public static Action OnPlayerEntered;
        public static Action OnPlayerExited;

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (_collisionMask.Contains(col.gameObject.layer))
            {
                OnPlayerEntered?.Invoke();
            }
        }

        private void OnCollisionStay2D(Collision2D col)
        {
            if (_collisionMask.Contains(col.gameObject.layer))
            {
                OnPlayerEntered?.Invoke();
            }
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            if (_collisionMask.Contains(col.gameObject.layer))
            {
                OnPlayerExited?.Invoke();
            }
        }
    }
}
