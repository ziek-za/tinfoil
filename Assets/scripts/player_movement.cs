using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour {

	public GameObject swipePrefab;
	public GameObject slashbox_prefab;
	public GameObject slashcircle_prefab;


	public float speed = 0.5f;
	public float sliceSpeed = 20f;
	float sliceCooldownValue = 0.7f;
	float sliceCooldown;

	float slamCooldownValue = 3f;
	float slamCooldown;

	float attackTimer;
	float attackAnimLength = 0.08f;

	private float width, height;
	private GameObject sliceInstance;
	private CharacterController characterController;

	Animator animator;

	private Vector3 target;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		sliceCooldown = 0;//sliceCooldownValue;
		slamCooldown = 0;//slamCooldownValue;
		attackTimer = 0;
		//characterController = gameObject.GetComponent<CharacterController> ();
		floor_generation floor = GameObject.Find("floor").GetComponent<floor_generation>();
		width = floor.width;
		height = floor.height;
	}
		
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			Application.LoadLevel (Application.loadedLevel);
		}

		// SEPERATION OF CONCERNS LINE -----------
		// Go away....
		if (sliceCooldown > 0) {
			sliceCooldown -= Time.deltaTime;
		} else {
			sliceCooldown = 0;
		}

		if (slamCooldown > 0) {
			slamCooldown -= Time.deltaTime;
		} else {
			slamCooldown = 0;
		}

		// SLam Attack
		if (Input.GetMouseButtonDown(1) && slamCooldown <= 0) {
			Invoke ("SlamMove", 0.4f);
			animator.SetBool ("MtnDew", true);
			slamCooldown = slamCooldownValue;
		} else {
			animator.SetBool ("MtnDew", false);
		}
		

		target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		target = new Vector3 (target.x, target.y, 0);
		Vector3 targetDir = (target - transform.position).normalized;




		if (Vector2.Distance(transform.position, target) > 0.1)
			//transform.Translate(targetDir * speed * Time.deltaTime);
			//characterController.Move(targetDir * speed * Time.deltaTime);
			gameObject.GetComponent<Rigidbody2D>().velocity = 75 * (targetDir * speed * Time.deltaTime);
		else
			gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.mtn_dew_anim")) {
			gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}

		// Slice attack
		if (Input.GetMouseButtonDown(0) && sliceCooldown <= 0 && !animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.mtn_dew_anim"))
		{
//			// Raycast to damage enemies
//			RaycastHit2D h = Physics2D.Linecast(transform.position, transform.position + targetDir * speed * Time.deltaTime * 2);
//
//			if (h.collider && h.collider.gameObject.tag == "AgentSmith") {
//				h.collider.gameObject.GetComponent<agent_movement> ().KillAgent ();
//			}

			float collider_angle = Vector2.Angle (Vector2.right, targetDir);

			if (target.y < this.transform.position.y) {
				collider_angle *= -1;
			}
		
			GameObject slash = GameObject.Instantiate (slashbox_prefab, this.transform.position, Quaternion.identity) as GameObject;
			slash.transform.RotateAround (this.transform.position, Vector3.forward, collider_angle);


			GameObject.Find ("SlashSound").GetComponent<AudioSource> ().Play ();

			//Play slice animation
			float angle = Vector2.Angle (Vector2.up, targetDir);
			sliceInstance = GameObject.Instantiate (swipePrefab, transform.position, Quaternion.identity) as GameObject;
			sliceInstance.transform.RotateAround(transform.position, Vector3.forward, angle);
			Invoke ("RemoveSlice", sliceCooldownValue - 0.1f);
			sliceCooldown = sliceCooldownValue;

			//Animate Char
			animator.SetBool ("Attacking", true);

			Camera.main.GetComponent<camera_shake> ().shakeDuration = sliceCooldownValue / 5;

			transform.Translate(targetDir * sliceSpeed * Time.deltaTime);
		}

		if (animator.GetBool("Attacking")) {
			attackTimer += Time.deltaTime;
		}

		if (attackTimer > 0 && attackTimer < attackAnimLength) {
			transform.Translate(targetDir * 10 * Time.deltaTime);
		} 

		if (attackTimer >= attackAnimLength) {
			attackTimer = 0;
			animator.SetBool ("Attacking", false);
		}

		// Fade slice
		if (sliceInstance) {
			Color alpha = sliceInstance.GetComponent<SpriteRenderer> ().material.color;
			alpha.a = 0;
			sliceInstance.GetComponent<SpriteRenderer> ().material.color = Color.Lerp (sliceInstance.GetComponent<SpriteRenderer> ().material.color, alpha, 4 * sliceCooldownValue * Time.deltaTime);
		}


		// Box collisions
		if (transform.position.x < -0.3f)
			transform.position = new Vector3 (-0.3f, transform.position.y, transform.position.z);
		if (transform.position.y < 0)
			transform.position = new Vector3 (transform.position.x, 0, transform.position.z);

		if (transform.position.x > width - 0.7f)
			transform.position = new Vector3 (width - 0.7f, transform.position.y, transform.position.z);
		if (transform.position.y > height)
			transform.position = new Vector3 (transform.position.x, height, transform.position.z);

		// Sprite faces correctly
		if (target.x > transform.position.x) {
			transform.localScale = new Vector3 (1, 1, 1);
		} else {
			transform.localScale = new Vector3 (-1, 1, 1);
		}
			
	}

	void SlamMove(){
		Camera.main.GetComponent<camera_shake> ().shakeDuration = slamCooldownValue / 12;
		GameObject.Instantiate (slashcircle_prefab, this.transform.position, Quaternion.identity);
		GameObject.Find ("StompWave").gameObject.GetComponent<ParticleSystem>().Play();
		GameObject.Find ("StompSound").GetComponent<AudioSource> ().Play ();
	}

	void RemoveSlice(){
		if (sliceInstance) {
			GameObject.Destroy (sliceInstance);
		}
	}
}
