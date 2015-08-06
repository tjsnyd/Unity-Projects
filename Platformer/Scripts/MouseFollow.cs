using UnityEngine;
using System.Collections;

public class MouseFollow : MonoBehaviour {
	private Vector3 mousePosition;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		mousePosition.z = 1;
		transform.position = mousePosition;
	}
}
