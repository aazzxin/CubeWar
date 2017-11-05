using UnityEngine;
using System.Collections;

public class Package : MonoBehaviour {
    [SerializeField]
    private GameObject ItemTemp;//用于实例化数组元素
    private Transform package;//即角色的package子对象

    public Weapon[] weapon=new Weapon[2];//0号位是远程，1号位是近程
    public int[] weaponNum = new int[2];
    private GameObject[] weaponTemp = new GameObject[2];
    GameObject rightWeaponPrefab;
    GameObject leftWeaponPrefab;

    public Item[] quickItem=new Item[4];
    public int[] quickItemNum = new int[4];
    private GameObject[] quickItemTemp = new GameObject[4];
    public Item[] bag=new Item[10];
    public int[] bagNum = new int[10];
    private GameObject[] bagItemTemp = new GameObject[10];

    public int coin;

    private int _quickkey;
    void Awake()//实例化所有元素
    {
        package = transform.Find("package");
        for (int i = 0; i < 2; i++)
        {
            weaponTemp[i] = Instantiate(ItemTemp) as GameObject;
            weaponTemp[i].transform.SetParent(package);
            weapon[i] = weaponTemp[i].GetComponent<Weapon>();
        }
        for (int i=0;i<4;i++)
        {
            quickItemTemp[i] = Instantiate(ItemTemp) as GameObject;
            quickItemTemp[i].transform.SetParent(package);
            quickItem[i] = quickItemTemp[i].GetComponent<Item>();
        }
        for (int i = 0; i < 10; i++)
        {
            bagItemTemp[i] = Instantiate(ItemTemp) as GameObject;
            bagItemTemp[i].transform.SetParent(package);
            bag[i] = bagItemTemp[i].GetComponent<Item>();
        }
    }
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        OpenPackage();
        QuickItemChange();
    }

    void OpenPackage()
    {
        if (Input.GetButtonDown("B"))
        {
            if (MainGameManager.uiManager.package_back.activeInHierarchy == false)
            {
                MainGameManager.uiManager.package_back.SetActive(true);
                MainGameManager.uiManager.package_icon.SetActive(true);
                MainGameManager.uiManager.package_num.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (MainGameManager.uiManager.package_back.activeInHierarchy == true)
            {
                bool turnFalse = false;
                if (MainGameManager.uiManager.Shop_Back != null) 
                {
                    if(MainGameManager.uiManager.Shop_Back.activeInHierarchy == false)
                    {
                        turnFalse = true;
                    }
                }
                else if (MainGameManager.uiManager.Warehouse_Back != null)
                {
                    if(MainGameManager.uiManager.Warehouse_Back.activeInHierarchy == false)
                    {
                        turnFalse = true;
                    }
                }
                else
                {
                    turnFalse = true;
                }
                if(turnFalse)
                {
                    MainGameManager.uiManager.package_back.SetActive(false);
                    MainGameManager.uiManager.package_icon.SetActive(false);
                    MainGameManager.uiManager.package_num.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;//锁定鼠标
                    Cursor.visible = false;
                }
            }
        }
    }

    public void SetWeapon()
    {
        if (System.Convert.ToInt32(weapon[0].GetID()) > 0 && weapon[0].GetCurrentQuantity() > 0)
        {
            //bool temp = MainGameManager.playerManager.rightWeapon.activeInHierarchy;
            //MainGameManager.playerManager.rightWeapon.SetActive(true);
            if (MainGameManager.playerManager.rightWeapon.transform.childCount > 0)
                Destroy(MainGameManager.playerManager.rightWeapon.transform.GetChild(0).gameObject);
            rightWeaponPrefab = Resources.Load("Item/Weapon/" + weapon[0].GetName()) as GameObject;
            GameObject rightWeapon = Instantiate(rightWeaponPrefab);
            rightWeapon.transform.SetParent(MainGameManager.playerManager.rightWeapon.transform);
            rightWeapon.transform.localPosition = rightWeaponPrefab.transform.position;
            rightWeapon.transform.localRotation = rightWeaponPrefab.transform.rotation;
            GameObject bullet = Resources.Load("bullet/" + weapon[0].GetName() + "_bullet") as GameObject;
            MainGameManager.playerManager.player.GetComponent<PlayerController>().SetBullet(bullet);
            //MainGameManager.playerManager.rightWeapon.SetActive(temp);
        }
        else if(System.Convert.ToInt32(weapon[0].GetID()) > 0 && weapon[0].GetCurrentQuantity() <= 0)
        {
            if (MainGameManager.playerManager.rightWeapon.transform.childCount > 0)
                Destroy(MainGameManager.playerManager.rightWeapon.transform.GetChild(0).gameObject);
            weapon[0].SetItem(ItemTemp.GetComponent<Weapon>());
            MainGameManager.playerManager.player.GetComponent<PlayerController>().SetTakeGun(false);
        }
        else
        {
            MainGameManager.playerManager.rightWeapon.SetActive(false);
            MainGameManager.playerManager.player.GetComponent<PlayerController>().SetTakeGun(false);
        }
        if (System.Convert.ToInt32(weapon[1].GetID()) > 0 && weapon[1].GetCurrentQuantity() > 0)
        {
            //bool temp = MainGameManager.playerManager.leftWeapon.active;
            //MainGameManager.playerManager.leftWeapon.SetActive(true);
            if (MainGameManager.playerManager.leftWeapon.transform.childCount > 0)
                Destroy(MainGameManager.playerManager.leftWeapon.transform.GetChild(0).gameObject);
            leftWeaponPrefab = Resources.Load("Item/Weapon/" + weapon[1].GetName()) as GameObject;
            GameObject leftWeapon = Instantiate(leftWeaponPrefab);
            leftWeapon.transform.SetParent(MainGameManager.playerManager.leftWeapon.transform);
            leftWeapon.transform.localPosition = leftWeaponPrefab.transform.position;
            leftWeapon.transform.localRotation = leftWeaponPrefab.transform.rotation;
            
            //MainGameManager.playerManager.leftWeapon.SetActive(temp);
        }
        else if (System.Convert.ToInt32(weapon[1].GetID()) > 0 && weapon[1].GetCurrentQuantity() > 0)
        {
            if (MainGameManager.playerManager.leftWeapon.transform.childCount > 0)
                Destroy(MainGameManager.playerManager.leftWeapon.transform.GetChild(0).gameObject);
            weapon[1].SetItem(ItemTemp.GetComponent<Weapon>());
        }
        else
        {
            MainGameManager.playerManager.leftWeapon.SetActive(false);
        }
    }

    //void ChangeItem();
    void QuickItemChange()
    {
        _quickkey = 0;
        if (Input.GetButtonDown("1")) _quickkey = 1;
        else if (Input.GetButtonDown("2")) _quickkey = 2;
        else if (Input.GetButtonDown("3")) _quickkey = 3;
        else if (Input.GetButtonDown("4")) _quickkey = 4;

        switch (_quickkey)
        {
            case 1:
                if(quickItem[0].GetCurrentQuantity()>0)
                {
                    UseItem(quickItem[0]);
                    Debug.Log("use 1");
                }
                _quickkey = 0;
                break;
            case 2:
                if (quickItem[1].GetCurrentQuantity() > 0)
                {
                    UseItem(quickItem[1]);
                    Debug.Log("use 2");
                }
                _quickkey = 0;
                break;
            case 3:
                if (quickItem[2].GetCurrentQuantity() > 0)
                {
                    UseItem(quickItem[2]);
                    Debug.Log("use 3");
                }
                _quickkey = 0;
                break;
            case 4:
                if (quickItem[3].GetCurrentQuantity() > 0)
                {
                    UseItem(quickItem[3]);
                    Debug.Log("use 4");
                }
                _quickkey = 0;
                break;
                //      default: break;
        }
    }

    bool TakeItem(Item item)
    { //捡取道具
        for (int i = 0; i < 4; i++)
        {
            if (System.Convert.ToInt32(quickItem[i].GetID()) == 0 || quickItem[i].GetCurrentQuantity() <= 0) 
            {
                //先改变该格子的道具类型
                if (item.GetItemType()==ItemType.Weapon)//武器
                {
                    quickItem[i] = quickItemTemp[i].GetComponent<Weapon>();
                }
                else if (item.GetItemType() == ItemType.RedMedicine)//红药
                {
                    quickItem[i] = quickItemTemp[i].GetComponent<RedMedicine>();
                }
                else if (item.GetItemType() == ItemType.BlueMedicine)//蓝药
                {
                }
                else if (item.GetItemType() == ItemType.BuffMedicine)//Buff
                {
                    quickItem[i] = quickItemTemp[i].GetComponent<BuffMedicine>();
                }
                else if (item.GetItemType() == ItemType.DebuffMedicine)//Debuff
                {
                    quickItem[i] = quickItemTemp[i].GetComponent<DebuffMedicine>();
                }
                MainGameManager.uiManager.SetQuickItemIconRef(i);

                quickItem[i].SetItem(item);
                quickItemNum[i] += item.GetCurrentQuantity();
                if (quickItemNum[i] > item.GetMaxQuantity())
                {
                    int resNumber = quickItemNum[i] - item.GetMaxQuantity();
                    quickItemNum[i] = item.GetMaxQuantity();
                    item.SetCurrentQuantity(resNumber);
                    quickItem[i].SetCurrentQuantity(resNumber);
                }
                else
                    Destroy(item.gameObject);

                MainGameManager.uiManager.SetQuickItem();
                return true;
            }
            else if (quickItem[i].GetID() == item.GetID())
            {
                if (quickItemNum[i] < item.GetMaxQuantity())
                {
                    quickItemNum[i] += item.GetCurrentQuantity();
                    if (quickItemNum[i] > item.GetMaxQuantity())
                    {
                        int resNumber = quickItemNum[i] - item.GetMaxQuantity();
                        quickItemNum[i] = item.GetMaxQuantity();
                        item.SetCurrentQuantity(resNumber);
                        quickItem[i].SetCurrentQuantity(resNumber);
                    }
                    else
                        Destroy(item.gameObject);

                    MainGameManager.uiManager.SetQuickItem();
                    return true;
                }
            }
        }
        for (int i = 0; i < 10; i++)
        {
            if (System.Convert.ToInt32(bagNum[i]) == 0) 
            {
                //背包中物品不需要更改类型，它不影响Use()
                //if (item.GetItemType()==ItemType.Weapon)//武器
                //{
                //    bag[i] = bagItemTemp[i].GetComponent<Weapon>();
                //}
                //else if (item.GetItemType() == ItemType.RedMedicine)//红药
                //{
                //    bag[i] = bagItemTemp[i].GetComponent<RedMedicine>();
                //}
                //else if (item.GetItemType() == ItemType.BlueMedicine)//蓝药
                //{
                //}
                //else if (item.GetItemType() == ItemType.BuffMedicine)//Buff
                //{
                //    bag[i] = bagItemTemp[i].GetComponent<BuffMedicine>();
                //}
                //else if (item.GetItemType() == ItemType.DebuffMedicine)//Debuff
                //{
                //    bag[i] = bagItemTemp[i].GetComponent<DebuffMedicine>();
                //}
                //MainGameManager.uiManager.SetBagIconRef(i);

                bag[i].SetItem(item);
                bagNum[i] += item.GetCurrentQuantity();
                if (bagNum[i] > item.GetMaxQuantity())
                {
                    int resNumber = bagNum[i] - item.GetMaxQuantity();
                    bagNum[i] = item.GetMaxQuantity();
                    item.SetCurrentQuantity(resNumber);
                    bag[i].SetCurrentQuantity(resNumber);
                }
                else
                    Destroy(item.gameObject);

                MainGameManager.uiManager.SetBag();
                return true;
            }
            else if (bag[i].GetID() == item.GetID())
            {
                if (bagNum[i] < item.GetMaxQuantity())
                {
                    bagNum[i] += item.GetCurrentQuantity();
                    if (bagNum[i] > item.GetMaxQuantity())
                    {
                        int resNumber = bagNum[i] - item.GetMaxQuantity();
                        bagNum[i] = item.GetMaxQuantity();
                        item.SetCurrentQuantity(resNumber);
                        bag[i].SetCurrentQuantity(resNumber);
                    }
                    else
                        Destroy(item.gameObject);

                    MainGameManager.uiManager.SetBag();
                    return true;
                }
            }
        }
        return false;
    }
    public int SpaceNum()//计算背包空格数。用于商店确认键，只有买入物品数小于等于空格数时，才能确认
    {
        int spaceNum = 0;
        for (int i = 0; i < 4; i++)
        {
            if (System.Convert.ToInt32(quickItem[i].GetID()) == 0 || quickItem[i].GetCurrentQuantity() <= 0)
            {
                spaceNum++;
            }
        }
        for (int i = 0; i < 10; i++)
        {
            if (System.Convert.ToInt32(bagNum[i]) == 0)
            {
                spaceNum++;
            }
        }
        return spaceNum;
    }
    public void BuyItem(Item item)//与TakeItem类似，只是这里不能叠加在同一类物品上，即必须含有空格子才能买入
    {
        for (int i = 0; i < 4; i++)
        {
            if (System.Convert.ToInt32(quickItem[i].GetID()) == 0 || quickItem[i].GetCurrentQuantity() <= 0)
            {
                //先改变该格子的道具类型
                if (item.GetItemType() == ItemType.Weapon)//武器
                {
                    quickItem[i] = quickItemTemp[i].GetComponent<Weapon>();
                }
                else if (item.GetItemType() == ItemType.RedMedicine)//红药
                {
                    quickItem[i] = quickItemTemp[i].GetComponent<RedMedicine>();
                }
                else if (item.GetItemType() == ItemType.BlueMedicine)//蓝药
                {
                }
                else if (item.GetItemType() == ItemType.BuffMedicine)//Buff
                {
                    quickItem[i] = quickItemTemp[i].GetComponent<BuffMedicine>();
                }
                else if (item.GetItemType() == ItemType.DebuffMedicine)//Debuff
                {
                    quickItem[i] = quickItemTemp[i].GetComponent<DebuffMedicine>();
                }
                MainGameManager.uiManager.SetQuickItemIconRef(i);

                quickItem[i].SetItem(item);
                quickItemNum[i] += item.GetCurrentQuantity();
                if (quickItemNum[i] > item.GetMaxQuantity())
                {
                    int resNumber = quickItemNum[i] - item.GetMaxQuantity();
                    quickItemNum[i] = item.GetMaxQuantity();
                    item.SetCurrentQuantity(resNumber);
                    quickItem[i].SetCurrentQuantity(resNumber);
                }
                else
                    Destroy(item.gameObject);

                MainGameManager.uiManager.SetQuickItem();
                return;
            }
        }
        for (int i = 0; i < 10; i++)
        {
            if (System.Convert.ToInt32(bagNum[i]) == 0)
            {
                bag[i].SetItem(item);
                bagNum[i] += item.GetCurrentQuantity();
                if (bagNum[i] > item.GetMaxQuantity())
                {
                    int resNumber = bagNum[i] - item.GetMaxQuantity();
                    bagNum[i] = item.GetMaxQuantity();
                    item.SetCurrentQuantity(resNumber);
                    bag[i].SetCurrentQuantity(resNumber);
                }
                else
                    Destroy(item.gameObject);

                MainGameManager.uiManager.SetBag();
                return;
            }
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Debug.Log("Found a item!");
            Item _item = coll.gameObject.GetComponent<Item>();
            bool ifPick = TakeItem(coll.gameObject.GetComponent<Item>());
        }
    }

    public void SetItemNum()//用于交换物品时数量的改变
    {
        for (int i = 0; i < 2; i++)
        {
            weaponNum[i] = weapon[i].GetCurrentQuantity();
        }
        for (int i = 0; i < 4; i++)
        {
            quickItemNum[i] = quickItem[i].GetCurrentQuantity();
        }
        for (int i = 0; i < 10; i++)
        {
            bagNum[i] = bag[i].GetCurrentQuantity();
        }
    }

    void UseItem(Item item)
    {
        item.Use();
    }
    void BagItemChange()
    {

    }//NGUI实现？？？

    public void SetQuickItemType(int i,ItemType itemType)//用于设定快捷键的道具类型，确保Use()的正确
    {
        if (itemType == ItemType.Weapon)//武器
        {
            quickItem[i] = quickItemTemp[i].GetComponent<Weapon>();
        }
        else if (itemType == ItemType.RedMedicine)//红药
        {
            quickItem[i] = quickItemTemp[i].GetComponent<RedMedicine>();
        }
        else if (itemType == ItemType.BlueMedicine)//蓝药
        {
        }
        else if (itemType == ItemType.BuffMedicine)//Buff
        {
            quickItem[i] = quickItemTemp[i].GetComponent<BuffMedicine>();
        }
        else if (itemType == ItemType.DebuffMedicine)//Debuff
        {
            quickItem[i] = quickItemTemp[i].GetComponent<DebuffMedicine>();
        }
    }
}
