using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

	//移动的方向
	public Vector3 Direction = Vector3.up;
	//移动的速度
	public float Speed = 2.0f;
	//lifetime in seconds
	public float Lifetime = 10.0f;

	// Use this for initialization
	void Start () {
	
		//destroys ammo in lifetime
		Invoke ("DestroyMe", Lifetime);

	}
	
	// Update is called once per frame
	void Update () {
	
		//以一定的速度改变子弹的移动位置
		transform.position += Direction * Speed * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
	}

	

	void DestroyMe()
	{
		//销毁场景里的子弹对象
		Destroy (gameObject);
	}
}
