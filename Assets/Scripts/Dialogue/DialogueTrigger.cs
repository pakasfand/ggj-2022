using System;
using Misc;
using UnityEngine;

namespace Dialogue
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private DialogueInstance _dialogueInstance;
        [SerializeField] private LayerMask _collisionMask;
        [SerializeField] private bool _triggerOnce;

        private bool _triggeredOnceBefore = false;

        public static Action<DialogueInstance> OnDialogueTriggered;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!_triggerOnce || !_triggeredOnceBefore)
            {
                if(_collisionMask.Contains(col.gameObject.layer))
                {
                    _triggeredOnceBefore = true;
                    OnDialogueTriggered?.Invoke(_dialogueInstance);
                }
            }
        }
    }
}
