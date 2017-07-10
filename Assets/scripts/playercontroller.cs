using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playercontroller : NetworkBehaviour {

	public GameObject myplayer;
	public GameObject enemyPrefab;
	public GameObject initPosition;
	public int playerSelection;

	void Start () {		
		GameObject.Find ("networkmanager").GetComponent<NetworkManagerHUD> ().showGUI = false;
		myplayer = this.gameObject;

		GameObject.Find("gameinformationcontainer").transform.GetChild(0).gameObject.SetActive(true);

		if (!isLocalPlayer)
		{
			return;
		}
		if (isServer) {
			playerSelection = GameObject.Find ("playerchoser").GetComponent<translatefunc> ().selectedIndex;
			Destroy (GameObject.Find ("playerchoser"));
			createserverplayer ();
		} else {
			playerSelection = GameObject.Find ("playerchoser").GetComponent<translatefunc> ().selectedIndex;
			Destroy (GameObject.Find ("playerchoser"));
			Cmdcreateclientplayer (playerSelection);
		}

	}

	void Update () {

	}

	void createserverplayer()
	{
		PlayerFactory factory;
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
		playerSelection = selection;
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

		GameObject player = factory.getremoteplayer ();
		player.transform.parent = transform;
		player.transform.position = transform.position;
		NetworkServer.Spawn (player);

		GameObject[] virtualplayers = GameObject.FindGameObjectsWithTag("virtualplayer");
		GameObject servervirtualplayer = virtualplayers [0];
		if (servervirtualplayer == gameObject)
			servervirtualplayer = virtualplayers [1];
		int serverPlayerSelection = servervirtualplayer.GetComponent<playercontroller> ().playerSelection;

		Rpcmarkclient (player.GetComponent<NetworkIdentity> ().netId,serverPlayerSelection);
	}

	//通知客户端哪一个是客户端玩家
	[ClientRpc]
	void Rpcmarkclient(NetworkInstanceId clientplayerid,int serverPlayerSelection)
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
				servervirtualplayer.GetComponent<playercontroller> ().playerSelection = serverPlayerSelection;
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