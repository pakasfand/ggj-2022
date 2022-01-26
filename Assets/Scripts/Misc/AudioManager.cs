using System;
using Player;
using UnityEngine;

namespace Misc
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        [Header("Sounds Effects")] [SerializeField]
        private AudioClip _jumpSfx;
        
        private void OnEnable()
        {
            PlayerController.OnPlayerJump += OnPlayerJump;
        }

        private void OnDisable()
        {
            PlayerController.OnPlayerJump -= OnPlayerJump;
        }

        private void OnPlayerJump()
        {
            _sfxSource.PlayOneShot(_jumpSfx);
        }
    }
}
