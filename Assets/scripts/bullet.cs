using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class bullet : NetworkBehaviour {

	//移动的方向
	public Vector3 Direction = Vector3.up;
	//移动的速度
	public float Speed = 2.0f;
	//lifetime in seconds
	public float Lifetime = 10.0f;
	public GameObject owner;
	// Use this for initialization
	void Start () {
		Invoke ("DestroyMe", Lifetime);
	}
	
	// Update is called once per frame
	void Update () {	
		//以一定的速度改变子弹的移动位置
		transform.position += Direction * Speed * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		//这里遇到了接口的问题，抽象类不能作为此处的模板，所以抽象类只能是这里模板的一个成员
		if (gameObject.tag == "enemybullet" && collider.gameObject.tag == "player") {
			collider.transform.parent.GetComponent<Player> ().abstractplayer.TakeDamage ();
			Destroy (gameObject);
		} else if (gameObject.tag == "playerbullet" && collider.gameObject.tag == "enemy") {
			collider.GetComponent<Enemy> ().abstractenemy.TakeDamage ();
			Destroy (gameObject);
			if (isServer)
				owner.GetComponent<Player> ().getScore ();
		}		
	}

	

	void DestroyMe()
	{
		//销毁场景里的子弹对象
		Destroy (gameObject);
	}
}
