using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	//飞船的生命值
	public int Health = 100;
	public float Speed = 1;
	//子弹两发的间隔
	public float ReloadDelay = 0.2f;
	public Vector2 MinMaxX = Vector2.zero;
	public GameObject PrefabAmmo = null;
	public GameObject GunPosition = null;
	private bool WeaponsActivated = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float a = Input.GetAxis ("Horizontal");
		if (a!=0.0) {
			transform.position = 
			new Vector3 (
				Mathf.Clamp (
					transform.position.x + a * Speed * Time.deltaTime,
					MinMaxX.x,
					MinMaxX.y
				),
				transform.position.y,
				transform.position.z
			);
			print ("pressed button");
			
		}


	}
	void LateUpdate(){
		if (Input.GetButton ("Fire1") && WeaponsActivated) 
		{
			Instantiate (PrefabAmmo,GunPosition.transform.position,PrefabAmmo.transform.rotation);
			WeaponsActivated = false;
			Invoke ("ActivateWeapons",ReloadDelay);
		}
	}
	void ActivateWeapons(){
		WeaponsActivated = true;
	}
}
