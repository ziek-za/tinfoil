using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
	private int life = 5;
	public GameObject usb_cap_prefab;
	private Vector3 usb_cap_oem_pos;
	private bool lost = false;

	void Start() {
		usb_cap_oem_pos = usb_cap_prefab.transform.position;
		// Correctly orientate USB cap
		UpdateUSB ();
	}

	public void ChangeHealth(bool up) {
		if (up) {
			life++;
		} else {
			life--;
		}

		life = Mathf.Min(Mathf.Max(life, 0), 5);

		UpdateUSB ();

		if (life <= 0) {
			YOULOSE (); // haha
		}
	}

	private void UpdateUSB() {
		usb_cap_prefab.transform.position = usb_cap_oem_pos;
		for (int i = 0; i < transform.childCount; i++) {
			if (i + 1 > life) {
				transform.GetChild (i).gameObject.SetActive (false);
			} else {
				usb_cap_prefab.transform.Translate (new Vector3 (0.5f,0,0));
				transform.GetChild (i).gameObject.SetActive (true);
			}
		}
	}

	private void YOULOSE() {
		if (!lost) {
			Invoke ("YOULOSE", 1f);
			lost = true;
		}

		// Update spawn rate (to zero)
		GameObject.Find("SpawnController").GetComponent<Spawning>().spawn_rate = Mathf.Infinity;

		// Remove all agents
		GameObject[] smiths = GameObject.FindGameObjectsWithTag("AgentSmith");
		for (int i = 0; i < smiths.Length; i++) {
			smiths [i].GetComponent<agent_movement> ().KillAgent();
		}
	}
}
