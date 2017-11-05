using UnityEngine;
using System.Collections;

public class DebuffMedicine : Item
{
    Human me;
    //private string _name;
    //[SerializeField]
    //private string _id;
    //[SerializeField]
    //private string _medicineType;
    //[SerializeField]
    //private string _rangeType;  //作用范围，对一个怪物还是一个区域

    private GameObject debuffmedicine;
    private Camera _camera;
    private Rigidbody effectRange;

    //private int _duration;
    //private int _value;
    //private int _maxQuantity;
 
    private float _effectDistance;
    private float _maxEffectDistance;

    //DebuffMedicine() { }

    //DebuffMedicine(string id)//构造函数
    //{
    //    debuffmedicine = GameObject.FindGameObjectWithTag("DebuffMedicine");
    //    me = GameObject.Find("Player").GetComponent<Human>();
        
    //    _id = id;
    //    _name = GetName(_id);
    //    _duration = GetDuration(_id);
    //    _value = GetVaule(_id);
    //    _maxQuantity = GetMaxQuantity(_id);
    //    _rangeType = GetRangeType(_id);
    //}
    public override void Use()
    {
        switch (GetRangeType())//先判断类型，单还是多
        {
            case "single":
                SingleRange();
                break;
            case "multi":
                MultiRange();
                break;
            default: Debug.Log("no type"); break;
        }
    }

    void SingleRange()//单体debuff道具：只可对命中目标debuff
    {
        _effectDistance = Vector3.Distance(me.transform.position, effectRange.transform.position);
        effectRange = Instantiate(effectRange) as Rigidbody;//创建一个道具
        ThrowDebuffMedicine();//投掷出道具
        if (_effectDistance > _maxEffectDistance) StartCoroutine(DestoryItem(effectRange));//到达最大距离
        
    }

    void MultiRange()//群体debuff道具，在范围内的都debuff
    {
        _effectDistance = Vector3.Distance(me.transform.position, effectRange.transform.position);
        effectRange = Instantiate(effectRange) as Rigidbody;

        ThrowDebuffMedicine();

        if (_effectDistance > 30)
        {
            Debug.Log("explore!");
            effectRange.transform.localScale = new Vector3(10, 10, 10);//到一定距离后扩大作用范围
            StartCoroutine(DestoryItem(effectRange));
        }
    }

    void ThrowDebuffMedicine()//投掷道具，和开枪类似，朝着准星投
     {
       Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
       Ray ray = _camera.ScreenPointToRay(point);

       effectRange.transform.rotation = Quaternion.LookRotation(ray.direction + new Vector3(0, 0.2f, 0));
       effectRange.GetComponent<Rigidbody>().velocity = effectRange.transform.forward*10.0f;
    }

    void OnTriggerEnter(Collider enemy)//检测是否有怪物进入碰撞体
    {
        if (enemy.GetComponent<Enemy>())
        {
            Debug.Log("hit enemy");
            enemy.GetComponent<Enemy>().TakeDebuff(GetMedicineType(), GetDuration());
            StartCoroutine(DestoryItem(effectRange));
        }
        else
            StartCoroutine(DestoryItem(effectRange));
    }

    IEnumerator DestoryItem(Rigidbody effectRange)//延时销毁掉道具
    {
        yield return new WaitForSeconds(2.0f);
        Debug.Log("destorying..");
        Destroy(effectRange);
    }
    
}
