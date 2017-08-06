using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_position : MonoBehaviour {

	// Use this for initialization
	void Start () {
		floor_generation floor = GameObject.Find ("floor").GetComponent<floor_generation>();
		int width = floor.width;
		int height = floor.height;
		// Center floor
		Camera.main.transform.position = new Vector3 ((float)width / 2, (float)height / 2 + 0.4f, -10);
		Camera.main.GetComponent<camera_shake> ().SetPosition();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
