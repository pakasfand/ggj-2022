using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _interfaceNameText;
        [SerializeField] private TextMeshProUGUI _interfaceDialogueText;
        [SerializeField] Image Interfaceportrait;
        [SerializeField] private GameObject _continueButton;
        [SerializeField] Animator anim;
        [SerializeField] float TypingSpeed;
        [SerializeField] float TimeUntilTextAppears;

        private Queue<string> NameText;
        private Queue <string> sentences;
        private Queue <TMP_FontAsset> NameFont;
        private Queue <TMP_FontAsset> DialogueFont;
        private Queue <Sprite> portraits;
        private Queue<DialogueCharacterType> _characterTypes;

        public static Action OnDialogueEnded;
        public static Action<DialogueCharacterType> OnDialogueInstanceStarted;
        public static Action OnDialogueInstanceEnded;

        private void OnEnable()
        {
            DialogueTrigger.OnDialogueTriggered += OnDialogueTriggered;
        }

        private void OnDisable()
        {
            DialogueTrigger.OnDialogueTriggered -= OnDialogueTriggered;
        }

        void Start()
        {
            NameText = new Queue<string>();
            sentences = new Queue<string>();
            NameFont = new Queue<TMP_FontAsset>();
            DialogueFont = new Queue<TMP_FontAsset>();
            portraits = new Queue<Sprite>();
            _characterTypes = new Queue<DialogueCharacterType>();
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            
            ResetDialogueState();
        }

        public void StartDialogue(DialogueInstance _dialogueInstance)
        {
            // Debug.Log("Starting dialogue of: " + dialogue.name);
            anim.SetBool("isOpen", true);

            StartCoroutine(CheckAnimationCompeted(_dialogueInstance));
        }
    
        public void DisplayNextSentence()
        {
            DisableContinueButton();
            
            if(sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            
            string sentence = sentences.Dequeue();
            var characterType = _characterTypes.Dequeue();
            StopAllCoroutines();

            _interfaceNameText.text     = NameText.Dequeue();
            _interfaceNameText.font     = NameFont.Dequeue();
            _interfaceDialogueText.font = DialogueFont.Dequeue();
            Interfaceportrait.sprite    = portraits.Dequeue();
            Interfaceportrait.enabled   = true;
        
            // Debug.Log(sentence);
            StartCoroutine(TypeSentence(sentence, characterType));
        }

        private IEnumerator CheckAnimationCompeted(DialogueInstance _dialogueInstance)
        {
            yield return new WaitForSecondsRealtime(TimeUntilTextAppears);
            sentences.Clear();
            foreach (Sentence sentence in _dialogueInstance.sentences)
            {
                sentences.Enqueue(sentence.text);
                NameText.Enqueue(sentence.character.CharacterName);
                NameFont.Enqueue(sentence.character.font);
                DialogueFont.Enqueue(sentence.character.font);
                portraits.Enqueue(sentence.character.portrait);
                _characterTypes.Enqueue(sentence.character.DialogueCharacterType);
            }
            
            DisplayNextSentence();
        }

        private IEnumerator TypeSentence (string sentence, DialogueCharacterType characterType)
        {
            OnDialogueInstanceStarted?.Invoke(characterType);
            _interfaceDialogueText.text = "";
            foreach(char letter in sentence.ToCharArray())
            {
                _interfaceDialogueText.text += letter;
                yield return new WaitForSecondsRealtime(TypingSpeed);
            }
            
            EnableContinueButton();
            OnDialogueInstanceEnded?.Invoke();
        }

        private void EndDialogue()
        {
            // Debug.Log("End of Conversation");
            OnDialogueEnded?.Invoke();
            StopAllCoroutines();
            anim.SetBool("isOpen", false);
            ResetDialogueState();
        }

        private void ResetDialogueState()
        {
            DisableContinueButton();
            _interfaceNameText.text = "";
            _interfaceDialogueText.text = "";
            Interfaceportrait.enabled = false;
        }
        
        private void EnableContinueButton() =>
            _continueButton.SetActive(true);

        private void DisableContinueButton() =>
            _continueButton.SetActive(false);
        
        private void OnDialogueTriggered(DialogueInstance _dialogueInstance) => 
            StartDialogue(_dialogueInstance);
    }
}
