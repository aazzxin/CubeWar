using UnityEngine;
using System.Collections;

public class Weapon : Item {

    //[SerializeField]
    //private int _damage;
    //private int _value;

    private Package package;
    //private string _name;
    //private string _id;

    //public Weapon() { }
    //public Weapon(string id)
    //{
    //    _id = id;
    //    _name = GetName(id);
    //    _damage = GetDamage(id);
    //    _value = GetVaule(id);
    //}

    public override void Use()
    {
        int id = int.Parse(GetID());
        package = MainGameManager.playerManager.package;
        if (id > 0 && id <= 20)//近程武器
        {
            ChangeWeapon(1);
        }
        else if (id > 20 && id <= 40)//远程武器
        {
            ChangeWeapon(0);
        }
    }

    void ChangeWeapon(int i)
    {
        //string idtmp = package.weapon[i]._id;
        //string nametmp = package.weapon[i].name;
        //int valuetmp = package.weapon[i]._value;
        //int damagetmp = package.weapon[i]._damage;

        //package.weapon[i]._id = _id;
        //package.weapon[i]._damage = _damage;
        //package.weapon[i]._name = _name;
        //package.weapon[i]._value = _value;

        //_id = idtmp;
        //_value = valuetmp;
        //_damage = damagetmp;
        //_name = nametmp;
        MainGameManager.itemManager.ItemTemp1 = this;
        MainGameManager.itemManager.ItemTemp2 = package.weapon[i];
        MainGameManager.itemManager.Swap();
        //package.SetWeapon();
    }
}
