using UnityEngine;
using System.Collections;

public enum SaveMode
{
    NewData,OldData
}
    
public class Globe : MonoBehaviour {
    //在这里记录当前切换场景的名称
    public static string loadName;

    public static float time;
    public static int kill;
    public static int ennemyDamage;
    public static int playerDamage;


    public static SaveMode saveMode = SaveMode.NewData;
    
    public static int HP, MaxHp, STR, DEF, MOS, ATS;
    public static float SP, MaxSp;
    public static string[] weaponID = new string[2];
    public static int[] weaponNum = new int[2];
    public static string[] quickItemID = new string[4];
    public static int[] quickItemNum = new int[4];
    public static string[] bagItemID = new string[10];
    public static int[] bagItemNum = new int[10];

    public static string[] warehouseItemID = new string[30];
    public static int[] warehouseItemNum = new int[30];

    void Start()
    {
        time = 0;
        kill = 0;
        ennemyDamage = 0;
        playerDamage = 0;

        MainGameManager.uiManager.SetBag();
        MainGameManager.uiManager.SetQuickItem();
        MainGameManager.uiManager.SetWeapon();
    }

    public static void SaveData()
    {
        Human human = MainGameManager.playerManager.human;
        Package package = MainGameManager.playerManager.package;
        Warehouse myWarehouse = MainGameManager.uiManager.warehouse;

        HP = human.HP;
        SP = human.SP;
        STR = human.STR;
        MaxHp = human.MaxHp;
        MaxSp = human.MaxSp;
        DEF = human.DEF;
        MOS = human.MOS;
        ATS = human.ATS;

        for (int i = 0; i < 2; i++)
        {
            weaponID[i] = package.weapon[i].GetID();
            weaponNum[i] = package.weaponNum[i];
            if (package.weapon[i] == null) i = 2;
        }

        for (int i = 0; i < 4; i++)
        {
            quickItemID[i] = package.quickItem[i].GetID();
            quickItemNum[i] = package.quickItemNum[i];
            if (package.quickItem[i] == null) i = 4;
        }

        for (int i = 0; i < 10; i++)
        {
            bagItemID[i] = package.bag[i].GetID();
            bagItemNum[i] = package.bagNum[i];
            if (package.bag[i] == null) i = 10;
        }
        if (myWarehouse != null)
        {
            for (int i = 0; i < 30; i++)
            {
                warehouseItemID[i] = myWarehouse.items[i].GetID();
                warehouseItemNum[i] = myWarehouse.itemsNum[i];
            }
        }
        

    }
}
