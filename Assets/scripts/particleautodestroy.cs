using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleautodestroy : MonoBehaviour {

	bool haveBeenPlayed = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<ParticleSystem> ().isPlaying)
			haveBeenPlayed = true;
		if (haveBeenPlayed) {
			if (GetComponent<ParticleSystem> ().isStopped) {
				Destroy (gameObject);
			}
		}
	}
}
