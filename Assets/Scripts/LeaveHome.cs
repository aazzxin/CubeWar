using UnityEngine;
using System.Collections;

public class LeaveHome : MonoBehaviour {

	void OnTriggerEnter(Collider coll)
    {
        if(coll.tag=="Player")
        {
            Globe.SaveData();
        }
    }
    
}
