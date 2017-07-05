using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackingenemy : MonoBehaviour {
	public float Speedy = 0.5f,Speedx = 0.5f;

	public GameObject prefabbullet = null;
	public GameObject gunposition = null;
	public Vector3 planeposition;
	PlayerAFactory afactory = new PlayerAFactory();

	void Start () {
		InvokeRepeating ("shoot", 1, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {	
		float dirx;	
		planeposition = afactory.getplayer ().transform.position;
		if (Mathf.Abs (planeposition.x - transform.position.x) <= 0.3)
			dirx = 0;
		else {
			dirx = Mathf.Sign (planeposition.x - transform.position.x);
		}
		transform.position = new Vector3 (
			transform.position.x + dirx*Speedx,
			transform.position.y - Time.deltaTime * Speedy,
			transform.position.z
		);
	}

	void shoot()
	{
		Instantiate (prefabbullet, gunposition.transform.position, prefabbullet.transform.rotation);
	}
}
