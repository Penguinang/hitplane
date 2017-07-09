using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// 工厂模式
public class PlayerFactory:NetworkBehaviour
{
	void Awake()
	{
		prefabpa = this.prefaba1;
		prefabpb = this.prefabb1;
		prefabpc = this.prefabc1;
	}

	void Start()
	{

	}

	void Update()
	{

	}
	// 单例的引用，是静态的
	static protected GameObject localplayer = null;
	static protected GameObject remoteplayer = null;
	public enum Nettype
	{
		SERVER,
		CILENT
	}

	public static void setlocalplayer(GameObject player)
	{
		if (!localplayer)
			localplayer = player;
	}
	public static void setremoteplayer(GameObject player)
	{
		if (!remoteplayer)
			remoteplayer = player;
	}

	//此处需要在工厂中用预设物体来生产玩家飞机，但是暂时不知道预设物体如何才能传给静态的变量，
	//所以先将工厂作为脚本绑定在一个GameObject中，用非静态的prefaba1获得预设，再赋值给静态的代表预设的变量
	public static GameObject prefabpa, prefabpb,prefabpc;
	public GameObject prefaba1, prefabb1,prefabc1;

	public virtual GameObject getlocalplayer()
	{return localplayer;}
	public virtual GameObject getremoteplayer()
	{return remoteplayer;}
}

public class PlayerAFactory:PlayerFactory
{
	public override GameObject getlocalplayer()
	{
		return  PlayerFactory.localplayer = PlayerFactory.localplayer == null ? Instantiate (prefabpa) : PlayerFactory.localplayer;
	}
	public override GameObject getremoteplayer()
	{
		return  PlayerFactory.remoteplayer =  PlayerFactory.remoteplayer == null ? Instantiate (prefabpa) :  PlayerFactory.remoteplayer;
	}

}

public class PlayerBFactory:PlayerFactory
{
	public override GameObject getlocalplayer()
	{
		return  PlayerFactory.localplayer = PlayerFactory.localplayer == null ? Instantiate (prefabpb) : PlayerFactory.localplayer;
	}
	public override GameObject getremoteplayer()
	{
		return  PlayerFactory.remoteplayer =  PlayerFactory.remoteplayer == null ? Instantiate (prefabpb) :  PlayerFactory.remoteplayer;
	}
}

public class PlayerCFactory:PlayerFactory
{
	public override GameObject getlocalplayer()
	{
		return  PlayerFactory.localplayer = PlayerFactory.localplayer == null ? Instantiate (prefabpc) : PlayerFactory.localplayer;
	}
	public override GameObject getremoteplayer()
	{
		return  PlayerFactory.remoteplayer =  PlayerFactory.remoteplayer == null ? Instantiate (prefabpc) :  PlayerFactory.remoteplayer;
	}
}