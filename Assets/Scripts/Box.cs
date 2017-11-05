using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Box : MonoBehaviour {
	private Animator _ani;
	public Image menu;
	private int count = 0;
	// Use this for initialization
	void Start () {
		_ani = GetComponent<Animator>();
		menu.gameObject.SetActive(false);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter() {
		Debug.Log("Opening!");
		_ani.Play("box_open");
		if (count == 0)
			StartCoroutine(ShowMenu());
		else
			menu.gameObject.SetActive(true);
	}

	IEnumerator ShowMenu() {

		yield return new WaitForSeconds(2.5f);
			menu.gameObject.SetActive(true);
			count ++;
	}


}
