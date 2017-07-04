using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	//外星飞船的生命值
	public int Health = 100;
	//外星飞船每秒移动的单元个数
	public float Speed = 1.0f;
	//外星飞船的移动范围
	public Vector2 MinMaxX = Vector2.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (MinMaxX.x + Mathf.PingPong (Time.time * Speed, 1.0f) * (MinMaxX.y - MinMaxX.x),
		                                 transform.position.y, transform.position.z);
	}
	void OnTriggerEnter(Collider other)
	{
		Health -= 20;
		if (Health <= 0) {
			Destroy (gameObject);
		}
		Destroy (other.gameObject);
		print ("enemy trigger");
	}

}
