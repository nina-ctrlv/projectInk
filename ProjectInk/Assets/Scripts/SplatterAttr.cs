using UnityEngine;
using System.Collections;

public class SplatterAttr : MonoBehaviour {

	public Material spriteMat;

	private float countDown;

	private bool atkTriggered;
	private int dmg;

	private Animator anim;

	// Use this for initialization
	void Start () {
		countDown = 15f;
		atkTriggered = false;
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(countDown <= 0 && !atkTriggered) {
			Destroy(gameObject);
		}
		else {
			countDown -= Time.deltaTime;
		}
	}

	// a = animation sequence number
	// d = dmg amount
	public void Atk(int a, int d) {
		atkTriggered = true;
		GetComponent<SpriteRenderer>().material = spriteMat;
		dmg = d;
		string s_anim = "";
		if(a == 0) {
			s_anim = "Spike";
			transform.position = new Vector3(transform.position.x, transform.position.y + .55f, transform.position.z);
		}
		StartCoroutine(PlayAtkAnim(s_anim));
	}

	// a = animation sequence name
	IEnumerator PlayAtkAnim(string a) {
		anim.enabled = true;
		anim.Play(a);
		float time = 0f;
		RuntimeAnimatorController ac = anim.runtimeAnimatorController;
		for(int i = 0; i < ac.animationClips.Length; i++) {
			if(ac.animationClips[i].name == a) {
				time = ac.animationClips[i].length;
			}
		}
		yield return new WaitForSeconds(time);
		Destroy(this.gameObject);
	}
}
