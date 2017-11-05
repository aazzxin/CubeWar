using UnityEngine;
using System.Collections;

public class circleRotate : MonoBehaviour {
	public float rotate = 0.3f;
	// Use this for initialization

	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate(0, rotate, 0);
	}
}
