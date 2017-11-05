using UnityEngine;
using System.Collections;

public class BuffMedicine : Item
{
    Human me;
    
    //private string _name;
    //private string _id;

    //private int _duration;
    //private int _value;
    //private int _maxQuantity;
    //private string _medicineType;
    //BuffMedicine() { }
    //BuffMedicine(string id)
    //{
    //    me = GetComponent<Human>();
    //    _id = id;
    //    _name = GetName(_id);
    //    _duration = GetDuration(_id);
    //    _value = GetVaule(_id);
    //    _maxQuantity = GetMaxQuantity(_id);
    //}
  public override void Use()
    {
        switch (GetMedicineType())
        {
            case "ATSUP":
                StartCoroutine(ATSUP());
                break;
            case "MOSUP":
                StartCoroutine(MOSUP());
                break;
            case "DEFUP":
                StartCoroutine(DEFUP());
                break;
            case "SPUP":
                StartCoroutine(SPUP());
                break;
        }
    }

    IEnumerator ATSUP()
    {
        me.ATS *= 2;
        yield return new WaitForSeconds(GetDuration());
        me.ATS /= 2;

    }

    IEnumerator MOSUP()
    {
        me.MOS *= 2;
        yield return new WaitForSeconds(GetDuration());
        me.MOS /= 2;

    }
    IEnumerator DEFUP()
    {
        me.DEF *= 2;
        yield return new WaitForSeconds(GetDuration());
        me.DEF /= 2;
    }

    IEnumerator SPUP()
    {
        me.MaxSp *= 2;
        yield return new WaitForSeconds(GetDuration());
        me.MaxSp /= 2;
    }
}
