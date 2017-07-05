using UnityEngine;
using System.Collections;

// 工厂模式
public class PlayerFactory:MonoBehaviour
{
	void Awake()
	{
		prefabpa = this.prefaba1;
		prefabpb = this.prefabb1;
		PlayerAFactory afactory = new PlayerAFactory();
		afactory.getplayer ();
	}

	void Start()
	{

	}

	void Update()
	{

	}
	// 单例的引用，是静态的
	static protected GameObject player;
	//此处需要在工厂中用预设物体来生产玩家飞机，但是暂时不知道预设物体如何才能传给静态的变量，
	//所以先将工厂作为脚本绑定在一个GameObject中，用非静态的prefaba1获得预设，再赋值给静态的代表预设的变量
	public static GameObject prefabpa, prefabpb;
	public GameObject prefaba1, prefabb1;

	public virtual GameObject getplayer()
	{return null;}
}

public class PlayerAFactory:PlayerFactory
{
	public override GameObject getplayer()
	{
		return player = player == null ? Instantiate (prefabpa) : player;
	}
}

public class PlayerBFactory:PlayerFactory
{
	public override GameObject getplayer()
	{
		return player = player == null ? Instantiate (prefabpb) : player;
	}
}