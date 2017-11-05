using UnityEngine;
using System.Collections;

public class Warehouse : MonoBehaviour {
    private Animator _ani;

    private Transform warehouse;//子对象，用于存储ItemTemp;
    public Item[] items = new Item[30];
    public int[] itemsNum = new int[30];
    [SerializeField]
    private GameObject ItemTemp;
    private GameObject[] WarehouseItemTemp=new GameObject[30];
    void Awake()
    {
        warehouse = transform.Find("warehouse");
        for (int i = 0; i < 30; i++)
        {
            WarehouseItemTemp[i] = Instantiate(ItemTemp);
            WarehouseItemTemp[i].transform.SetParent(warehouse);
            items[i] = WarehouseItemTemp[i].GetComponent<Item>();

            items[i].SetItem(Globe.warehouseItemID[i]);
            itemsNum[i] = Globe.warehouseItemNum[i];
            items[i].SetCurrentQuantity(itemsNum[i]);
        }

        MainGameManager.uiManager.FindWarehouse();
        MainGameManager.uiManager.SetWarehouse();

    }
	// Use this for initialization
	void Start () {
        _ani = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag=="Player")
        {
            _ani.SetBool("Open", true);
            MainGameManager.uiManager.package_back.SetActive(true);
            MainGameManager.uiManager.package_icon.SetActive(true);
            MainGameManager.uiManager.package_num.SetActive(true);

            MainGameManager.uiManager.Warehouse_Back.SetActive(true);
            MainGameManager.uiManager.Warehouse_Icon.SetActive(true);
            MainGameManager.uiManager.Warehouse_Num.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if(coll.tag=="Player")
        {
            _ani.SetBool("Open", false);
            MainGameManager.uiManager.package_back.SetActive(false);
            MainGameManager.uiManager.package_icon.SetActive(false);
            MainGameManager.uiManager.package_num.SetActive(false);
            
            MainGameManager.uiManager.Warehouse_Back.SetActive(false);
            MainGameManager.uiManager.Warehouse_Icon.SetActive(false);
            MainGameManager.uiManager.Warehouse_Num.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;//锁定鼠标
            Cursor.visible = false;
        }
    }
    public void SetItemNum()//用于交换物品时数量的改变
    {
        for (int i = 0; i < 30; i++)
        {
            itemsNum[i] = items[i].GetCurrentQuantity();
        }
    }
    
}
