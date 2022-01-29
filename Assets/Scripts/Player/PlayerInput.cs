using System;
using Dialogue;
using UnityEngine;

namespace Player
{
	[RequireComponent (typeof (PlayerController))]
	public class PlayerInput : MonoBehaviour 
	{
		PlayerController player;

		private bool _enabled = true;
		
		private void OnEnable()
		{
			DialogueTrigger.OnDialogueTriggered += OnDialogueTriggered;
			DialogueManager.OnDialogueEnded += OnDialogueEnded;
		}

		private void OnDisable()
		{
			DialogueTrigger.OnDialogueTriggered -= OnDialogueTriggered;
			DialogueManager.OnDialogueEnded -= OnDialogueEnded;
		}
		
		void Start () {
			player = GetComponent<PlayerController> ();
		}

		void Update ()
		{
			if (!_enabled) { return; }
			
			Vector2 directionalInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
			player.SetDirectionalInput (directionalInput);

			if (Input.GetKeyDown (KeyCode.Space))
			{
				player.OnJumpInputDown ();
			}
			if (Input.GetKeyUp (KeyCode.Space))
			{
				player.OnJumpInputUp ();
			}
			if (Input.GetKey(KeyCode.E))
			{
				player.OnDescendInputDown();
			}
			if (Input.GetKeyUp(KeyCode.E))
			{
				player.OnDescendInputUp();
			}
		}

		private void OnDialogueTriggered(DialogueInstance _dialogueInstance) =>
			_enabled = false;

		private void OnDialogueEnded() => _enabled = true;
	}
}
