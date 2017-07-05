using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public int Health = 100;
	public float Speed = 1.0f;
	public Vector2 MinMaxX = Vector2.zero;
	public GameObject bullet = null;
	public GameObject gunposition = null;

	void Start () {
		InvokeRepeating ("shoot", 3, 1);
	
	}

	void Update () {
		transform.position = new Vector3 (MinMaxX.x + Mathf.PingPong (Time.time * Speed, 1.0f) * (MinMaxX.y - MinMaxX.x),
		                                 transform.position.y, transform.position.z);
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "playerbullet") {
			Health -= 20;
			if (Health <= 0) {
				Destroy (gameObject);
			}
			Destroy (other.gameObject);
			print ("enemy trigger");
		}
	}

	public void shoot()
	{
		Instantiate (bullet, gunposition.transform.position, bullet.transform.rotation);
	}
}
