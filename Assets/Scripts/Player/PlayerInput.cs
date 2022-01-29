using Misc;
using UnityEngine;

namespace Player
{
	[RequireComponent (typeof (PlayerController))]
	public class PlayerInput : MonoBehaviour {

		PlayerController player;

		void Start () {
			player = GetComponent<PlayerController> ();
		}

		void Update () {
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

			if (Input.GetKeyDown(KeyCode.C))
			{
				SerializationManager.instance.CreateSnapshot();
			}

			if (Input.GetKeyDown(KeyCode.Z))
			{
				SerializationManager.instance.Rollback();
			}
			
			if (Input.GetKeyDown(KeyCode.X))
			{
				SerializationManager.instance.PopSnapshot();
			}
		}
	}
}
