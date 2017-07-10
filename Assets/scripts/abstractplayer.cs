using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

//实现抽象类与接口，在每一个敌方飞机或我方飞机脚本类内部都有一个抽象飞机类，提供TakeDamage（），Init（）等接口
abstract public class AbstractPlane
{
	public int health;
	public int maxHealth;
	public int reloadDelay;
	abstract public void TakeDamage();
	abstract public void Init();
}

abstract public class AbstractPlayer:AbstractPlane{
	public AbstractPlayer()
	{
	}

	abstract public void kissByEnemy ();
}

public class PlayerA : AbstractPlayer
{
	public override void Init()
	{
		maxHealth = 100;
		reloadDelay = 1;
		health = maxHealth;
	}
	public override void TakeDamage()
	{
		health -= 10;		
	}
	public override void kissByEnemy ()
	{
		health -= 20;
	}
}


public class PlayerB : AbstractPlayer
{
	public override void Init()
	{
		maxHealth = 200;
		reloadDelay = 3;
		health = maxHealth;
	}
	public override void TakeDamage()
	{
		health -= 10;			
	}
	public override void kissByEnemy ()
	{
		health -= 100;
	}
}

public class PlayerC : AbstractPlayer
{
	public override void Init()
	{
		maxHealth = 100;
		reloadDelay = 4;
		health = maxHealth;
	}
	public override void TakeDamage()
	{
		health -= 5;			
	}
	public override void kissByEnemy ()
	{
		health = 0;
	}
}

abstract public class AbstractEnemy:AbstractPlane{
	
}

public class NormalAbstractEnemy:AbstractEnemy
{
	public override void Init()
	{
		health = 10;
	}
	public override void TakeDamage()
	{
		health = 0;			
	}
}
public class SeniorAbstractEnemy:AbstractEnemy
{
	public override void Init()
	{
		health = 100;
	}
	public override void TakeDamage()
	{
		health -= 20;			
	}
}