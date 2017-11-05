using UnityEngine;
using System.Collections;


public class ViewChange : MonoBehaviour {
	public float min = 150.0f;
	public float max = 400.0f;
	public float currentValue = 150.0f;
	public float changValue = 50.0f;
	// Use this for initialization
	void Start () {
		transform.localPosition = new Vector3(0, currentValue, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Q") && currentValue < max) { 
		    currentValue += changValue;
			Debug.Log("+");
		}

		if (Input.GetButtonDown("E") && currentValue > min) { 
		currentValue -= changValue;
		Debug.Log("-");
		}
			
		transform.localPosition = new Vector3(0, currentValue, 0);
	}

	public void BroadonVision() {
		if (currentValue < max)
			currentValue += changValue;
	}

	public void NarrowVision() {
		if (currentValue > min)
			currentValue -= changValue;
	
	}
}
