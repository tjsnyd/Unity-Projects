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
		

	}
	
	
	public void FireBullet(){
		
		
		//Clone of the bullet
		GameObject Clone;
		
		//spawning the bullet at position
		Clone = (Instantiate (bullet, Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.rotation)) as GameObject;

		//add force to the spawned objected

		
		Destroy (Clone.gameObject, .5f);
	}
	
	public void FireBullet(float m){
		

		//Clone of the bullet
		GameObject Clone;
		
		//spawning the bullet at position
		Clone = (Instantiate (bullet, transform.position, transform.rotation)) as GameObject;
		
		Rigidbody2D body = Clone.GetComponent<Rigidbody2D> ();
		//add force to the spawned objected
		body.velocity = new Vector2(m * speed /20,0);
	
		Destroy (Clone.gameObject, .5f);
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
		Vector3 mag = new Vector2 ((m2.x - m1.x), (m2.y - m1.y));
		Rigidbody2D body = Clone.GetComponent<Rigidbody2D> ();
		//add force to the spawned objected
		AreaEffector2D ae = Clone.GetComponent<AreaEffector2D>();
		ae.forceMagnitude = mag.magnitude;
		body.velocity = mag.normalized * 10;
		
		Destroy (Clone.gameObject, .5f);
	}
}
