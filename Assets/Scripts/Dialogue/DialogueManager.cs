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
        [SerializeField] Animator anim;
        [SerializeField] float TypingSpeed;
        [SerializeField] float TimeUntilTextAppears;

        private Queue<string> NameText;
        private Queue <string> sentences;
        private Queue <TMP_FontAsset> NameFont;
        private Queue <TMP_FontAsset> DialogueFont;
        private Queue <Sprite> portraits;

        public static Action OnDialogueEnded;

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
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        public void StartDialogue(DialogueInstance _dialogueInstance)
        {
            // Time.timeScale = 0;

            anim.SetBool("isOpen", true);

            // Debug.Log("Starting dialogue of: " + dialogue.name);

            StartCoroutine(CheckAnimationCompeted(_dialogueInstance));
        }
    
        public void DisplayNextSentence()
        {
            if(sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            
            string sentence = sentences.Dequeue();
            StopAllCoroutines();

            _interfaceNameText.text = NameText.Dequeue();
            _interfaceNameText.font = NameFont.Dequeue();
            _interfaceDialogueText.font = DialogueFont.Dequeue();
            Interfaceportrait.sprite = portraits.Dequeue();
        
            StartCoroutine(TypeSentence(sentence));
            // Debug.Log(sentence);
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
            }
            DisplayNextSentence();
        }

        private IEnumerator TypeSentence (string sentence)
        {
            _interfaceDialogueText.text = "";
            foreach(char letter in sentence.ToCharArray())
            {
                _interfaceDialogueText.text += letter;
                yield return new WaitForSecondsRealtime(TypingSpeed);
            }
        }

        private void EndDialogue()
        {
            OnDialogueEnded?.Invoke();
            anim.SetBool("isOpen", false);
            _interfaceDialogueText.text = "";
            _interfaceNameText.text = "";
            // Time.timeScale = 1;
            // Debug.Log("End of Conversation");
        }
        
        private void OnDialogueTriggered(DialogueInstance _dialogueInstance) => 
            StartDialogue(_dialogueInstance);
    }
}
