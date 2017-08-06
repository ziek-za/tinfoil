using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawning : MonoBehaviour {
	public GameObject[] portals_prefabs;
	public float spawn_rate;
	public Text kill_count;
	public Text splashText;
	public Image splashImage;
	public Transform introScreen;

	private float spawn_counter = 0;
	private float width, height;
	private int agents_killed = 0;

	private float difficulty_counter = 0;
	private float increase_difficulty = 10.0f; // Difficulty counter (10 s)
	private float speed_min, speed_max, speed_max_cap = 1.5f;

	private bool levelStarted = false;

	// Use this for initialization
	void Start () {
		floor_generation floor = GameObject.Find("floor").GetComponent<floor_generation>();
		width = floor.width;
		height = floor.height;
		speed_min = 0.4f;
		speed_max = speed_min;
		Invoke ("StartGame", 15f);
		Invoke ("FadeText", 13f);
	}
	
	// Update is called once per frame
	void Update () {
		if (levelStarted) {
			spawn_counter += Time.deltaTime;
			difficulty_counter += Time.deltaTime;

			if (spawn_counter > spawn_rate) {
				Vector3 spawn_location = new Vector3 (Random.Range (0, width), Random.Range (0, height), -1f);
				GameObject portal = portals_prefabs [Random.Range (0, portals_prefabs.Length)];

				GameObject new_portal = GameObject.Instantiate (portal, spawn_location, Quaternion.identity) as GameObject;
				new_portal.GetComponent<Portal> ().SetSpeedRangeForAgent (speed_min, speed_max);

				spawn_counter = 0;
			}

			if (difficulty_counter > increase_difficulty) {
				// Increase speed range
				speed_max += 0.2f;
				speed_max = Mathf.Min (speed_max, speed_max_cap);

				// Decrease spawn rate
				spawn_rate -= 0.1f;
				spawn_rate = Mathf.Max (0.1f, spawn_rate);

				difficulty_counter = 0;
			}
		} 
	}

	void FadeText(){
		Image[] images = introScreen.GetComponentsInChildren<Image> ();
		for (int i = 0; i < images.Length; i++){
			images[i].CrossFadeAlpha (0, 2, false);
		}

		Text[] text = introScreen.GetComponentsInChildren<Text> ();
		for (int i = 0; i < text.Length; i++){
			text[i].CrossFadeAlpha (0, 2, false);
		}

	}

	void StartGame(){
		levelStarted = true;
	}

	public void UpdateKillCount(){
		agents_killed++;
		kill_count.text = "agents_killed: " + agents_killed;
	}
}
