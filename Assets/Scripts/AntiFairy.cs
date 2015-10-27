using UnityEngine;
using System.Collections;

public class AntiFairy : MonoBehaviour {
	
	public float Speed;
	public int Direction_x;
	public int Direction_y;
	
	private GameObject[] hazard;

	// Use this for initialization
	void Start () {
		hazard = GameObject.FindGameObjectsWithTag ("Hazard");

		GetComponent<Rigidbody2D>().velocity = new Vector2 (Direction_x * Speed, Direction_y * Speed);
		foreach (GameObject item in hazard) {
			Physics2D.IgnoreCollision (this.GetComponent<Collider2D>(), item.GetComponent<Collider2D>());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == "Terrain") {
			ContactPoint2D contact = col.contacts [0];
			if (contact.normal.x != 0)
				Direction_x = -Direction_x;
			if (contact.normal.y != 0)
				Direction_y = -Direction_y;
		}
		GetComponent<Rigidbody2D>().velocity = new Vector2 (Direction_x * Speed, Direction_y * Speed);
	}
}
