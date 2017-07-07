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
		GetComponent<Rigidbody2D> ().velocity = new Vector3 (0,-1,0);
	}
	
	// Update is called once per frame
	void Update () {	

	}

	void FixedUpdate()
	{
		float dirx;	
		planeposition = afactory.getlocalplayer ().transform.position;
		if (Mathf.Abs (planeposition.x - transform.position.x) <= 0.3)
			dirx = 0;
		else {
			dirx = Mathf.Sign (planeposition.x - transform.position.x);
		}
		GetComponent<Rigidbody2D> ().AddForce (new Vector3(dirx*0.5f,0,0));
	}

	void shoot()
	{
		Instantiate (prefabbullet, gunposition.transform.position, prefabbullet.transform.rotation);
	}
}
