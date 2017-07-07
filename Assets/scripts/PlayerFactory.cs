﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// 工厂模式
public class PlayerFactory:NetworkBehaviour
{
	void Awake()
	{
		prefabpa = this.prefaba1;
		prefabpb = this.prefabb1;
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
	public static GameObject prefabpa, prefabpb;
	public GameObject prefaba1, prefabb1;

	public virtual GameObject getlocalplayer()
	{return null;}
	public virtual GameObject getremoteplayer()
	{return null;}
}

public class PlayerAFactory:PlayerFactory
{
	public override GameObject getlocalplayer()
	{
		return localplayer = localplayer == null ? Instantiate (prefabpa) : localplayer;
	}
	public override GameObject getremoteplayer()
	{
		return remoteplayer = remoteplayer == null ? Instantiate (prefabpa) : remoteplayer;
	}

}

public class PlayerBFactory:PlayerFactory
{
	
}