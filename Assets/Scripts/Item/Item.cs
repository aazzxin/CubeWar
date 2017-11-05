using UnityEngine;
using System.Collections;

public enum ItemType { Item ,Weapon, RedMedicine, BlueMedicine, BuffMedicine, DebuffMedicine }
public class Item : MonoBehaviour {
    [SerializeField]
    private int _vaule, _currentQuantity, _maxQuantity, _damage;//分别是道具价值,当期道具数量，一格最大数目,伤害,CD
    [SerializeField]
    private float _cd;
    [SerializeField]
    private int _recovery, _duration;
    private string _rangeType, _medicineType;  //作用范围，对一个怪物还是一个区域
    [SerializeField]
    private string _name, _id;

    
    public int GetVaule() { return _vaule; }
    public int GetCurrentQuantity() { return _currentQuantity; }
    public void SetCurrentQuantity(int quantity) { _currentQuantity = quantity; }
    public int GetMaxQuantity() { return _maxQuantity; }
    public int GetDamage() { return _damage; }
    public float GetCD() { return _cd; }

    public int GetRecovery(){ return _recovery; }
    public int GetDuration() { return _duration; }
    public string GetRangeType(){ return _rangeType; }
    public string GetMedicineType(){ return _medicineType; }

    public string GetName() { return _name; }

    public Item() { }
    public Item(string id)
    {
        _id = id;
    }
    public void SetItem(Item item)
    {
        _id = item._id;
        _name = item.GetName();

        _vaule = item.GetVaule();
        _currentQuantity = item.GetCurrentQuantity();
        _maxQuantity = item.GetMaxQuantity();
        _damage = item.GetDamage();
        _cd = item.GetCD();

        _recovery = item.GetRecovery();
        _duration = item.GetDuration();
        _rangeType = item.GetRangeType();
        _medicineType = item.GetMedicineType();

        _itemType = item.GetItemType();
    }
    public void SetItem(string id)
    {
        SetID(id);
        GetName(id);

        GetVaule(id);
        GetMaxQuantity(id);
        GetDamage(id);
        GetCD(id);

        GetRecovery(id);
        GetDuration(id);
        GetRangeType(id);
        GetMedicineType(id);

        SetItemType();
    }
    public string GetID() { return _id; }
    public void SetID(string id) { _id = id; }
    public string GetName(string _id)
    {
        _name = PlayerPrefs.GetString(_id + "name", "None");
        return _name;
    }
    public int GetVaule(string _id)
    {
        _vaule = PlayerPrefs.GetInt(_id + "value", 0);
        return _vaule;
    }
    public int GetMaxQuantity(string _id)
    {
        _maxQuantity = PlayerPrefs.GetInt(_id + "maxquantity", 0);
        return _maxQuantity;
    }
    public int GetDuration(string _id)
    {
        _duration = PlayerPrefs.GetInt(_id + "duration", 0);
        return _duration;
    }
    public int GetDamage(string _id)
    {
        _damage = PlayerPrefs.GetInt(_id + "damage", 0);
        return _damage;
    }
    public float GetCD(string _id)
    {
        _cd = PlayerPrefs.GetFloat(_id + "cd", 0);
        return _cd;
    }
    public int GetRecovery(string _id)
    {
        _recovery = PlayerPrefs.GetInt(_id + "recovery", 0);
        return _recovery;
    }
    public string GetRangeType(string _id)
    {
        _rangeType = PlayerPrefs.GetString(_id + "rangetype", "None");
        return _rangeType;
    }
    public string GetMedicineType(string _id)
    {
        _medicineType = PlayerPrefs.GetString(_id + "medicinetype", "None");
        return _medicineType;
    }

    [SerializeField]
    private ItemType _itemType = ItemType.Item;
    public ItemType GetItemType()
    {
        return _itemType;
    }
    public void SetItemType()
    {
        if(System.Convert.ToInt32(_id) >0)
        {
            if (System.Convert.ToInt32(_id) <= 40)//武器
            {
                _itemType = ItemType.Weapon;
            }
            else if (System.Convert.ToInt32(_id) <= 50)//红药
            {
                _itemType = ItemType.RedMedicine;
            }
            else if (System.Convert.ToInt32(_id) <= 60)//蓝药
            {
                _itemType = ItemType.BlueMedicine;
            }
            else if (System.Convert.ToInt32(_id) <= 90)//Buff
            {
                _itemType = ItemType.BuffMedicine;
            }
            else if (System.Convert.ToInt32(_id) <= 120)//Debuff
            {
                _itemType = ItemType.DebuffMedicine;
            }
        }
        else
            _itemType = ItemType.Item;
    }
    void Awake()
    {
        PlayerPrefs.SetString(_id + "name", _name);
        PlayerPrefs.SetInt(_id + "value", _vaule);
        PlayerPrefs.SetInt(_id + "maxquantity", _maxQuantity);
        PlayerPrefs.SetInt(_id + "duration", _duration);
        PlayerPrefs.SetInt(_id + "damage", _damage);
        PlayerPrefs.SetFloat(_id + "cd", _cd);
        PlayerPrefs.SetInt(_id + "recovery", _recovery);
        PlayerPrefs.SetString(_id + "rangetype", _rangeType);
        PlayerPrefs.SetString(_id + "medicinetype", _medicineType);

        SetItemType();
    }
    virtual public void Use()
    {

    }
}
