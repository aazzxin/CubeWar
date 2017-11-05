using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public Image menu;
	public void StartGame() {
		SceneManager.LoadScene("Manager");
        Globe.saveMode = SaveMode.NewData;
	}

    public void ContinueGame()
    {
        SceneManager.LoadScene("Manager");
        Globe.saveMode = SaveMode.OldData;
    }

	public void Exit() {
		Application.Quit();
		Debug.Log("退出");
	}

	public void CloseMenu() {
		menu.gameObject.SetActive(false);
	}
}
