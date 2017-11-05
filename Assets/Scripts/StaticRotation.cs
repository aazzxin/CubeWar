using UnityEngine;
using System.Collections;

public class StaticRotation : MonoBehaviour {

    private Transform _camera;



	// Use this for initialization
	void Start () {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(_camera.position);
	}
}
