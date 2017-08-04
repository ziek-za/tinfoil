using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cull_rotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = (this.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized;
		this.transform.right = new Vector3(direction.x, direction.y);
	}
}
