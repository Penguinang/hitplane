using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class attackingenemy : NetworkBehaviour {
	public float Speedy = 0.5f,Speedx = 0.5f;

	public GameObject prefabbullet = null;
	public GameObject gunposition = null;
	public Vector3 localplaneposition,remoteplaneposition;
	PlayerFactory factory = new PlayerFactory();


	void Start () {
		InvokeRepeating ("shoot", 1, 1.5f);
		GetComponent<Rigidbody2D> ().velocity = new Vector3 (0,-1,0);
	}

	void Update () {	

	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "player")
			collider.transform.parent.gameObject.GetComponent<Player> ().abstractplayer.kissByEnemy ();
	}

	void FixedUpdate()
	{		
		if (!isServer)
			return;
		//判断移动方向
		float dirx;	
		//判断本地玩家与远程玩家哪一个在水平距离近攻击哪个
		if(factory.getlocalplayer())
			localplaneposition = factory.getlocalplayer ().transform.position;
		Vector3 position = localplaneposition;
		if (factory.getremoteplayer ()) {
			remoteplaneposition = factory.getremoteplayer ().transform.position;
			if (Mathf.Abs (remoteplaneposition.x - transform.position.x) < Mathf.Abs (localplaneposition.x - transform.position.x))
				position = remoteplaneposition;
		}
		//在一定距离内可以认为与玩家在同一横坐标上，防止太精确导致波动
		if (Mathf.Abs (position.x - transform.position.x) <= 0.3)
			dirx = 0;
		else {
			dirx = Mathf.Sign (position.x - transform.position.x);
		}
		GetComponent<Rigidbody2D> ().AddForce (new Vector3(dirx*0.5f,0,0));
	}

	void shoot()
	{
		Instantiate (prefabbullet, gunposition.transform.position, prefabbullet.transform.rotation);
	}
}
