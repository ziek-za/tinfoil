using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slashbox : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("KillMe", 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D h) {
		if (h && h.gameObject.tag == "AgentSmith") {
			h.gameObject.GetComponent<agent_movement> ().KillAgent ();
		}
	}

	void KillMe() {
		GameObject.Destroy (gameObject);
	}
}
