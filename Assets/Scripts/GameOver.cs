using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        MainGameManager.playerManager.ErasePackage();
        MainGameManager.playerManager.SetPackage();
        MainGameManager.uiManager.SetBag();
        MainGameManager.uiManager.SetQuickItem();
        MainGameManager.uiManager.SetWeapon();
        MainGameManager.playerManager.human.HP = Globe.HP;
        MainGameManager.playerManager.player.GetComponent<PlayerController>().SetDied(false);
        MainGameManager.playerManager.player.GetComponent<Animator>().SetBool("Died", false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
	
}
