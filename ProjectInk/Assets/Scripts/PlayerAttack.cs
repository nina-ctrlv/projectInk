using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour {

	public List<GameObject> marks;

	// mark attack
	public KeyCode markInput;
	public Rigidbody2D inkMarker;
	public float bulletSpeed;
	public float maxMarkCooldown;
	public float markCooldown;

	// spike attack
	public KeyCode spikeInput;
	public float maxSpikeCooldown;
	public int spikeDmg;
	private float spikeCooldown;

	// explosion attack
	public KeyCode explosionInput;
	public float maxExplosionCooldown;
	public int explDmg;
	private float explosionCooldown;

	private PlayerController pc;

	// Use this for initialization
	void Start () {
		pc = GetComponent<PlayerController>();
		marks = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		Attack();
	}

	void Attack() {
		// see if attack pressed
		if(Input.GetKeyDown(markInput) && markCooldown <= 0) {
			markCooldown = maxMarkCooldown;
			ShootMark();
		}
		else if(Input.GetKeyDown(spikeInput) && spikeCooldown <= 0) {
			spikeCooldown = maxSpikeCooldown;
			Debug.Log("Spike");
			TriggerAtk("Ground", 0, spikeDmg);
		}
		else if(Input.GetKeyDown(explosionInput) && explosionCooldown <= 0) {
			explosionCooldown = maxExplosionCooldown;
			Debug.Log("Explosion");
			TriggerAtk("Enemy", 1, explDmg);
		}
		if(markCooldown > 0) {
			markCooldown -= Time.deltaTime;
		}
		if(spikeCooldown > 0) {
			spikeCooldown -= Time.deltaTime;
		}
	}

	// t = tag
	// a = animation sequence number
	// d = damage
	void TriggerAtk(string t, int a, int d) {
		for(int i = 0; i < marks.Count; i++) {
			if(marks[i] != null) {
				if(marks[i].transform.parent.tag == t) {
					marks[i].GetComponent<SplatterAttr>().Atk(a, d);
				}
			}
			else {
				marks.RemoveAt(i);
				i--;
			}
		}
	}

	void ShootMark() {
		// shoot projectile
		Vector2 markerClonePos = transform.position;
		float dist = .8f;
		if(!pc.FacingRight()) {
			dist *= -1;
		}
		markerClonePos.x += dist;
		markerClonePos.y += .3f;

		Rigidbody2D markerClone = Instantiate(inkMarker, markerClonePos, transform.rotation) as Rigidbody2D;
		MarkerController markerCloneCtrl = markerClone.GetComponent<MarkerController>();
		//markerCloneCtrl.player = gameObject;
		markerCloneCtrl.pa = this;

		if(!pc.FacingRight()) {
			markerClone.AddForce (Vector2.left * bulletSpeed);
			markerClone.gameObject.GetComponent<SpriteRenderer>().flipX = true;
		}
		else {
			markerClone.AddForce (Vector2.right * bulletSpeed);
			markerClone.gameObject.GetComponent<SpriteRenderer>().flipX = false;
		}


	}
}
