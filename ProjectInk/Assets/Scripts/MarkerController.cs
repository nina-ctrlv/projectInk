using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarkerController : MonoBehaviour {

	public GameObject inkSplatter;
	public int maxSplatters;
	//public GameObject player;
	public PlayerAttack pa;
	private int count;
	private ParticleSystem ps;
	private List<ParticleCollisionEvent> colEvents;
	private ArrayList objs; // objects marked

	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem>();
		colEvents = new List<ParticleCollisionEvent>();	
		objs = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col) {
		if(col.gameObject.tag != "Splatter" && col.gameObject.tag != "Untitled" && col.gameObject.tag != "Marker") {
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			GetComponent<ParticleSystem>().Play();
			GetComponent<SpriteRenderer>().enabled = false;
			Destroy(gameObject, .5f);
		}
	}

	void OnParticleCollision(GameObject other) {
		if(count < maxSplatters && !objs.Contains(other.GetInstanceID()) && other.tag != "Splatter" && other.tag != "Untitled" && other.tag != "Marker") {
			ps.GetCollisionEvents(other, colEvents);
			Vector2 pos = colEvents[0].intersection;
			count++;

			GameObject s = Instantiate(inkSplatter, pos, transform.rotation) as GameObject;
			s.transform.parent = other.transform;
			objs.Add(other.GetInstanceID());
			pa.marks.Add(s);
		}
	}
}
