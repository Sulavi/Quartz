using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {

	private Transform player;
	public bool Track = true;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(Track)
			TrackPlayer ();	
	}

	void TrackPlayer ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = player.position.x;
		float targetY = player.position.y;
		
		transform.position = new Vector3 (targetX, targetY, transform.position.z);
		                                
	}
}
