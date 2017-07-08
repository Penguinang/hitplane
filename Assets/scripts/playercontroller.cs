using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playercontroller : NetworkBehaviour {

	public GameObject myplayer;

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

	}

	void createserverplayer()
	{
		PlayerAFactory factory = new PlayerAFactory();
		GameObject player = factory.getlocalplayer ();
		myplayer = player;
		player.transform.parent = transform;
		player.transform.position = new Vector3 (0,0,0);
		NetworkServer.Spawn (player);
	}

	[Command]
	void Cmdcreateclientplayer()
	{
		PlayerAFactory afactory = new PlayerAFactory();
		GameObject player = afactory.getremoteplayer ();
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
				player.transform.parent = transform;
				player.transform.position = new Vector3 (0, 0, 0);
				myplayer = player;
				PlayerFactory.setlocalplayer (player);
			} else {
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


	public override void OnStartServer()
	{
		GameObject gamemanager = GameObject.FindGameObjectWithTag ("enemymanager");
		if (gamemanager)
			gamemanager.GetComponent<enemymanager> ().Activate ();
	}


}