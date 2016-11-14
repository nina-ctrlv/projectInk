using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float maxSpeed;
	private float moveX;
	private float xBeforeJump;
	private bool facingRight;

	// jump variables
	public GameObject groundDetecter;
	public LayerMask ground;
	public float jumpForce = 2f;
	private bool grounded;
	private float checkRadius = .1f;
	private bool jumpingUp;
	private float EPSILON = Mathf.Pow (10, -14);

	// mark attack
	public KeyCode markInput;
	public Rigidbody2D inkMarker;
	public float bulletSpeed;
	public float maxMarkCooldown;
	public float markCooldown;


	// Use this for initialization
	void Start () {
		grounded = Physics2D.OverlapCircle (groundDetecter.transform.position, checkRadius, ground) && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < EPSILON;
		jumpingUp = !grounded && (GetComponent<Rigidbody2D> ().velocity.y > 0);
		facingRight = true;
	}
	
	// Update is called once per frame
	void Update () {
		Attack();
		Move();
	}

	void Attack() {
		// see if attack pressed
		if(Input.GetKeyDown(markInput) && markCooldown <= 0) {
			markCooldown = maxMarkCooldown;
			ShootMark();
		}
		if(markCooldown > 0) {
			markCooldown -= Time.deltaTime;
		}
	}

	void ShootMark() {
		// shoot projectile
		Vector2 markerClonePos = transform.position;
		float dist = .8f;
		if(!facingRight) {
			dist *= -1;
		}
		markerClonePos.x += dist;
		markerClonePos.y += .3f;

		Rigidbody2D markerClone = Instantiate(inkMarker, markerClonePos, transform.rotation) as Rigidbody2D;

		if(!facingRight) {
			markerClone.AddForce (Vector2.left * bulletSpeed);
			markerClone.gameObject.GetComponent<SpriteRenderer>().flipX = true;
		}
		else {
			markerClone.AddForce (Vector2.right * bulletSpeed);
			markerClone.gameObject.GetComponent<SpriteRenderer>().flipX = false;
		}


	}

	void Move() {
		moveX = Input.GetAxis("Horizontal");
		// moves left/right but can't move when jumping
		if(grounded) { 
			GetComponent<Rigidbody2D>().velocity = new Vector2 (moveX * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
			xBeforeJump = moveX;
		}
		else {
			GetComponent<Rigidbody2D>().velocity = new Vector2 (xBeforeJump * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);			
		}

		// flips sprite to face correct way
		if(moveX < 0) {
			facingRight = false;
			GetComponent<SpriteRenderer>().flipX = true;
		}
		else if(moveX > 0) {
			facingRight = true;
			GetComponent<SpriteRenderer>().flipX = false;
		}

		// Jump through ground when jumping up
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("Ground"), jumpingUp);

		if (Input.GetAxis("Vertical") > 0 && grounded && !jumpingUp) {
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			//Debug.Log("Jump up: " + Vector2.up * jumpForce);
			GetComponent<Rigidbody2D>().AddForce (Vector2.up * jumpForce);
			grounded = false;
			jumpingUp = true;
		}

		// check if grounded
		grounded = Physics2D.OverlapCircle (groundDetecter.transform.position, checkRadius, ground) && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < EPSILON;
		if(jumpingUp) {
			jumpingUp = GetComponent<Rigidbody2D> ().velocity.y > 0;
			//Debug.Log("Jumping up: " + jumpingUp);
		}
	}
}
