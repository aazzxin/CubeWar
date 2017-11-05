using UnityEngine;
using System.Collections;

public class MyAudioManager : MonoBehaviour,IGameManager {

    #region IGmaeManager
    public ManagerStatus status { get; private set; }

    public void Startup()
    {
        status = ManagerStatus.Started;
    }

    public void Reset()
    {

    }
    #endregion

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
