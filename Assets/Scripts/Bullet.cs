using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    [SerializeField]
    private int _damage, _speed;
    [SerializeField]
    private float _stayTime;
    [SerializeField]
    private float _deviation;//子弹的偏移属性
    [SerializeField]
    private int _countsTimes;//一次发射的子弹数
    public int GetDamage() { return _damage; }
    public void SetDamage(int damage) { _damage = damage; }
    public int GetSpeed() { return _speed; }
    public float GetStayTime() { return _stayTime; }
    public float GetDeviation() { return _deviation; }
    public int GetCountsTimes()
    {
        return _countsTimes;
    }

    private Damage damageTrigger;
	// Use this for initialization
	void Start () {
        damageTrigger = transform.Find("DamageTrigger").gameObject.GetComponent<Damage>();
        if (damageTrigger != null)
            damageTrigger.SetDamage(_damage);
        StartCoroutine(DestroyIt());
	}
	
    IEnumerator DestroyIt()
    {
        yield return new WaitForSeconds(_stayTime);
        if (this.gameObject != null)
            Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player" && damageTrigger.damageForm == DamageFrom.Human)//人物的子弹碰到人物不会消失
        { }
        else if (coll.gameObject.layer == LayerMask.NameToLayer("Enemy") && damageTrigger.damageForm == DamageFrom.Enemy)//怪物子弹碰到怪物不会消失
        { }
        else if (coll.gameObject.tag == "Bullet")
        { }
        else
            Destroy(this.gameObject);
    }
}
