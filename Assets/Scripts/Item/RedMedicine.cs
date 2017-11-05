using UnityEngine;
using System.Collections;

public class RedMedicine : Item
{

    //private string _name;
    //private string _id;

    //private int _recovery;
    //private int _value;
    //private int _maxQuantity;

    //RedMedicine()
    //{ }
    //RedMedicine(string id)
    //{
    //    me = GetComponent<Human>();
    //    _id = id;
    //    _name = GetName(_id);
    //    _recovery = GetRecovery(_id);
    //    _value = GetVaule(_id);
    //    _maxQuantity = GetMaxQuantity(_id);

    //}
    
    private GameObject _effect;
   public override void Use()
    {
        Human me = MainGameManager.playerManager.human;
        if (me.HP > 0 && me.HP < me.MaxHp)
        {
            int HPTemp = me.HP + GetRecovery();
            if (HPTemp > me.MaxHp)
            {
                me.HP = me.MaxHp;
            }
            else
            {
                me.HP = HPTemp;
            }
            GameObject _effectPrefab=Resources.Load("Effect/" + "RedMedicine") as GameObject;
            _effect = Instantiate(_effectPrefab) as GameObject;
            _effect.transform.SetParent(me.gameObject.transform);
            _effect.transform.localPosition = new Vector3(0, 0, 0);

            this.SetCurrentQuantity(this.GetCurrentQuantity() - 1);//数目-1
            MainGameManager.playerManager.package.SetItemNum();//设置背包道具数目
            MainGameManager.uiManager.SetQuickItem();
        }
    }
}