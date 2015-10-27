using UnityEngine;
using System.Collections;
using System.Linq;

public class playerController : MonoBehaviour {

	public float Speed = 5f;
	public bool Shield1 = true;
	public float HitStun;
	public float invul; // The amount of time the character is invulnerable after he gets hit.

	private Animator animator;
	private bool Hit;
	private bool noControl = false;	//True when the player shouldn't control the character

	private float timeHit;
	private ContactPoint2D contact;
	private GameObject[] enemies;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
		enemies = GameObject.FindGameObjectsWithTag ("Hazard");
		enemies = enemies.Concat(GameObject.FindGameObjectsWithTag ("Enemy")).ToArray();
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == "Hazard" || col.gameObject.tag == "Enemy") {
			contact = col.contacts [0];
			timeHit = Time.time;
			Hit = true;
			noControl = true;
			StartCoroutine("Blink");

			//Allow player to pass thorugh enemies
			foreach (GameObject enemy in enemies) {
				Physics2D.IgnoreCollision (this.GetComponent<Collider2D>(), enemy.GetComponent<Collider2D>(), true);
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");

		if (Hit) {
			damaged();
		} 
		if(!noControl) {
			bool slash = Input.GetButtonDown ("Sword");
			ManageMovement (h, v);
			animator.SetBool ("Shield1", Shield1);
			if (slash)
				animator.SetTrigger ("Slash");
		}
	}

	void ManageMovement(float horizontal,float vertical) {
		if (horizontal != 0f || vertical != 0f) {
			animator.SetBool ("moving", true);
			animateWalk (horizontal, vertical);
		} else {
			animator.SetBool ("moving", false);
		}
		Vector2 movement = new Vector2 (horizontal * Speed ,vertical * Speed);
		GetComponent<Rigidbody2D>().velocity = movement;
	}

	void animateWalk(float h,float v) {
		if(animator){
			if ((v > 0)&&(v>h)) {
				animator.SetInteger ("Direction", 1); 
			}
			if ((h > 0)&&(v<h)) { 
				animator.SetInteger ("Direction", 2);
			}
			if ((v < 0)&&(v<h)) {
				animator.SetInteger ("Direction", 3);
			}
			if ((h < 0 )&&(v>h)) {
				animator.SetInteger ("Direction", 4);
			}
		}
	}

	void damaged(){

		if (Time.time < timeHit + HitStun) {
			this.GetComponent<Rigidbody2D>().AddForce (contact.normal * 100, ForceMode2D.Force);	//Knockback Force
		} else {
			noControl = false;
		}

		if (Time.time > timeHit + invul) {
			Hit = false;	

			//Stop ignoring collisons with enemies
			foreach (GameObject item in enemies) {
				Physics2D.IgnoreCollision (this.GetComponent<Collider2D>(), item.GetComponent<Collider2D>(), false);
			}
		}
	}

	IEnumerator Blink(){
		float blinkTime = 0.08f; //Time between blinks
		float t = timeHit;
		Color color = Color.white;
		while(Time.time < (timeHit + invul)){
			if (Time.time > (t + blinkTime)) {
				if (this.GetComponent<Renderer>().material.color.a == 0) {
					color.a = 1;
				} else {
					color.a = 0;
				}
				t = Time.time;
				this.GetComponent<Renderer>().material.color = color;
			}
			yield return null;
		}
		color.a = 1;
		this.GetComponent<Renderer>().material.color = color;
	}
}
