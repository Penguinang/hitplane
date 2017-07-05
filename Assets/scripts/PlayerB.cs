using UnityEngine;
using System.Collections;

public class PlayerB : Player
{
	void Start ()
	{

	}

	override public void shoot()
	{
		Instantiate (bullet, GunPosition.transform.position, bullet.transform.rotation);
	}
}

