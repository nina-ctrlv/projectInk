using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float speed;

	private Vector2 prevPos;
	private Vector2 tarPos;
	private Vector2 lerpPos;

	// Use this for initialization
	void Start () {
		prevPos = new Vector2(target.position.x, target.position.y);	
		//transform.position = prevPos;
	}
	
	// Update is called once per frame
	void Update () {
		tarPos = new Vector2(target.position.x, target.position.y);
		//dist = Vector3.Distance(prevPos, tarPos);
        lerpPos = Vector2.Lerp(prevPos, tarPos, Time.deltaTime * speed);


        transform.position = new Vector3(lerpPos.x, lerpPos.y, -5f);
        prevPos = lerpPos;
	}
}
