using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cull_rotation : MonoBehaviour {
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("TinfoilMan");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = (this.transform.position - player.transform.position).normalized;
		this.transform.right = new Vector3(direction.x, direction.y);
	}
}
