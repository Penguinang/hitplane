using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playercontroller : NetworkBehaviour {

	public GameObject myplayer;

	public float Speedx = 1;
	public float Speedy = 1;
	public float Minx = -10.0f, Maxx = 10.0f;
	public float Miny = -10.0f, Maxy = 10.0f;

	void Start () {		
		myplayer = this.gameObject;

		if (!isLocalPlayer)
		{
			return;
		}
		if (isServer) {
			createserverplayer ();
		} else {
			Cmdcreateclientplayer ();
		}
	}

	void Update () {

		if (!isLocalPlayer)
			return;
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis ("Vertical");
		transform.position = new Vector3 (
			Mathf.Clamp(transform.position.x + x * Time.deltaTime*Speedx,Minx,Maxx),
			Mathf.Clamp(transform.position.y + y * Time.deltaTime*Speedy,Miny,Maxy),
			transform.position.z
		);
	}

	void createserverplayer()
	{
		PlayerAFactory factory = new PlayerAFactory();
		GameObject player = factory.getlocalplayer ();
		player.tag = "serverplayer";
		myplayer = player;
		myplayer.transform.parent = transform;
		myplayer.transform.position = new Vector3 (0,0,0);
		NetworkServer.Spawn (player);
	}

	[Command]
	void Cmdcreateclientplayer()
	{
		PlayerAFactory factory = new PlayerAFactory();
		GameObject player = factory.getremoteplayer ();
		player.tag = "clientplayer";
		GameObject[] virtualplayers= GameObject.FindGameObjectsWithTag ("virtualplayer");
		player.transform.parent = transform;
		player.transform.position = new Vector3 (0, 0, 0);
		NetworkServer.Spawn (player);
		Rpcmarkclient (player.GetComponent<NetworkIdentity> ().netId);

	}

	//通知客户端哪一个是客户端玩家
	[ClientRpc]
	void Rpcmarkclient(NetworkInstanceId clientplayerid)
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("player");
		foreach(GameObject player in objects)
		{
			if (player.GetComponent<NetworkIdentity> ().netId == clientplayerid) {
				player.tag = "clientplayer";
				player.transform.parent = transform;
				player.transform.position = new Vector3 (0, 0, 0);
				myplayer = player;
				PlayerFactory.setlocalplayer (player);
				print ("markclientplayer");
			} else {
				player.tag = "serverplayer";				
				GameObject[] virtualplayers = GameObject.FindGameObjectsWithTag("virtualplayer");
				GameObject servervirtualplayer = virtualplayers [0];
				if (servervirtualplayer == gameObject)
					servervirtualplayer = virtualplayers [1];
				player.transform.parent = servervirtualplayer.transform;
				player.transform.position = new Vector3 (0, 0, 0);
				PlayerFactory.setremoteplayer (player);

			}
		}
	}

}