using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgscroll : MonoBehaviour {
	public GameObject current;
	public GameObject next;

	public float speed = 0.0001f;
	void Start () {
		
	}

	void Update () {
	}

	void FixedUpdate()
	{
		transform.position += new Vector3(0,-1*speed,0);
		if (next.transform.position.y<0.01) {
			current.transform.position = new Vector3 (0, 10, 0)+current.transform.position;
			GameObject temp = current;
			current = next;
			next = temp;
		}
	}
}
