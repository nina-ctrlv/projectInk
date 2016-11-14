using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarkerController : MonoBehaviour {

	public GameObject inkSplatter;
	public int maxSplatters;
	private int count;
	private ParticleSystem ps;
	private List<ParticleCollisionEvent> colEvents;

	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem>();
		colEvents = new List<ParticleCollisionEvent>();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col) {
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		GetComponent<ParticleSystem>().Play();
		GetComponent<SpriteRenderer>().enabled = false;
		Destroy(gameObject, .5f);
	}

	void OnParticleCollision(GameObject other) {
		if(count < maxSplatters) {
			ps.GetCollisionEvents(other, colEvents);
			Vector2 pos = colEvents[0].intersection;
			count++;

			Instantiate(inkSplatter, pos, transform.rotation);
		}
	}
}
