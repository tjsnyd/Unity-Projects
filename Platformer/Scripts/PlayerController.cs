using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {

	private PlayerPhysics playerPhysics;
	private bool jump;
	// Use this for initialization
	void Awake () {
		playerPhysics = GetComponent<PlayerPhysics>();
	}
	
	// Update is called once per frame
	private void Update()
	{
		if (!jump)
		{
			// Read the jump input in Update so button presses aren't missed.
			jump = Input.GetKeyDown(KeyCode.UpArrow);
		}
	}

	private void FixedUpdate()
	{

		// Read the inputs.
		bool crouch = Input.GetKey(KeyCode.LeftControl);
		float h = Input.GetAxis("Horizontal");
		// Pass all parameters to the character control script.
		playerPhysics.Move(h,crouch,jump);
		jump = false;
	}
	
	
}
