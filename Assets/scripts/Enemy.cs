using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public AbstractEnemy abstractenemy;
	public ParticleSystem explode;
	void Start ()
	{
		abstractenemy = new NormalAbstractEnemy ();
		abstractenemy.Init ();
	}

	void Update ()
	{
		if (abstractenemy.health <= 0)
			Destroy (gameObject);
	}
}

