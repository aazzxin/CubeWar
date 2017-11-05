using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {
    public int num;

    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag=="Player")
        {
            coll.GetComponent<Package>().coin += num;
            Destroy(this.gameObject);
        }
    }
}
