using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class DestroyOnImpact : MonoBehaviour
    {
        [SerializeField] private LayerMask _collisionMask;
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (_collisionMask.Contains(col.gameObject.layer))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
