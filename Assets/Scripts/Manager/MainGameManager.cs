using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MyUIManager))]
[RequireComponent(typeof(ItemManager))]
[RequireComponent(typeof(MyAudioManager))]
[RequireComponent(typeof(PlayerManager))]

public class MainGameManager : MonoBehaviour {

    private static MainGameManager _instance;
    private List<IGameManager> managers;

    public static PlayerManager playerManager;
    public static MyUIManager uiManager;
    public static ItemManager itemManager;
    public static MyAudioManager audioManager;
    
    void Awake()
    {
        _instance = this;
        managers = new List<IGameManager>();

        playerManager = GetComponent<PlayerManager>();
        uiManager = GetComponent<MyUIManager>();
        itemManager = GetComponent<ItemManager>();
        audioManager = GetComponent<MyAudioManager>();
        
        managers.Add(playerManager);
        managers.Add(uiManager);
        managers.Add(itemManager);
        managers.Add(audioManager);
        
        StartCoroutine(StartSetupManager());
    }

    IEnumerator StartSetupManager()
    {
        foreach (IGameManager manager in managers)
        {
            manager.Startup();
            yield return null;
            while (manager.status != ManagerStatus.Started)
            {
                yield return null;
            }
        }
    }

    public void ResetGame()
    {
        //foreach (IGameManager manager in managers)
        //{
        //    manager.Reset();
        //}
        StartCoroutine(StartSetupManager());
    }

    public static MainGameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);
	}
	
    public void SaveGame()
    {
        GameObject me = playerManager.player;
        Package package = playerManager.package;
        Warehouse myWarehouse = uiManager.warehouse;

        PlayerPrefs.SetString("Player_id", me.name);
        PlayerPrefs.SetInt("Player_HP", me.GetComponent<Human>().HP);
        PlayerPrefs.SetFloat("Player_SP", me.GetComponent<Human>().SP);
        PlayerPrefs.SetInt("Player_STR", me.GetComponent<Human>().STR);
        PlayerPrefs.SetInt("Player_MaxHp", me.GetComponent<Human>().MaxHp);
        PlayerPrefs.SetFloat("Player_MaxSp", me.GetComponent<Human>().MaxSp);
        PlayerPrefs.SetInt("Player_DEF", me.GetComponent<Human>().DEF);
        PlayerPrefs.SetInt("Player_MOS", me.GetComponent<Human>().MOS);
        PlayerPrefs.SetInt("Player_ATS", me.GetComponent<Human>().ATS);

        for (int i = 0; i < 2; i++)
        {
            PlayerPrefs.SetString("package_weapon" + i, package.weapon[i].GetID());
            PlayerPrefs.SetInt("package_weaponNum" + i, package.weaponNum[i]);
            if (package.weapon[i] == null) i = 2;
        }

        for (int i = 0; i < 4; i++)
        {
            PlayerPrefs.SetString("package_quickItem" + i, package.quickItem[i].GetID());
            PlayerPrefs.SetInt("package_quickItemNum" + i, package.quickItemNum[i]);
            if (package.quickItem[i] == null) i = 4;
        }

        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.SetString("package_bag" + i, package.bag[i].GetID());
            PlayerPrefs.SetInt("package_bagNum" + i, package.bagNum[i]);
            if (package.bag[i] == null) i = 10;
        }
        for (int i = 0; i < 30; i++)
        {
            PlayerPrefs.SetString("warehouse" + i, myWarehouse.items[i].GetID());
            PlayerPrefs.SetInt("warehouseNum" + i, myWarehouse.itemsNum[i]);
        }

        Application.LoadLevel("Welcome");
        Destroy(this.gameObject);
    }

    public void ReturnHome()
    {
        Human human = playerManager.human;
        human.HP = Globe.HP;
        human.SP = Globe.SP;
        human.STR = Globe.STR;
        human.MOS = Globe.MOS;
        human.ATS = Globe.ATS;
        human.DEF = Globe.DEF;
        human.MaxHp = Globe.MaxHp;
        human.MaxSp = Globe.MaxSp;
        Package package = playerManager.package;
        GameObject player = playerManager.player;
        for (int i = 0; i < 2; i++)
        {
            package.weaponNum[i] = Globe.weaponNum[i];
            package.weapon[i].SetItem(Globe.weaponID[i]);
            package.weapon[i].SetCurrentQuantity(package.weaponNum[i]);
        }

        for (int i = 0; i < 4; i++)
        {
            package.quickItem[i].SetID(Globe.quickItemID[i]);
            package.quickItem[i].SetItemType();
            //改变该格子的道具类型
            if (package.quickItem[i].GetItemType() == ItemType.Weapon)//武器
            {
                package.quickItem[i] = package.quickItem[i].gameObject.GetComponent<Weapon>();
            }
            else if (package.quickItem[i].GetItemType() == ItemType.RedMedicine)//红药
            {
                package.quickItem[i] = package.quickItem[i].gameObject.GetComponent<RedMedicine>();
            }
            else if (package.quickItem[i].GetItemType() == ItemType.BlueMedicine)//蓝药
            {
            }
            else if (package.quickItem[i].GetItemType() == ItemType.BuffMedicine)//Buff
            {
                package.quickItem[i] = package.quickItem[i].gameObject.GetComponent<BuffMedicine>();
            }
            else if (package.quickItem[i].GetItemType() == ItemType.DebuffMedicine)//Debuff
            {
                package.quickItem[i] = package.quickItem[i].gameObject.GetComponent<DebuffMedicine>();
            }
            package.quickItemNum[i] = Globe.quickItemNum[i];
            package.quickItem[i].SetItem(Globe.quickItemID[i]);
            package.quickItem[i].SetCurrentQuantity(package.quickItemNum[i]);
        }

        for (int i = 0; i < 10; i++)
        {
            package.bagNum[i] = Globe.bagItemNum[i];
            package.bag[i].SetItem(Globe.bagItemID[i]);
            package.bag[i].SetCurrentQuantity(package.bagNum[i]);
        }

        playerManager.leftWeapon = player.transform.Find("root").GetChild(2).GetChild(1).GetChild(1).gameObject;
        playerManager.rightWeapon = player.transform.Find("root").GetChild(2).GetChild(1).GetChild(0).gameObject;
        package.SetWeapon();



        Globe.loadName = "Home";
        PlayerPrefs.SetString("direction", "C");
        Application.LoadLevel("Loading");
    }
}
