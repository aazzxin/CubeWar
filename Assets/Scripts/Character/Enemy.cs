using UnityEngine;
using System.Collections;

public class Enemy : Character {

    private Animator _HPAni;

	// Use this for initialization
	void Start () {
        _HPAni = transform.Find("HP").Find("HPValue").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        _HPAni.SetFloat("Length", System.Convert.ToSingle(HP) / MaxHp);
    }
}
