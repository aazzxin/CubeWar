using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {
    
    [SerializeField]
    private GameObject ItemTemp;//用于实例化数组元素
    private Transform shop;//子对象，用于存储ItemTemp
    public Item[] goodsItem = new Item[5];
    public int[] goodsItemNum = new int[5];
    public int[] goodsItemCost = new int[5];
    private GameObject[] goodsItemTemp = new GameObject[5];
    public Item[] consume = new Item[10];
    public int[] consumeNum = new int[10];
    private GameObject[] consumeItemTemp = new GameObject[10];
    public int[] buyOrSell = new int[10];//表示买还是卖，负数买入，正数为卖出。
    public int allCost = 0;
    //[SerializeField]
    //private Item[] goodsTest = new Item[5];

    private int x, y;
    [SerializeField]
    private SceneChange Door;
    // Use this for initialization
    void Awake () {
        x = PlayerPrefs.GetInt("Shop_x");
        y = PlayerPrefs.GetInt("Shop_y");
        PlayerPrefs.SetInt("isRead_" + x + "_" + y, 2);

        string doorRoomName = "";
        if (PlayerPrefs.GetInt("isInMap_" + (x - 1) + "_" + y) == 1)
        {
            doorRoomName = PlayerPrefs.GetString("room_name_" + (x - 2) + "_" + y);
            Door.direction = "L";
            PlayerPrefs.SetInt("isRead_" + (x - 2) + "_" + y, 1);
            PlayerPrefs.SetInt("isRead_" + (x - 1) + "_" + y, 1);
        }
            
        if (PlayerPrefs.GetInt("isInMap_" + x + "_" + (y + 1)) == 1)
        {
            doorRoomName = PlayerPrefs.GetString("room_name_" + x + "_" + (y + 2));
            Door.direction = "D";
            PlayerPrefs.SetInt("isRead_" + x + "_" + (y + 2), 1);
            PlayerPrefs.SetInt("isRead_" + x + "_" + (y + 1), 1);
        }
            
        if (PlayerPrefs.GetInt("isInMap_" + x + "_" + (y - 1)) == 1)
        {
            doorRoomName = PlayerPrefs.GetString("room_name_" + x + "_" + (y - 2));
            Door.direction = "U";
            PlayerPrefs.SetInt("isRead_" + x + "_" + (y - 2), 1);
            PlayerPrefs.SetInt("isRead_" + x + "_" + (y - 1), 1);
        }
            
        if (PlayerPrefs.GetInt("isInMap_" + (x + 1) + "_" + y) == 1)
        {
            doorRoomName = PlayerPrefs.GetString("room_name_" + (x + 2) + "_" + y);
            Door.direction = "R";
            PlayerPrefs.SetInt("isRead_" + (x + 2) + "_" + y, 1);
            PlayerPrefs.SetInt("isRead_" + (x + 1) + "_" + y, 1);
        }
        Door.nextscene_name = doorRoomName;
        
        
        shop = transform.Find("shop");
        for (int i = 0; i < 5; i++)
        {
            goodsItemTemp[i] = Instantiate(ItemTemp) as GameObject;
            goodsItemTemp[i].transform.SetParent(shop);
            goodsItem[i] = goodsItemTemp[i].GetComponent<Item>();
            while(true)
            {
                int index = Random.Range(1, 120);
                string itemName=PlayerPrefs.GetString(index + "name", "None");
                if(itemName!="None")
                {
                    goodsItem[i].SetItem(System.Convert.ToString(index));
                    int numTemp = Random.Range(1, PlayerPrefs.GetInt(index + "maxquantity", 0));
                    goodsItemNum[i] = numTemp;
                    goodsItem[i].SetCurrentQuantity(goodsItemNum[i]);
                    goodsItemCost[i] = goodsItem[i].GetVaule() * goodsItem[i].GetCurrentQuantity();
                    break;
                }
            }
        }
        //SetGoods();
        //for (int i = 0; i < 5; i++)
        //{
        //    goodsItemNum[i] = goodsItem[i].GetCurrentQuantity();
        //    goodsItemCost[i] = goodsItem[i].GetVaule() * goodsItem[i].GetCurrentQuantity();
        //}
        for (int i = 0; i < 10; i++)
        {
            consumeItemTemp[i] = Instantiate(ItemTemp) as GameObject;
            consumeItemTemp[i].transform.SetParent(shop);
            consume[i] = consumeItemTemp[i].GetComponent<Item>();
        }

        MainGameManager.uiManager.FindShop();
        MainGameManager.uiManager.SetShop();
        MainGameManager.uiManager.FindMinimap();
    }
	
	// Update is called once per frame
	void Update () {
        Globe.time += Time.deltaTime;
    }

    //void SetGoods()//初始化商品
    //{
    //    for(int i=0;i<5;i++)
    //    {
    //        goodsItem[i].SetItem(goodsTest[i]);
    //    }
    //}
    
    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag=="Player")
        {
            MainGameManager.uiManager.package_back.SetActive(true);
            MainGameManager.uiManager.package_icon.SetActive(true);
            MainGameManager.uiManager.package_num.SetActive(true);

            MainGameManager.uiManager.Shop_Back.SetActive(true);
            MainGameManager.uiManager.Shop_Icon.SetActive(true);
            MainGameManager.uiManager.Shop_Num.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    void OnTriggerExit(Collider coll)
    {
        if(coll.tag=="Player")
        {
            MainGameManager.uiManager.package_back.SetActive(false);
            MainGameManager.uiManager.package_icon.SetActive(false);
            MainGameManager.uiManager.package_num.SetActive(false);

            MainGameManager.uiManager.Shop_Back.SetActive(false);
            MainGameManager.uiManager.Shop_Icon.SetActive(false);
            MainGameManager.uiManager.Shop_Num.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;//锁定鼠标
            Cursor.visible = false;
        }
    }

    public void SetItemNum()//用于交换物品时数量的改变
    {
        for (int i = 0; i < 5; i++)
        {
            goodsItemNum[i] = goodsItem[i].GetCurrentQuantity();
            goodsItemCost[i] = goodsItem[i].GetVaule() * goodsItem[i].GetCurrentQuantity();
        }
        for (int i = 0; i < 10; i++)
        {
            consumeNum[i] = consume[i].GetCurrentQuantity();
        }
    }
    public void SetAllCost()
    {
        allCost = 0;
        for (int i = 0; i < 10; i++)
        {
            allCost += buyOrSell[i];
        }
    }
    public void Enter()//确认按钮，进行结算功能
    {
        int buysNum = 0;
        for (int i = 0; i < 10; i++)
        {
            if (buyOrSell[i] < 0)
            {
                buysNum++;
            }
        }
        if (MainGameManager.playerManager.package.SpaceNum() >= buysNum && MainGameManager.playerManager.package.coin >= -1 * allCost)
        {
            for (int i = 0; i < 10; i++)
            {
                if (buyOrSell[i] < 0)
                {
                    MainGameManager.playerManager.package.BuyItem(consume[i]);
                    consume[i].SetItem(ItemTemp.GetComponent<Item>());
                    MainGameManager.uiManager.ConsumeIconDrag[i].DragType = 3;
                }
                else if (buyOrSell[i] > 0)
                {
                    consume[i].SetItem(ItemTemp.GetComponent<Item>());
                    MainGameManager.uiManager.ConsumeIconDrag[i].DragType = 3;
                }
                buyOrSell[i] = 0;
            }

            MainGameManager.playerManager.package.coin += allCost;
            allCost = 0;

            MainGameManager.playerManager.package.SetItemNum();

            SetItemNum();

            MainGameManager.uiManager.SetInfo();
            MainGameManager.uiManager.SetBag();
            MainGameManager.uiManager.SetQuickItem();
            MainGameManager.uiManager.SetShop();
        }
    }
}
