﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public int Health = 100;
	public float Speedx = 1;
	public float Speedy = 1;
	public float Minx = 0, Maxx = 0;
	public float Miny = 0, Maxy = 0;

	public float ReloadDelay = 3f;
	public GameObject bullet = null;
	public GameObject GunPosition = null;
	public state currentstate = state.NORMAL;

	void Start () {
	
	}

	void Update () {
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis ("Vertical");
		transform.position = new Vector3 (
			Mathf.Clamp(transform.position.x + x * Time.deltaTime*Speedx,Minx,Maxx),
			Mathf.Clamp(transform.position.y + y * Time.deltaTime*Speedy,Miny,Maxy),
			transform.position.z
		);

	}
	void LateUpdate(){
		bool fire = Input.GetButton ("Fire1");
		if(fire)
			trychangestate (operation.FIRE);
	}
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "enemybullet") 
		{
			Health -= 10;
			print ("plane get shot");
			Destroy (collider.gameObject);
			if (Health <= 0)
				Destroy (this.gameObject);
		} 
		else if (collider.gameObject.tag == "enemybody") 
		{
			Health -= 20;
			print ("plane get enemyed");
			if (Health <= 0)
				Destroy (this.gameObject);

		}
			
	}
	void ActivateWeapons(){
		trychangestate (operation.TIMEXIANZHE);
	}
	void ActivateSkill()
	{
		trychangestate (operation.TIMESKILL);
	}

	virtual public void shoot ()
	{}

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
				shoot ();
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