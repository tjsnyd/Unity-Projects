using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {

	private PlayerPhysics playerPhysics;
	private Fire fire;
	private bool jump;
	public Vector3[] toPush;
	private Vector3 mouse1, mouse2;
	private bool down = false;
	public GameObject bullet;
	public float speed;
	private bool shot;
	private bool fireDown, fireUp, force;

	// Use this for initialization
	void Awake () {
		playerPhysics = GetComponent<PlayerPhysics>();
		fire = GetComponent<Fire>();

	}
	
	// Update is called once per frame
	private void Update()
	{

		if (!jump)
		{
			// Read the jump input in Update so button presses aren't missed.
			jump = Input.GetButtonDown("Jump");
		}
		fireDown = Input.GetButtonDown ("Fire1");
		fireUp = Input.GetButtonUp("Fire1");
		if (Input.GetButtonDown ("Force")) {
			
			if (Time.timeScale == 1.0F) {
				Time.timeScale = 0F;
				force = true;

			} else {
				Time.timeScale = 1.0F;
				force = false;
				if(shot){
					Vector2 dir = new Vector2(mouse2.x - mouse1.x, mouse2.y - mouse1.y);
					int pos = isPos (dir);
					Vector2 vecDir;
					if(pos == 0){
						vecDir = Vector2.up;
					}
					else if(pos == 1){
						vecDir = Vector2.left;
					}
					else if(pos == 2){
						vecDir = Vector2.down;
					}
					else{
						vecDir = Vector2.right;
					}

					fire.area.forceAngle = Vector2.Angle (dir, vecDir) + 90 * pos;
					print ("dir:" + dir);
					fire.FireBullet(mouse1,mouse2);
					shot = false;
				}
			}
		}
		if (fireDown && !down && force) {
			mouse1 = Input.mousePosition;
			down = true;
			print (mouse1);

		}
		if (fireUp && down && force) {
			mouse2 = Input.mousePosition;	
			shot = true;
			print (mouse2);
			down = false;
		}

				
			//Time.fixedDeltaTime = 0F * Time.timeScale;
		


		
	}
	//returns quadrant
	private int isPos(Vector2 v){
		if (v.x > 0 && v.y > 0)
			return 3;
		else if (v.x < 0 && v.y > 0)
			return 0;
		else if (v.x < 0 && v.y < 0)
			return 1;
		else
			return 2;

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

