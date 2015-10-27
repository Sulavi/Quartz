using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

	public int Health;

	private bool Hit;
	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == "Hazard" || col.gameObject.tag == "Enemy") {
			Health--;
			if (Health == 0) {
				animator.SetTrigger ("Death");
			}
		}
	}
}
