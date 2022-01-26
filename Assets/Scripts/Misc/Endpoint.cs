using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class Endpoint : MonoBehaviour
    {
        [SerializeField] private LayerMask _collisionMask;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_collisionMask.Contains(col.gameObject.layer))
            {
                SceneManager.LoadScene(
                    SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
