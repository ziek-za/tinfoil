using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor_generation : MonoBehaviour {
	public GameObject floor_prefab;

	public int width;
	public int height;

	// Use this for initialization
	void Start () {
		// Generate floor tiling
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				GameObject.Instantiate (
					floor_prefab,
					new Vector3(x, y, 10),
					Quaternion.identity,
					this.transform
				);
			}
		}
		// Center floor
		Camera.main.transform.position = new Vector3 (width / 2, height / 2, -10);
	}

	// Update is called once per frame
	void Update () {

	}
}
