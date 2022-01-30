using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class Exit : MonoBehaviour
    {
        [SerializeField] private LayerMask _collisionMask;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_collisionMask.Contains(col.gameObject.layer))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
