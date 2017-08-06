using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_layering : MonoBehaviour {

	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("TinfoilMan");
	}
	
	// Update is called once per frame
	void Update () {
		if (player.transform.position.y > transform.position.y) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
		} else {
			transform.position = new Vector3 (transform.position.x, transform.position.y, 1);
		}
	}
}
