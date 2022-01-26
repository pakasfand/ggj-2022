using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Misc
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        [Header("Footsteps")] 
        [SerializeField] private List<AudioClip> _footSteps;
        
        [Header("Sounds Effects")]
        [SerializeField] private AudioClip _jumpSfx;

        private void Awake()
        {
            var audioManager = FindObjectsOfType<AudioManager>();
            
            if (audioManager.Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            PlayerController.OnPlayerJump += OnPlayerJump;
            PlayerController.OnPlayerFootStep += OnPlayerFootStep;
        }

        private void OnDisable()
        {
            PlayerController.OnPlayerJump -= OnPlayerJump;
            PlayerController.OnPlayerFootStep -= OnPlayerFootStep;
        }

        private void OnPlayerFootStep()
        {
            _sfxSource.PlayOneShot(_footSteps[Random.Range(0, _footSteps.Count)]);
        }

        private void OnPlayerJump()
        {
            _sfxSource.PlayOneShot(_jumpSfx);
        }
    }
}
