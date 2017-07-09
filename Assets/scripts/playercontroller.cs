using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playercontroller : NetworkBehaviour {

	public GameObject myplayer;
	public GameObject enemyPrefab;
	public GameObject initPosition;
	int playerSelection;

	void Start () {		
		myplayer = this.gameObject;

		GameObject.Find("gameinformationcontainer").transform.GetChild(0).gameObject.SetActive(true);

		if (!isLocalPlayer)
		{
			return;
		}
		if (isServer) {
			createserverplayer ();
		} else {
			playerSelection = GameObject.Find ("playerchoser").GetComponent<translatefunc> ().selectedIndex;
			Cmdcreateclientplayer (playerSelection);
			Destroy (GameObject.Find ("playerchoser"));
		}

	}

	void Update () {

	}

	void createserverplayer()
	{
		PlayerFactory factory;
		playerSelection = GameObject.Find ("playerchoser").GetComponent<translatefunc> ().selectedIndex;
		switch (playerSelection) {
		case 0:
			factory = new PlayerAFactory ();
			break;
		case 1:
			factory = new PlayerBFactory ();
			break;
		case 2:
			factory = new PlayerCFactory ();
			break;
		default:
			print ("no suit plane,give you a playerA");
			factory = new PlayerAFactory ();
			break;
		}
		Destroy (GameObject.Find ("playerchoser"));

		GameObject player = factory.getlocalplayer ();
		myplayer = player;
		player.transform.parent = transform;
		player.transform.position = transform.position;
		NetworkServer.Spawn (player);
	}

	[Command]
	void Cmdcreateclientplayer(int selection)
	{
		PlayerFactory factory;
		switch (selection) {
		case 0:
			factory = new PlayerAFactory ();
			break;
		case 1:
			factory = new PlayerBFactory ();
			break;
		case 2:
			factory = new PlayerCFactory ();
			break;
		default:
			print ("no suit plane,give you a playerA");
			factory = new PlayerAFactory ();
			break;
		}

		GameObject player = factory.getremoteplayer ();
		player.transform.parent = transform;
		player.transform.position = transform.position;
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
				player.transform.position = transform.position;
				myplayer = player;
				PlayerFactory.setlocalplayer (player);
			} else {
				GameObject[] virtualplayers = GameObject.FindGameObjectsWithTag("virtualplayer");
				GameObject servervirtualplayer = virtualplayers [0];
				if (servervirtualplayer == gameObject)
					servervirtualplayer = virtualplayers [1];
				player.transform.parent = servervirtualplayer.transform;
				player.transform.position = transform.position;
				PlayerFactory.setremoteplayer (player);

			}
		}
	}


	public override void OnStartServer()
	{
		GameObject enemymanager = GameObject.FindGameObjectWithTag ("enemymanager");
		if (enemymanager) {
			enemymanager.GetComponent<enemymanager> ().Activate ();
		}
	}


}