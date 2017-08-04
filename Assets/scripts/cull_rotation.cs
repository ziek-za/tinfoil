using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cull_rotation : MonoBehaviour {
	private GameObject player;
	private int max_distance;
	private Transform child;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("TinfoilMan");
		floor_generation floor = GameObject.Find("floor").GetComponent<floor_generation>();
		max_distance = (floor.width / 2);
		child = this.transform.GetChild (0);
	}
	
	// Update is called once per frame
	void Update () {
		// Update direction
		Vector3 direction = (this.transform.position - player.transform.position).normalized;
		this.transform.right = new Vector3(direction.x, direction.y);
		// Update scale based on distance away
		float distance_from_center = Vector2.Distance(this.transform.position, player.transform.position);
		float scale_ratio = max_distance / distance_from_center;
		child.localScale = new Vector3 (scale_ratio, 2, 1);
	}
}
