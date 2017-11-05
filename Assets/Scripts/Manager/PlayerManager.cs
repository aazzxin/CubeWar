using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour,IGameManager {

    #region IGmaeManager
    public ManagerStatus status { get; private set; }

    public void Startup()
    {
        status = ManagerStatus.Started;

        string direction = PlayerPrefs.GetString("direction");
        Room room = FindObjectOfType<Room>();
        if(room!=null)
        {
            Vector3 player_position = new Vector3(0, 0, 0);
            Quaternion player_rotation = new Quaternion(0, 0, 0, 0);
            switch (direction)
            {
                case "U":
                    player_position = room.southDoor.transform.position;
                    player_position = new Vector3(player_position.x, player_position.y, player_position.z + 2);
                    player_rotation = room.southDoor.transform.rotation;
                    break;
                case "D":
                    player_position = room.northDoor.transform.position;
                    player_position = new Vector3(player_position.x, player_position.y, player_position.z - 2);
                    player_rotation = room.northDoor.transform.rotation;
                    break;
                case "L":
                    player_position = room.eastDoor.transform.position;
                    player_position = new Vector3(player_position.x - 2, player_position.y, player_position.z);
                    player_rotation = room.eastDoor.transform.rotation;
                    break;
                case "R":
                    player_position = room.westDoor.transform.position;
                    player_position = new Vector3(player_position.x + 2, player_position.y, player_position.z);
                    player_rotation = room.westDoor.transform.rotation;
                    break;
                case "C":
                    float player_x = (room.westDoor.transform.position.x + room.eastDoor.transform.position.x) / 2;
                    float player_z = (room.southDoor.transform.position.z + room.northDoor.transform.position.z) / 2;
                    player_position = new Vector3(player_x, 1.5f, player_z);
                    break;
            }
            player = Instantiate(playerPrefab, player_position, player_rotation) as GameObject;
        }
        else
        {
            player = Instantiate(playerPrefab);
        }

        player.transform.SetParent(this.gameObject.transform);

        human = player.GetComponent<Human>();
        package = player.GetComponent<Package>();
        if (Globe.saveMode == SaveMode.OldData)
        {
            SetHuman();
            
            SetPackage();
        }
        leftWeapon = player.transform.Find("root").GetChild(2).GetChild(1).GetChild(1).gameObject;
        rightWeapon = player.transform.Find("root").GetChild(2).GetChild(1).GetChild(0).gameObject;
        package.SetWeapon();

    }

    public void Reset()
    {
        Startup();
    }
    #endregion

    public GameObject playerPrefab;

    public GameObject player;
    public Human human;
    public Package package;
    public GameObject leftWeapon;
    public GameObject rightWeapon;

    public void SetPlayerTransform()
    {
        if(player!=null)
        {
            string direction = PlayerPrefs.GetString("direction");
            Room room = FindObjectOfType<Room>();
            if (room != null)
            {
                Vector3 player_position = new Vector3(0, 0, 0);
                Quaternion player_rotation = new Quaternion(0, 0, 0, 0);
                switch (direction)
                {
                    case "U":
                        player_position = room.southDoor.transform.position;
                        player_position = new Vector3(player_position.x, player_position.y, player_position.z + 1);
                        player_rotation = room.southDoor.transform.rotation;
                        break;
                    case "D":
                        player_position = room.northDoor.transform.position;
                        player_position = new Vector3(player_position.x, player_position.y, player_position.z - 1);
                        player_rotation = room.northDoor.transform.rotation;
                        break;
                    case "L":
                        player_position = room.eastDoor.transform.position;
                        player_position = new Vector3(player_position.x - 1, player_position.y, player_position.z);
                        player_rotation = room.eastDoor.transform.rotation;
                        break;
                    case "R":
                        player_position = room.westDoor.transform.position;
                        player_position = new Vector3(player_position.x + 1, player_position.y, player_position.z);
                        player_rotation = room.westDoor.transform.rotation;
                        break;
                    case "C":
                        float player_x = (room.westDoor.transform.position.x + room.eastDoor.transform.position.x) / 2;
                        float player_z = (room.southDoor.transform.position.z + room.northDoor.transform.position.z) / 2;
                        player_position = new Vector3(player_x, 1.5f, player_z);
                        break;
                }
                player.transform.position = player_position;
                player.transform.rotation = player_rotation;
            }
            else
            {
                player.transform.position = playerPrefab.transform.position;
                player.transform.rotation = playerPrefab.transform.rotation;
            }
        }
        
    }
    public void ErasePackage()//死亡时清空背包
    {
        if(Globe.saveMode==SaveMode.OldData)
        {
            for (int i = 0; i < 2; i++)
            {
                PlayerPrefs.SetString("package_weapon" + i, "0");
                PlayerPrefs.SetInt("package_weaponNum" + i, 0);
            }

            for (int i = 0; i < 4; i++)
            {
                PlayerPrefs.SetString("package_quickItem" + i, "0");
                PlayerPrefs.SetInt("package_quickItemNum" + i, 0);
            }

            for (int i = 0; i < 10; i++)
            {
                PlayerPrefs.SetString("package_bag" + i, "0");
                PlayerPrefs.SetInt("package_bagNum" + i, 0);
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                Globe.weaponID[i] = "0";
                Globe.weaponNum[i] = 0;
            }

            for (int i = 0; i < 4; i++)
            {
                Globe.quickItemID[i] = "0";
                Globe.quickItemNum[i] = 0;
            }

            for (int i = 0; i < 10; i++)
            {
                Globe.bagItemID[i] = "0";
                Globe.bagItemNum[i] = 0;
            }
        }
    }


    public void SetHuman()
    {
        if(Globe.saveMode==SaveMode.OldData)
        {
            human.HP = PlayerPrefs.GetInt("Player_HP");
            human.SP = PlayerPrefs.GetFloat("Player_SP");
            human.STR = PlayerPrefs.GetInt("Player_STR");
            human.MOS = PlayerPrefs.GetInt("Player_MOS");
            human.ATS = PlayerPrefs.GetInt("Player_ATS");
            human.DEF = PlayerPrefs.GetInt("Player_DEF");
            human.MaxHp = PlayerPrefs.GetInt("Player_MaxHp");
            human.MaxSp = PlayerPrefs.GetFloat("Player_MaxSp");
        }
        else
        {
            human.HP = Globe.HP;
            human.SP = Globe.SP;
            human.STR = Globe.STR;
            human.MOS = Globe.MOS;
            human.ATS = Globe.ATS;
            human.DEF = Globe.DEF;
            human.MaxHp = Globe.MaxHp;
            human.MaxSp = Globe.MaxSp;
        }
    }
    public void SetPackage()
    {
        if (Globe.saveMode == SaveMode.OldData)
        {
            for (int i = 0; i < 2; i++)
            {
                package.weaponNum[i] = PlayerPrefs.GetInt("package_weaponNum" + i);
                package.weapon[i].SetItem(PlayerPrefs.GetString("package_weapon" + i));
                package.weapon[i].SetCurrentQuantity(package.weaponNum[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                package.quickItem[i].SetID(PlayerPrefs.GetString("package_quickItem" + i));
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
                package.quickItemNum[i] = PlayerPrefs.GetInt("package_quickItemNum" + i);
                package.quickItem[i].SetItem(PlayerPrefs.GetString("package_quickItem" + i));
                package.quickItem[i].SetCurrentQuantity(package.quickItemNum[i]);
            }

            for (int i = 0; i < 10; i++)
            {
                package.bagNum[i] = PlayerPrefs.GetInt("package_bagNum" + i);
                package.bag[i].SetItem(PlayerPrefs.GetString("package_bag" + i));
                package.bag[i].SetCurrentQuantity(package.bagNum[i]);
            }

            leftWeapon = player.transform.Find("root").GetChild(2).GetChild(1).GetChild(1).gameObject;
            rightWeapon = player.transform.Find("root").GetChild(2).GetChild(1).GetChild(0).gameObject;
            package.SetWeapon();
        }
        else
        {
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

            leftWeapon = player.transform.Find("root").GetChild(2).GetChild(1).GetChild(1).gameObject;
            rightWeapon = player.transform.Find("root").GetChild(2).GetChild(1).GetChild(0).gameObject;
            package.SetWeapon();
        }
    }
}
