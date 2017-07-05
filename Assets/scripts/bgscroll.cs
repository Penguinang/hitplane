using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgscroll : MonoBehaviour {
	public float maxy = 0;
	public float miny = 0;
	public float speed = 0.001f;
	private Vector3 startposition;
	void Start () {
		startposition = transform.position;
	}

	void Update () {
		float delta = miny+Mathf.Repeat (speed*Time.time, maxy - miny);
		transform.position = startposition + delta * Vector3.down;
		//transform.position = new Vector3 (transform.position.x,miny+Mathf.Repeat (-0.1f*Time.time, maxy-miny),0);
	}
}
