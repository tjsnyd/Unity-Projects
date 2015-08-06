using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public GameObject bullet;
	public float speed;
	public AreaEffector2D area;
	// Use this for initialization
	void Start () {
		area = bullet.GetComponent<AreaEffector2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Q)){//when the left mouse button is clicked
			
			print ("1");//print a message to act as a debug
			
			FireBullet();//look for and use the fire bullet operation
			
		}
	}
	
	
	
	
	public void FireBullet(){
		
		
		
		//Clone of the bullet
		GameObject Clone;
		
		//spawning the bullet at position
		Clone = (Instantiate (bullet, transform.position, transform.rotation)) as GameObject;
		
		Rigidbody2D body = Clone.GetComponent<Rigidbody2D> ();
		//add force to the spawned objected
		body.AddForce (new Vector2(speed, 0));
	
		Destroy (Clone.gameObject, 2);
	}

	public void FireBullet(Vector3 m1, Vector3 m2){
		
		//Clone of the bullet
		GameObject Clone;
		Vector3 spot = Camera.main.ScreenToWorldPoint (m1);
		spot.z = 1;
		Vector3 spot2 = Camera.main.ScreenToWorldPoint (m2);
		spot2.x = 1;
		//spawning the bullet at position
		Clone = (Instantiate (bullet, spot, transform.rotation)) as GameObject;
		Vector3 mag = new Vector2 ((m2.x - m1.x)*5, (m2.y - m1.y) * 5);
		Rigidbody2D body = Clone.GetComponent<Rigidbody2D> ();
		//add force to the spawned objected
		body.transform.Translate(spot2, body.transform);
		
		Destroy (Clone.gameObject, 1f);
	}
}
