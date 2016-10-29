using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float maxSpeed;
	private float moveX;
	private float xBeforeJump;

	// jump variables
	public GameObject groundDetecter;
	public LayerMask ground;
	public float jumpForce = 2f;
	private bool grounded;
	private float checkRadius = .1f;
	private bool jumpingUp;
	private float EPSILON = Mathf.Pow (10, -14);

	// Use this for initialization
	void Start () {
		grounded = Physics2D.OverlapCircle (groundDetecter.transform.position, checkRadius, ground) && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < EPSILON;
		jumpingUp = !grounded && (GetComponent<Rigidbody2D> ().velocity.y > 0);
	}
	
	// Update is called once per frame
	void Update () {
		Move();
	}

	void Move() {
		moveX = Input.GetAxis("Horizontal");
		if(grounded) { 
			//Debug.Log("??????");
			GetComponent<Rigidbody2D>().velocity = new Vector2 (moveX * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
			xBeforeJump = moveX;
		}
		else {
			//Debug.Log("HALP");
			GetComponent<Rigidbody2D>().velocity = new Vector2 (xBeforeJump * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);			
		}
//		GetComponent<Rigidbody2D>().velocity = new Vector2 (moveX * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

		// Jump through ground when jumping up
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("Ground"), jumpingUp);

		if (Input.GetAxis("Vertical") > 0 && grounded && !jumpingUp) {
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			Debug.Log("Jump up: " + Vector2.up * jumpForce);
			GetComponent<Rigidbody2D>().AddForce (Vector2.up * jumpForce);
			grounded = false;
			jumpingUp = true;
		}

		grounded = Physics2D.OverlapCircle (groundDetecter.transform.position, checkRadius, ground) && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < EPSILON;
		if(jumpingUp) {
			jumpingUp = GetComponent<Rigidbody2D> ().velocity.y > 0;
			Debug.Log("Jumping up: " + jumpingUp);
		}
	}
}
