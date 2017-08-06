using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
	
	public GameObject agent;
	public float kill_timer;

	// Use this for initialization
	void Start () {
		Invoke ("KillMyself", kill_timer);
	}

	void KillMyself() {
		agent = GameObject.Instantiate (agent, this.transform.position, Quaternion.identity) as GameObject;

		GameObject.Destroy (gameObject);
	}

	public void SetSpeedRangeForAgent(float speed_min, float speed_max) {
		agent.GetComponent<agent_movement> ().speed = Random.Range (speed_min, speed_max);
	}
}
