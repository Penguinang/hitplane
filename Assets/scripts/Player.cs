using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : NetworkBehaviour {

	float ReloadDelay = 3f;
	public float Speedx = 4;
	public float Speedy = 1;
	public float Minx = -10.0f, Maxx = 10.0f;
	public float Miny = -10.0f, Maxy = 10.0f;

	public GameObject bulletPrefab = null;
	public GameObject GunPosition = null;
	public state currentstate = state.NORMAL;

	[SyncVar(hook = "UpdateHealth")]
	int health;

	public AbstractPlayer abstractplayer;

	[SyncVar(hook="UpdateScore")]
	public int score = 0;

	void Awake()
	{
		
	}

	void Start () {
		Invoke ("InitAbstractPlayer", 0.2f);
		PlayerFactory factory = new PlayerFactory ();

	}

	void InitAbstractPlayer()
	{
		switch (gameObject.GetComponent<playercontroller> ().playerSelection) {
		case 0:
			abstractplayer = new PlayerA ();
			break;
		case 1:
			abstractplayer = new PlayerB ();
			break;
		case 2:
			abstractplayer = new PlayerC ();
			break;
		default:
			print ("no suit type,give you a playerA");
			abstractplayer = new PlayerA ();
			break;
		}
		abstractplayer.Init ();
		ReloadDelay = abstractplayer.reloadDelay;
	}

	void Update () {
		//如果使用接口，就要将Health放在抽象类里面，也就是Player的成员的成员
		//如果想保持完全同步，就要蒋Health写在Player里面，作为Player的成员，使用Attribute[syncvar]
		if (isServer)
			if (abstractplayer != null) {
				health = abstractplayer.health;
			if (abstractplayer.health <= 0){
				Invoke ("die",0);	//服务器向客户端同步health需要一些时间
			}
		}	
	}
	void die()
	{
		Destroy (this.gameObject);
	}

	void FixedUpdate()
	{
		if (!isLocalPlayer)
			return;
		bool fire = Input.GetButton ("Fire1");
		if(fire)
			trychangestate (operation.FIRE);

		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis ("Vertical");
		transform.position = new Vector3 (
			Mathf.Clamp(transform.position.x + x * Time.deltaTime*Speedx,Minx,Maxx),
			Mathf.Clamp(transform.position.y + y * Time.deltaTime*Speedy,Miny,Maxy),
			transform.position.z
		);
	}
	void OnTriggerEnter2D(Collider2D collider)
	{

	}

	void UpdateScore(int score)
	{
		if(!isLocalPlayer)
			GameObject.Find ("remotescore").GetComponent<Text> ().text = "Remote Score\n"+score;
		else
			GameObject.Find ("localscore").GetComponent<Text> ().text = "Local Score\n"+score;
	}

	void UpdateHealth(int health)
	{
		if (isLocalPlayer) {
			GameObject.Find ("localhealth").GetComponent<Text> ().text = "LocalPlayer\n"+abstractplayer.maxHealth+"Max\n" + health;
			GameObject.Find ("localhealthbar").GetComponent<Slider> ().value = (float)health / abstractplayer.maxHealth;
		} else {
			GameObject.Find ("remotehealth").GetComponent<Text> ().text = "RemotePlayer\n"+abstractplayer.maxHealth+"Max\n" + health;
			GameObject.Find ("remotehealthbar").GetComponent<Slider> ().value = (float)health / abstractplayer.maxHealth;
		}
	}

	public void getScore()
	{
		score += 10;
	}

	void ActivateWeapons(){
		trychangestate (operation.TIMEXIANZHE);
	}
	void ActivateSkill()
	{
		trychangestate (operation.TIMESKILL);
	}

	[Command]
	void Cmdshoot()
	{
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			GunPosition.transform.position,
			GunPosition.transform.rotation);
		
		bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.forward * 6;

		NetworkServer.Spawn(bullet);
		Destroy(bullet, 2.0f);
		//cmd 在服务器执行，
		//因此可以只在服务器绑定子弹的Owner，只在服务器为玩家加分，用Syncvar同步到客户端，在每个端为每个飞机更新分数
		//用cmd，
		//只在服务器改变血量，用Syncvar同步，在客户端更新UI  × 因为血量所在的类没有绑定在玩家上，因此不能使用SyncVar
		bullet.GetComponent<bullet>().owner = gameObject;
	}

	public Vector3 getposition()
	{
		return this.transform.position;
	}

	//statemachine
	public enum state
	{
		NORMAL,
		XIANZHE,
		PNORMAL,
		PXIANZHE
	};
	public enum operation 
	{
		FIRE,
		SKILL,
		TIMESKILL,
		TIMEXIANZHE
	};
	public class transition
	{
		public state current;
		public operation command;
		public transition(state currentarg,operation commandarg)
		{
			this.current = currentarg;
			this.command = commandarg;
		}
		public override int GetHashCode()
		{
			return current.GetHashCode () * 5 + command.GetHashCode ();
		}
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			transition other = (transition)obj;
			return current == other.current && command == other.command;
		}
	}

	Dictionary<transition,state> alltransitions = new Dictionary<transition,state>
	{
		{new transition(state.NORMAL,operation.FIRE),state.XIANZHE},
		{new transition(state.NORMAL,operation.SKILL),state.PNORMAL},
		{new transition(state.XIANZHE,operation.TIMEXIANZHE),state.NORMAL},
		{new transition(state.XIANZHE,operation.SKILL),state.PXIANZHE},
		{new transition(state.PNORMAL,operation.TIMESKILL),state.NORMAL},
		{new transition(state.PNORMAL,operation.FIRE),state.PXIANZHE},
		{new transition(state.PXIANZHE,operation.TIMESKILL),state.XIANZHE},
		{new transition(state.PXIANZHE,operation.TIMEXIANZHE),state.PNORMAL}
	};

	public void trychangestate(operation command)
	{
		state nextstate;
		if (alltransitions.TryGetValue (new transition (currentstate, command), out nextstate)) {
			currentstate = nextstate;
			print ("\ncurrentstate changed to" + currentstate);
			switch (command) {
			case operation.FIRE:
				Cmdshoot ();
				Invoke ("ActivateWeapons", ReloadDelay);
				break;
			case operation.SKILL:
				print ("\nskill");
				break;
			case operation.TIMESKILL:
				print ("\ntimeskill");
				Invoke ("ActivateSkill",10);
				break;
			case operation.TIMEXIANZHE:
				print ("\nXIANZHE TIME");
				break;
			}
		}
		else {
			print ("\nno fit operation:current is" + currentstate + "command is" + command);
		}
	}
}
