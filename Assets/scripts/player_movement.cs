using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour {

	public float speed = 0.5f;
	public float sliceSpeed = 20f;

	private Vector3 target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		target = new Vector3 (target.x, target.y, 0);
		Vector3 targetDir = (target - transform.position).normalized;

		if (Vector2.Distance(transform.position, target) > 0.1)
			transform.Translate(targetDir * speed * Time.deltaTime);

		//transform.position = new Vector3 (transform.position.x + xDist, transform.position.y + yDist, 0);

		//transform.position = new Vector3 (transform.position.x, transform.position.y, 0);

		if (Input.GetMouseButtonDown(0))
		{
			// TODO: Raycast to damage enemies
			// 	     Play slice animation
			transform.Translate(targetDir * sliceSpeed * Time.deltaTime);
		}


		if (target.x > transform.position.x) {
			transform.localScale = new Vector3 (1, 1, 1);
		} else {
			transform.localScale = new Vector3 (-1, 1, 1);
		}
			
	}
}
