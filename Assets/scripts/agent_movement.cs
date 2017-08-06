using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agent_movement : MonoBehaviour {

	Animator animator;
	SpriteRenderer spriteRenderer;
	public GameObject shadow;
	public GameObject binary_explosion;
	public float speed = 0.4f;

	float cableRate = 1f;

	private float width, height;
	private GameObject myComputer;
	private Vector3 target;
	private float idleTime = 1f;
	private bool laid = false;

	private LineRenderer lanCable;
	private List<Vector3> lanPoints;
	private bool attacking = false;
	private HealthBar health_bar;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer> ();

		floor_generation floor = GameObject.Find("floor").GetComponent<floor_generation>();
		width = floor.width;
		height = floor.height;

		myComputer = GameObject.Find ("myComputer_0");
		target = transform.position;

		lanCable = GetComponentInChildren<LineRenderer> ();
		lanPoints = new List<Vector3> ();
		lanPoints.Add (new Vector3 (getNearestWallPoint ().x, getNearestWallPoint ().y, 5));
		lanPoints.Add (new Vector3 (transform.position.x, transform.position.y, 5));

		InvokeRepeating ("LayCable",0, cableRate);

		health_bar = GameObject.Find ("HealthPoints").GetComponent<HealthBar> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Vector2.Distance(transform.position, target) < 0.01 && idleTime > 0) {
			idleTime -= Time.deltaTime;
		}

		// We are at the target
		if (Vector2.Distance(transform.position, target) < 0.75f && attacking && !laid) {
//			Debug.Log (target);

			Invoke ("CableDelay", 0.75f);

			animator.SetBool ("ReachedTarget", true);
			Invoke ("TurnOffRenderer", 1f);
			laid = true;
		} else {
			lanPoints[lanPoints.Count - 1] = transform.position;
		}

		if (Vector2.Distance(transform.position, target) < 0.01 && idleTime <= 0){
			int attacking_chance = Random.Range (0, 5);
			idleTime = 1;

			if (attacking_chance == 0) {
				attacking = true;
				target = myComputer.transform.position;
			} else {
				attacking = false;
				target = transform.position + (Random.insideUnitSphere * 3);

				while (target.x < -0.3f || target.x - 0.7f > width || target.y < 0 || target.y > height) {
					target = transform.position + (Random.insideUnitSphere * 3);
				}
				target = new Vector3 (target.x, target.y, 0);
			}
		} else {
			if (spriteRenderer.enabled) {
				Vector3 targetDir = (target - transform.position).normalized;
				gameObject.GetComponent<Rigidbody2D> ().velocity = 75 * (targetDir * speed * Time.deltaTime);
			} else {
				gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			}

			if (idleTime != 1) {
				animator.SetBool ("Walking", false);
				gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			} else {
				animator.SetBool ("Walking", true);
			}
		}

		lanCable.positionCount = lanPoints.Count;
		lanCable.SetPositions (lanPoints.ToArray());


		// Sprite faces correctly
		if (target.x > transform.position.x) {
			transform.localScale = new Vector3 (1, 1, 1);
		} else {
			transform.localScale = new Vector3 (-1, 1, 1);
		}

	}

	void CableDelay(){
		shadow.SetActive (false);
		health_bar.ChangeHealth (false);
		lanPoints.Add (new Vector3 (target.x, target.y, 5));

		lanCable.positionCount = lanPoints.Count;
		lanCable.SetPositions (lanPoints.ToArray());

		lanCable.transform.parent = null;
		gameObject.transform.Find ("PlugSound").GetComponent<AudioSource> ().Play ();
	}

	void LayCable(){
		lanPoints.Add (new Vector3 (transform.position.x, transform.position.y, 5));
		lanCable.positionCount = lanPoints.Count;
		lanCable.SetPositions (lanPoints.ToArray());
	}
		



	Vector2 getNearestWallPoint(){
		Vector2 max_x_wall = new Vector2 (transform.position.x, width);
		Vector2 min_x_wall = new Vector2 (transform.position.x, -1);
		Vector2 max_y_wall = new Vector2 (height, transform.position.y);
		Vector2 min_y_wall = new Vector2 (-1, transform.position.y);

		float max_x_dist = Vector2.Distance (transform.position, max_x_wall);
		float min_x_dist = Vector2.Distance (transform.position, min_x_wall);
		float max_y_dist = Vector2.Distance (transform.position, max_y_wall);
		float min_y_dist = Vector2.Distance (transform.position, min_y_wall);

		if (max_x_dist < min_x_dist && max_x_dist < max_y_dist && max_x_dist < min_y_dist)
			return max_x_wall;
		else if (min_x_dist < max_x_dist && min_x_dist < max_y_dist && min_x_dist < min_y_dist)
			return min_x_wall;
		else if (max_y_dist < min_x_dist && max_y_dist < max_x_dist && max_y_dist < min_y_dist)
			return max_y_wall;
		else
			return min_y_wall;
	}

	public void KillAgent() {
		animator.SetBool ("Dead", true);
		Invoke ("TurnOffRenderer", 0.1f);

	}

	void TurnOffRenderer() {
		if (spriteRenderer.enabled) {
			gameObject.transform.Find ("DieSound").GetComponent<AudioSource> ().Play ();
			GameObject.Find ("SpawnController").GetComponent<Spawning> ().UpdateKillCount ();

		}
		spriteRenderer.enabled = false;
		binary_explosion.SetActive (true);
		Invoke ("RemoveAgent", 1f);
	}

	void RemoveAgent() {
		
		GameObject.Destroy (gameObject);
	}
}
