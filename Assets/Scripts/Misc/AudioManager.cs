using System.Collections;
using System.Collections.Generic;
using Dialogue;
using Player;
using UnityEngine;

namespace Misc
{
    public class AudioManager : MonoBehaviour
    {
        [Header("AudioSources")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _dialogueSource;

        [Header("Footsteps")] 
        [SerializeField] private List<AudioClip> _footSteps;

        [Header("Voices")]
        [SerializeField] private List<AudioClip> _mechanicalVoice;
        [SerializeField] private List<AudioClip> _natureVoice;
        
        [Header("Sounds Effects")]
        [SerializeField] private AudioClip _jumpSfx;

        private int _voiceLineIndex;
        
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
            DialogueManager.OnDialogueInstanceStarted += OnDialogueInstanceStarted;
            DialogueManager.OnDialogueInstanceEnded += OnDialogueInstanceEnded;
        }

        private void OnDisable()
        {
            PlayerController.OnPlayerJump -= OnPlayerJump;
            PlayerController.OnPlayerFootStep -= OnPlayerFootStep;
            DialogueManager.OnDialogueInstanceStarted -= OnDialogueInstanceStarted;
            DialogueManager.OnDialogueInstanceEnded -= OnDialogueInstanceEnded;
        }

        private void OnPlayerFootStep()
        {
            _sfxSource.PlayOneShot(_footSteps[Random.Range(0, _footSteps.Count)]);
        }

        private void OnPlayerJump()
        {
            _sfxSource.PlayOneShot(_jumpSfx);
        }
        
        private void OnDialogueInstanceStarted(DialogueCharacterType characterType)
        {
            if (characterType == DialogueCharacterType.Mechanical)
            {
                StartCoroutine(CharacterSpeechLoop(_mechanicalVoice));
            }
            else
            {
                StartCoroutine(CharacterSpeechLoop(_natureVoice));
            }
        }

        private IEnumerator CharacterSpeechLoop(List<AudioClip> speech)
        {
            while (true)
            {
                _dialogueSource.PlayOneShot(speech[_voiceLineIndex]);
                yield return new WaitForSeconds(speech[_voiceLineIndex].length);
                
                _voiceLineIndex += 1;
                if (_voiceLineIndex >= speech.Count)
                {
                    _voiceLineIndex = 0;
                }
            }
        }

        private void OnDialogueInstanceEnded()
        {
            StopCoroutine(nameof(CharacterSpeechLoop));
            StopAllCoroutines();
            _voiceLineIndex = 0;
        }
    }
}
