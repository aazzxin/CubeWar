using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyUIManager : MonoBehaviour, IGameManager {

    #region IGmaeManager
    public ManagerStatus status { get; private set; }

    public void Startup()
    {
        status = ManagerStatus.Started;

        _player = MainGameManager.playerManager.player;
        _human = MainGameManager.playerManager.human;
        _package = MainGameManager.playerManager.package;
        HP.text = System.Convert.ToString(_human.HP);

        

        STR = InfoValue.transform.Find("STR").GetComponent<Text>();
        DEF = InfoValue.transform.Find("DEF").GetComponent<Text>();
        ATS = InfoValue.transform.Find("ATS").GetComponent<Text>();
        MOS = InfoValue.transform.Find("MOS").GetComponent<Text>();
        
        
        for (int i=0;i<10;i++)
        {
            GameObject BagIconFrameChild = BagIconFrame.transform.GetChild(i).gameObject;
            BagIcon[i] = BagIconFrameChild.GetComponentInChildren<Image>();

            ItemDrag BagIconDrag = BagIconFrameChild.GetComponent<ItemDrag>();
            BagIconDrag.SetRef(_package.bag[i]);

            BagNum[i] = BagNumFrame.transform.GetChild(i).GetComponentInChildren<Text>();
        }

        for(int i=0;i<4;i++)
        {
            GameObject QuickIconFrameChild = QuickItemIconFrame.transform.GetChild(i).gameObject;
            QuickItemIcon[i] = QuickIconFrameChild.GetComponentInChildren<Image>();

            ItemDrag QuickIconDrag = QuickIconFrameChild.GetComponent<ItemDrag>();
            QuickIconDrag.SetRef(_package.quickItem[i]);

            QuickItemNum[i] = QuickItemNumFrame.transform.GetChild(i).GetComponentInChildren<Text>();


            GameObject QuickIconFrameChild2 = QuickItemIconFrame2.transform.GetChild(i).gameObject;
            QuickItemIcon2[i] = QuickIconFrameChild2.GetComponentInChildren<Image>();
            ItemDrag QuickIconDrag2 = QuickIconFrameChild2.GetComponent<ItemDrag>();
            QuickIconDrag2.SetRef(_package.quickItem[i]);
            QuickItemNum2[i] = QuickItemNumFrame2.transform.GetChild(i).GetComponentInChildren<Text>();
        }

        leftWeaponIcon = GameObject.Find("leftWeaponIcon_Package").GetComponent<Image>();
        ItemDrag leftWeaponIconDrag = leftWeaponIcon.transform.parent.GetComponent<ItemDrag>();
        leftWeaponIconDrag.SetRef(_package.weapon[1]);
        rightWeaponIcon = GameObject.Find("rightWeaponIcon_Package").GetComponent<Image>();
        ItemDrag rightWeaponIconDrag = rightWeaponIcon.transform.parent.GetComponent<ItemDrag>();
        rightWeaponIconDrag.SetRef(_package.weapon[0]);
        leftWeaponIcon2 = GameObject.Find("leftWeaponIcon_UI").GetComponent<Image>();
        ItemDrag leftWeaponIconDrag2 = leftWeaponIcon2.transform.parent.GetComponent<ItemDrag>();
        leftWeaponIconDrag2.SetRef(_package.weapon[1]);
        rightWeaponIcon2 = GameObject.Find("rightWeaponIcon_UI").GetComponent<Image>();
        ItemDrag rightWeaponIconDrag2 = rightWeaponIcon2.transform.parent.GetComponent<ItemDrag>();
        rightWeaponIconDrag2.SetRef(_package.weapon[0]);

        leftWeaponText = GameObject.Find("leftWeaponNum_Package").GetComponent<Text>();
        rightWeaponText = GameObject.Find("rightWeaponNum_Package").GetComponent<Text>();
        leftWeaponText2 = GameObject.Find("leftWeaponNum_UI").GetComponent<Text>();
        rightWeaponText2 = GameObject.Find("rightWeaponNum_UI").GetComponent<Text>();

        FindWarehouse();
        for (int i = 0; i < 30; i++)
        {
            GameObject WarehouseIconFrameChild = Warehouse_Icon.transform.GetChild(0).GetChild(i).gameObject;
            WarehouseIcon[i] = WarehouseIconFrameChild.GetComponentInChildren<Image>();
            WarehouseNum[i] = Warehouse_Num.transform.GetChild(0).GetChild(i).GetComponentInChildren<Text>();
        }
        FindShop();
        for (int i = 0; i < 5; i++)
        {
            GameObject GoodsIconFrameChild = Shop_Icon.transform.GetChild(0).GetChild(i).gameObject;
            goodsIcon[i] = GoodsIconFrameChild.GetComponentInChildren<Image>();
            goodsNum[i] = Shop_Num.transform.GetChild(0).GetChild(i).GetComponentInChildren<Text>();
            goodsCost[i] = Shop_Num.transform.GetChild(2).GetChild(i).GetComponentInChildren<Text>();
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject ConsumeIconFrameChild = Shop_Icon.transform.GetChild(1).GetChild(i).gameObject;
            consumeIcon[i] = ConsumeIconFrameChild.GetComponentInChildren<Image>();
            consumeNum[i] = Shop_Num.transform.GetChild(1).GetChild(i).GetComponentInChildren<Text>();
        }
        allCost = Shop_Num.transform.GetChild(3).GetComponentInChildren<Text>();

        SetBag();
        SetQuickItem();
        SetWeapon();
        SetWarehouse();
        SetShop();
        
        //MainGameManager.playerManager.rightWeapon.SetActive(false);

        package_back.SetActive(false);
        package_icon.SetActive(false);
        package_num.SetActive(false);

        Warehouse_Back.SetActive(false);
        Warehouse_Icon.SetActive(false);
        Warehouse_Num.SetActive(false);


        Shop_Back.SetActive(false);
        Shop_Icon.SetActive(false);
        Shop_Num.SetActive(false);
        
    }

    public void Reset()
    {
        Start();
    }
    #endregion

    

    private GameObject _player;
    private Human _human;
    private Package _package;
    public Text HP;
    public Animator HPImageAni;
    public Animator HPPivotAni;
    public Animator SPPivotAni;

    public GameObject WeaponFrame;
    private Image leftWeaponIcon;
    private Image rightWeaponIcon;//表示背包的武器图标栏
    private Text leftWeaponText;
    private Text rightWeaponText;

    public GameObject WeaponFrame2;
    private Image leftWeaponIcon2;
    private Image rightWeaponIcon2;//2表示是在UI面板上
    private Text leftWeaponText2;
    private Text rightWeaponText2;

    public GameObject package_back;
    public GameObject package_icon;
    public GameObject package_num;
    public GameObject InfoValue;
    private Text STR;
    private Text DEF;
    private Text ATS;
    private Text MOS;
    public Text CoinNum;

    public GameObject BagIconFrame;
    private Image[] BagIcon = new Image[10];
    public GameObject BagNumFrame;
    private Text[] BagNum = new Text[10];

    public GameObject QuickItemIconFrame;
    private Image[] QuickItemIcon = new Image[4];
    public GameObject QuickItemNumFrame;
    private Text[] QuickItemNum = new Text[4];
    public GameObject QuickItemIconFrame2;
    private Image[] QuickItemIcon2 = new Image[4];
    public GameObject QuickItemNumFrame2;
    private Text[] QuickItemNum2 = new Text[4];

    public Warehouse warehouse;
    public GameObject Warehouse_Back;
    public GameObject Warehouse_Icon;
    public GameObject Warehouse_Num;
    private Image[] WarehouseIcon = new Image[30];
    private Text[] WarehouseNum = new Text[30];

    public Shop shop;
    public GameObject Shop_Back;
    public GameObject Shop_Icon;
    public GameObject Shop_Num;
    private Image[] goodsIcon = new Image[5];
    private Text[] goodsNum = new Text[5];
    private Text[] goodsCost = new Text[5];
    private Image[] consumeIcon = new Image[10];
    private Text[] consumeNum = new Text[10];
    public ItemDrag[] ConsumeIconDrag = new ItemDrag[10];
    private Text allCost;


    private GameObject _miniMapCanvas;
    private GameObject _miniMap;
    private GameObject _tileMap;


    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update ()
    {
        SetHP();
        SetInfo();

        SetMinimap();

        SetMenu();
    }
    
    void SetMinimap()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(_tileMap.activeInHierarchy)
            {
                _miniMap.SetActive(true);
                _tileMap.SetActive(false);
            }
            else
            {
                _tileMap.SetActive(true);
                _miniMap.SetActive(false);
            }
        }
    }
    void SetHP()
    {
        HP.text = System.Convert.ToString(_human.HP);
        HPImageAni.SetFloat("Color", System.Convert.ToSingle(_human.HP) / _human.MaxHp);
        HPPivotAni.SetFloat("Length", System.Convert.ToSingle(_human.HP) / _human.MaxHp);
        SPPivotAni.SetFloat("Length", System.Convert.ToSingle(_human.SP) / _human.MaxSp);
    }

    public void SetInfo()
    {
        STR.text = System.Convert.ToString(_human.STR);
        DEF.text = System.Convert.ToString(_human.DEF);
        ATS.text = System.Convert.ToString(_human.ATS);
        MOS.text = System.Convert.ToString(_human.MOS);

        CoinNum.text = System.Convert.ToString(_package.coin);
    }
    

    public void SetBag()
    {
        for (int i = 0; i < 10; i++)
        {
            if (System.Convert.ToInt32(_package.bag[i].GetID()) <= 0 || _package.bag[i].GetCurrentQuantity() <= 0)
            {
                BagIcon[i].color = new Color(255, 255, 255, 0);//设为透明
            }
            else
            {
                BagIcon[i].color = new Color(255, 255, 255, 255);//满透明度
                BagIcon[i].overrideSprite= Resources.Load("Icon/" + _package.bag[i].GetName() + "_icon", typeof(Sprite)) as Sprite;
            }

            if (_package.bagNum[i] <= 0)
            {
                BagNum[i].gameObject.SetActive(false);
            }
            else
            {
                BagNum[i].gameObject.SetActive(true);
                BagNum[i].text = System.Convert.ToString(_package.bagNum[i]);
            }
        }
    }
    public void SetBagIconRef(int i)//捡取道具改变了Item类型时，也需要重新更改指向
    {
        GameObject BagIconFrameChild = BagIconFrame.transform.GetChild(i).gameObject;
        ItemDrag BagIconDrag = BagIconFrameChild.GetComponent<ItemDrag>();
        BagIconDrag.SetRef(_package.bag[i]);
    }
    public void SetQuickItem()
    {
        for(int i=0;i<4;i++)
        {
            if (System.Convert.ToInt32(_package.quickItem[i].GetID()) <= 0 || _package.quickItem[i].GetCurrentQuantity() <= 0)
            {
                QuickItemIcon[i].color = new Color(255, 255, 255, 0);
                QuickItemIcon2[i].color = new Color(255, 255, 255, 0);
            }
            else
            {
                QuickItemIcon[i].color = new Color(255, 255, 255, 255);
                QuickItemIcon2[i].color = new Color(255, 255, 255, 255);
                QuickItemIcon[i].overrideSprite = Resources.Load("Icon/" + _package.quickItem[i].GetName() + "_icon", typeof(Sprite)) as Sprite;
                QuickItemIcon2[i].overrideSprite = Resources.Load("Icon/" + _package.quickItem[i].GetName() + "_icon", typeof(Sprite)) as Sprite;
            }

            if (_package.quickItemNum[i] <= 0) 
            {
                QuickItemNum[i].gameObject.SetActive(false);
                QuickItemNum2[i].gameObject.SetActive(false);
            }
            else
            {
                QuickItemNum[i].gameObject.SetActive(true);
                QuickItemNum2[i].gameObject.SetActive(true);
                QuickItemNum[i].text = System.Convert.ToString(_package.quickItemNum[i]);
                QuickItemNum2[i].text = System.Convert.ToString(_package.quickItemNum[i]);
            }
        }
    }
    public void SetQuickItemIconRef(int i)
    {
        GameObject QuickIconFrameChild = QuickItemIconFrame.transform.GetChild(i).gameObject;
        ItemDrag QuickIconDrag = QuickIconFrameChild.GetComponent<ItemDrag>();
        QuickIconDrag.SetRef(_package.quickItem[i]);
        GameObject QuickIconFrameChild2 = QuickItemIconFrame2.transform.GetChild(i).gameObject;
        ItemDrag QuickIconDrag2 = QuickIconFrameChild2.GetComponent<ItemDrag>();
        QuickIconDrag2.SetRef(_package.quickItem[i]);
    }
    //public void SetWeaponIconRef(int i)
    //{
    //    ItemDrag leftWeaponIconDrag = leftWeaponIcon.transform.parent.GetComponent<ItemDrag>();
    //    leftWeaponIconDrag.SetRef(_package.weapon[1]);
    //    ItemDrag rightWeaponIconDrag = leftWeaponIcon.transform.parent.GetComponent<ItemDrag>();
    //    rightWeaponIconDrag.SetRef(_package.weapon[0]);
    //    ItemDrag leftWeaponIconDrag2 = leftWeaponIcon2.transform.parent.GetComponent<ItemDrag>();
    //    leftWeaponIconDrag2.SetRef(_package.weapon[1]);
    //    ItemDrag rightWeaponIconDrag2 = leftWeaponIcon2.transform.parent.GetComponent<ItemDrag>();
    //    rightWeaponIconDrag2.SetRef(_package.weapon[0]);
    //}
    public void SetWeapon()
    {
        //rightWeapon
        if (System.Convert.ToInt32(_package.weapon[0].GetID()) <= 0)
        {
            rightWeaponIcon.color = new Color(255, 255, 255, 0);//设为透明
            rightWeaponIcon2.color = new Color(255, 255, 255, 0);//设为透明
        }
        else
        {
            rightWeaponIcon.color = new Color(255, 255, 255, 255);//满透明度
            rightWeaponIcon.overrideSprite = Resources.Load("Icon/" + _package.weapon[0].GetName() + "_icon", typeof(Sprite)) as Sprite;
            rightWeaponIcon2.color = new Color(255, 255, 255, 255);//满透明度
            rightWeaponIcon2.overrideSprite = Resources.Load("Icon/" + _package.weapon[0].GetName() + "_icon", typeof(Sprite)) as Sprite;
        }
        if (_package.weaponNum[0] <= 0)
        {
            rightWeaponText.gameObject.SetActive(false);
            rightWeaponText2.gameObject.SetActive(false);
        }
        else
        {
            rightWeaponText.gameObject.SetActive(true);
            rightWeaponText.text = System.Convert.ToString(_package.weaponNum[0]);
            rightWeaponText2.gameObject.SetActive(true);
            rightWeaponText2.text = System.Convert.ToString(_package.weaponNum[0]);
        }
        //leftWeapon
        if (System.Convert.ToInt32(_package.weapon[1].GetID()) <= 0)
        {
            leftWeaponIcon.color = new Color(255, 255, 255, 0);//设为透明
            leftWeaponIcon2.color = new Color(255, 255, 255, 0);//设为透明
        }
        else
        {
            leftWeaponIcon.color = new Color(255, 255, 255, 255);//满透明度
            leftWeaponIcon.overrideSprite = Resources.Load("Icon/" + _package.weapon[1].GetName() + "_icon", typeof(Sprite)) as Sprite;
            leftWeaponIcon2.color = new Color(255, 255, 255, 255);//满透明度
            leftWeaponIcon2.overrideSprite = Resources.Load("Icon/" + _package.weapon[1].GetName() + "_icon", typeof(Sprite)) as Sprite;
        }
        if (_package.weaponNum[1] <= 0)
        {
            leftWeaponText.gameObject.SetActive(false);
            leftWeaponText2.gameObject.SetActive(false);
        }
        else
        {
            leftWeaponText.gameObject.SetActive(true);
            leftWeaponText.text = System.Convert.ToString(_package.weaponNum[1]);
            leftWeaponText2.gameObject.SetActive(true);
            leftWeaponText2.text = System.Convert.ToString(_package.weaponNum[1]);
        }
    }
    //public void SetWarehouseIconRef(int i)
    //{
    //    GameObject WarehouseIconFrameChild = Warehouse_Icon.transform.GetChild(i).gameObject;
    //    ItemDrag WarehouseconDrag = WarehouseIconFrameChild.GetComponent<ItemDrag>();
    //    WarehouseconDrag.SetRef(_package.quickItem[i]);
    //}
    public void FindWarehouse()
    {
        //Warehouse
        warehouse = FindObjectOfType<Warehouse>();
        if (warehouse != null)
        {
            for (int i = 0; i < 30; i++)
            {
                GameObject WarehouseIconFrameChild = Warehouse_Icon.transform.GetChild(0).GetChild(i).gameObject;
                ItemDrag WarehouseIconDrag = WarehouseIconFrameChild.GetComponent<ItemDrag>();
                WarehouseIconDrag.SetRef(warehouse.items[i]);
            }
        }
        
    }
    public void SetWarehouse()
    {
        if (warehouse != null)
        {
            for (int i = 0; i < 30; i++)
            {
                if (System.Convert.ToInt32(warehouse.items[i].GetID()) <= 0 || warehouse.items[i].GetCurrentQuantity() <= 0)
                {
                    WarehouseIcon[i].color = new Color(255, 255, 255, 0);
                }
                else
                {
                    WarehouseIcon[i].color = new Color(255, 255, 255, 255);
                    WarehouseIcon[i].overrideSprite = Resources.Load("Icon/" + warehouse.items[i].GetName() + "_icon", typeof(Sprite)) as Sprite;
                }

                if (warehouse.itemsNum[i] <= 0)
                {
                    WarehouseNum[i].gameObject.SetActive(false);
                }
                else
                {
                    WarehouseNum[i].gameObject.SetActive(true);
                    WarehouseNum[i].text = System.Convert.ToString(warehouse.itemsNum[i]);
                }
            }
        }
    }
    public void FindShop()
    {
        //shop
        shop = FindObjectOfType<Shop>();
        if (shop != null)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject GoodsIconFrameChild = Shop_Icon.transform.GetChild(0).GetChild(i).gameObject;
                ItemDrag GoodsIconDrag = GoodsIconFrameChild.GetComponent<ItemDrag>();
                GoodsIconDrag.SetRef(shop.goodsItem[i]);
            }
            for (int i = 0; i < 10; i++)
            {
                GameObject ConsumeIconFrameChild = Shop_Icon.transform.GetChild(1).GetChild(i).gameObject;
                consumeIcon[i] = ConsumeIconFrameChild.GetComponentInChildren<Image>();
                ConsumeIconDrag[i] = ConsumeIconFrameChild.GetComponent<ItemDrag>();
                ConsumeIconDrag[i].SetRef(shop.consume[i]);
            }
        }
    }
    public void SetShop()
    {
        if (shop != null)
        {
            for (int i = 0; i < 5; i++)
            {
                if (System.Convert.ToInt32(shop.goodsItem[i].GetID()) <= 0 || shop.goodsItem[i].GetCurrentQuantity() <= 0)
                {
                    goodsIcon[i].color = new Color(255, 255, 255, 0);
                }
                else
                {
                    goodsIcon[i].color = new Color(255, 255, 255, 255);
                    goodsIcon[i].overrideSprite = Resources.Load("Icon/" + shop.goodsItem[i].GetName() + "_icon", typeof(Sprite)) as Sprite;
                }

                if (shop.goodsItemNum[i] <= 0)
                {
                    goodsNum[i].gameObject.SetActive(false);
                }
                else
                {
                    goodsNum[i].gameObject.SetActive(true);
                    goodsNum[i].text = System.Convert.ToString(shop.goodsItemNum[i]);
                }
                if (shop.goodsItemCost[i] <= 0)
                {
                    goodsCost[i].gameObject.SetActive(false);
                }
                else
                {
                    goodsCost[i].gameObject.SetActive(true);
                    goodsCost[i].text = System.Convert.ToString(shop.goodsItemCost[i]);
                }
            }
            for (int i = 0; i < 10; i++)
            {
                if (System.Convert.ToInt32(shop.consume[i].GetID()) <= 0)
                {
                    consumeIcon[i].color = new Color(255, 255, 255, 0);
                }
                else
                {
                    consumeIcon[i].color = new Color(255, 255, 255, 255);
                    consumeIcon[i].overrideSprite = Resources.Load("Icon/" + shop.consume[i].GetName() + "_icon", typeof(Sprite)) as Sprite;
                }

                if (shop.consumeNum[i] <= 0)
                {
                    consumeNum[i].gameObject.SetActive(false);
                }
                else
                {
                    consumeNum[i].gameObject.SetActive(true);
                    consumeNum[i].text = System.Convert.ToString(shop.consumeNum[i]);
                }
            }

            allCost.gameObject.SetActive(true);
            allCost.text = System.Convert.ToString(shop.allCost);

        }
    }

    public void FindMinimap()
    {
        _miniMapCanvas = GameObject.Find("Minimap Canvas");
        if (_miniMapCanvas != null)
        {
            _miniMap = _miniMapCanvas.transform.GetChild(0).gameObject;
            _tileMap = _miniMapCanvas.transform.GetChild(1).gameObject;
            _tileMap.SetActive(false);
        }
    }


    public GameObject saveCanvasPrefab;
    private GameObject _saveCanvas;
    private Button _saveButton;
    public GameObject returnCanvasPrefab;
    private GameObject _returnCanvas;
    private Button _returnButton;
    private void SetMenu()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Application.loadedLevelName=="Home")
            {
                if (_saveCanvas == null)
                {
                    _saveCanvas = Instantiate(saveCanvasPrefab);
                    _saveButton = _saveCanvas.transform.Find("Save").GetComponent<Button>();
                    _saveButton.onClick.AddListener(delegate ()
                    {
                        MainGameManager.Instance.SaveGame();
                    });
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Destroy(_saveCanvas);
                    Cursor.lockState = CursorLockMode.Locked;//锁定鼠标
                    Cursor.visible = false;
                }
            }
            else
            {
                if (_returnCanvas == null)
                {
                    _returnCanvas = Instantiate(returnCanvasPrefab);
                    _returnButton = _returnCanvas.transform.Find("Return").GetComponent<Button>();
                    _returnButton.onClick.AddListener(delegate ()
                    {
                        MainGameManager.Instance.ReturnHome();
                    });
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Destroy(_returnCanvas);
                    Cursor.lockState = CursorLockMode.Locked;//锁定鼠标
                    Cursor.visible = false;
                }
            }
        }
    }
}
