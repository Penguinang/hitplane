using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class enemymanager :  NetworkBehaviour{
	public GameObject enemyPrefab;
	public Transform enemySpawnLocation;
	protected int enemyNumber = 0;

	void Start () {
	}

	void Update () {

	}

	public void Activate()
	{
		if (!isServer)
			return;
		InvokeRepeating ("spawnEnemy",1,7);
	}

	void spawnEnemy()
	{
		GameObject enemy = Instantiate (enemyPrefab,new Vector3(Random.Range(-6.4f,6.4f),enemySpawnLocation.position.y,enemySpawnLocation.position.z),enemySpawnLocation.rotation);
		NetworkServer.Spawn (enemy);
		enemyNumber++;
	}
}
