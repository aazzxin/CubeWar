using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour,IGameManager {
    #region IGmaeManager
    public ManagerStatus status { get; private set; }

    public void Startup()
    {
        status = ManagerStatus.Started;

    }

    public void Reset()
    {

    }
    #endregion

    public ItemDrag ItemTemp1Drag;
    public Item ItemTemp1;
    public ItemDrag ItemTemp2Drag;
    public Item ItemTemp2;//分别是交换道具的前者和后者
    public Item ItemTemp;//用于Temp1和Temp2转换的容器

    public Item ItemPrefab;

    Package package;
    //Warehouse warehouse;
    Shop shop;
    void Awake()
    {
        ItemTemp = Instantiate(ItemPrefab);
        ItemTemp.transform.SetParent(this.gameObject.transform);
        
        //warehouse = FindObjectOfType<Warehouse>();
        shop = FindObjectOfType<Shop>();
    }

    // Use this for initialization
    void Start () {
        package = MainGameManager.playerManager.player.GetComponent<Package>();
    }
	

    public void Swap()
    {
        if (ItemTemp2 != null && ItemTemp1 != null && ItemTemp1.GetItemType() != ItemType.Item) 
        {
            if(ItemTemp1== package.weapon[0])
            {
                if (!(System.Convert.ToInt32(ItemTemp2.GetID()) > 20 && System.Convert.ToInt32(ItemTemp2.GetID()) <= 40) && ItemTemp2.GetItemType() != ItemType.Item)
                {
                    return;
                }
            }
            else if (ItemTemp1 == package.weapon[1])//如果是近程武器
            {
                if (!(System.Convert.ToInt32(ItemTemp2.GetID()) > 0 && System.Convert.ToInt32(ItemTemp2.GetID()) <= 20) && ItemTemp2.GetItemType() != ItemType.Item)
                {
                    return;
                }
            }
            if (ItemTemp2==package.weapon[0])//如果替换到的目标是远程武器
            {
                if (!(System.Convert.ToInt32(ItemTemp1.GetID()) > 20 && System.Convert.ToInt32(ItemTemp1.GetID()) <= 40))
                {
                    return;
                }
            }
            else if(ItemTemp2 == package.weapon[1])//如果替换到的目标是近程武器
            {
                if (!(System.Convert.ToInt32(ItemTemp1.GetID()) > 0 && System.Convert.ToInt32(ItemTemp1.GetID()) <= 20))
                {
                    return;
                }
            }
            SwapClear();
        }
        ItemTemp1 = null;ItemTemp2 = null;
        if (ItemTemp1Drag != null && ItemTemp2Drag != null)
        {
            ItemTemp1Drag = null; ItemTemp2Drag = null;
        }
    }
    private void SwapClear()
    {
        ItemTemp.SetItem(ItemTemp1);
        int dragTypeTemp = -1;
        if (ItemTemp1Drag != null)
        {
            dragTypeTemp = ItemTemp1Drag.DragType;
        }

        for (int i = 0; i < 4; i++)
        {
            if (ItemTemp1 == package.quickItem[i])
            {
                package.SetQuickItemType(i, ItemTemp2.GetItemType());
                MainGameManager.uiManager.SetQuickItemIconRef(i);
                ItemTemp1 = package.quickItem[i];
            }
        }
        ItemTemp1.SetItem(ItemTemp2);
        if (MainGameManager.uiManager.shop != null)
        {
            shop = MainGameManager.uiManager.shop;
            for (int i = 0; i < 10; i++)
            {
                if (ItemTemp1 == shop.consume[i])
                {
                    if (ItemTemp1.GetCurrentQuantity() <= 0)
                    {
                        ItemTemp1Drag.DragType = 3;
                        shop.buyOrSell[i] = 0;
                    }
                    else if (ItemTemp1Drag.DragType == 1)
                    {
                        shop.buyOrSell[i] = (ItemTemp1.GetVaule() * ItemTemp1.GetCurrentQuantity()) / 2;
                    }
                    else if (ItemTemp1Drag.DragType == 2)
                    {
                        shop.buyOrSell[i] = -1 * ItemTemp1.GetVaule() * ItemTemp1.GetCurrentQuantity();
                    }
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (ItemTemp2 == package.quickItem[i])
            {
                package.SetQuickItemType(i, ItemTemp.GetItemType());
                MainGameManager.uiManager.SetQuickItemIconRef(i);
                ItemTemp2 = package.quickItem[i];
            }
        }
        ItemTemp2.SetItem(ItemTemp);
        if (MainGameManager.uiManager.shop != null)
        {
            shop = MainGameManager.uiManager.shop;
            for (int i = 0; i < 10; i++)
            {
                if (ItemTemp2 == shop.consume[i])
                {
                    if (dragTypeTemp != -1)
                        ItemTemp2Drag.DragType = dragTypeTemp;//若dragTypeTemp受到赋值，不为-1，则ItemTemp2Drag的类型为dragTypeTemp
                    if (ItemTemp2Drag.DragType == 1)
                        shop.buyOrSell[i] = (ItemTemp2.GetVaule() * ItemTemp2.GetCurrentQuantity()) / 2;
                    else if (ItemTemp2Drag.DragType == 2)
                        shop.buyOrSell[i] = -1 * ItemTemp2.GetVaule() * ItemTemp2.GetCurrentQuantity();
                }
            }
        }


        package.SetItemNum();
        if (MainGameManager.uiManager.warehouse != null)
        {
            MainGameManager.uiManager.warehouse.SetItemNum();
        }
        if (MainGameManager.uiManager.shop != null)
        {
            MainGameManager.uiManager.shop.SetItemNum();
            MainGameManager.uiManager.shop.SetAllCost();
        }

        MainGameManager.uiManager.SetBag();
        MainGameManager.uiManager.SetQuickItem();
        MainGameManager.uiManager.SetWeapon();
        MainGameManager.uiManager.SetWarehouse();
        MainGameManager.uiManager.SetShop();

        if (ItemTemp2 == package.weapon[0] || ItemTemp2 == package.weapon[1] || ItemTemp1 == package.weapon[0] || ItemTemp1 == package.weapon[1])
        {
            package.SetWeapon();
        }
    }

    public void Buy()
    {
        if(MainGameManager.uiManager.shop!=null)
        {
            MainGameManager.uiManager.shop.Enter();
        }
    }
}

