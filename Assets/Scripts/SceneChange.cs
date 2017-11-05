using UnityEngine;
using System.Collections;
using UnityEditor;

//using UnityEditor.SceneManagement;
public class SceneChange : MonoBehaviour {

    public GameObject me;
    public string nextscene_name;
    public Package package;
    //public GameObject playerPrefab;
    //public Vector3 player_position;
    //public Quaternion player_rotation;
    public string direction;//UDLRC，分别是上下左右中

    public void OnTriggerStay(Collider coll)
    {
        if(coll.tag=="Player")
        {
            Tran();
        }
    }
    public void Tran()
    {
        //SavePlayerData();
        //DestroyObject(me);
        PlayerPrefs.SetString("direction", direction);

        Globe.loadName = nextscene_name;
        Application.LoadLevel("Loading");

        //EditorSceneManager.LoadScene(nextscene_id);
        //LoadPlayerData();
    }

    //void SavePlayerData()
    //{
    //    me = GameObject.FindWithTag("Player");
    //    package = me.GetComponent<Package>();

    //    PlayerPrefs.SetString("Player_id", me.name);
    //    PlayerPrefs.SetInt("Player_HP", me.GetComponent<Human>().HP);
    //    PlayerPrefs.SetFloat("Player_SP", me.GetComponent<Human>().SP);
    //    PlayerPrefs.SetInt("Player_STR", me.GetComponent<Human>().STR);
    //    PlayerPrefs.SetInt("Player_MaxHp", me.GetComponent<Human>().MaxHp);
    //    PlayerPrefs.SetFloat("Player_MaxSp", me.GetComponent<Human>().MaxSp);
    //    PlayerPrefs.SetInt("Player_DEF", me.GetComponent<Human>().DEF);
    //    PlayerPrefs.SetInt("Player_MOS", me.GetComponent<Human>().MOS);
    //    PlayerPrefs.SetInt("Player_ATS", me.GetComponent<Human>().ATS);

    //    for(int i = 0; i < 2; i++)
    //    {
    //        PlayerPrefs.SetString("package_weapon" + i, package.weapon[i].GetID());
    //        PlayerPrefs.SetInt("package_weaponNum" + i, package.weaponNum[i]);
    //    }

    //    for (int i = 0; i < 4; i++)
    //    {
    //        PlayerPrefs.SetString("package_quickItem" + i, package.quickItem[i].GetID());
    //        PlayerPrefs.SetInt("package_quickItemNum" + i, package.quickItemNum[i]);
    //    }

    //    for (int i = 0; i < 10; i++)
    //    {
    //        PlayerPrefs.SetString("package_bag" + i, package.bag[i].GetID());
    //        PlayerPrefs.SetInt("package_bagNum" + i, package.bagNum[i]);
    //    }


    //}
    
    //void LoadPlayerData()
    //{
    //    me = Instantiate(playerPrefab, player_position, player_rotation) as GameObject;
    //    //MainGameManager.playerManager.player = Instantiate(playerPrefab, player_position, player_rotation) as GameObject;
    //    me = GameObject.FindWithTag("Player");
    //    me.GetComponent<Human>().HP = PlayerPrefs.GetInt("Player_HP");
    //    me.GetComponent<Human>().SP = PlayerPrefs.GetFloat("Player_SP");
    //    me.GetComponent<Human>().STR = PlayerPrefs.GetInt("Player_STR");
    //    me.GetComponent<Human>().MOS = PlayerPrefs.GetInt("Player_MOS");
    //    me.GetComponent<Human>().ATS = PlayerPrefs.GetInt("Player_ATS");
    //    me.GetComponent<Human>().DEF = PlayerPrefs.GetInt("Player_DEF");
    //    me.GetComponent<Human>().MaxHp = PlayerPrefs.GetInt("Player_MaxHp");
    //    me.GetComponent<Human>().MaxSp = PlayerPrefs.GetFloat("Player_MaxSp");
    //    Package new_package = me.GetComponent<Package>();
    //    new_package = package;

    //}
    
}
