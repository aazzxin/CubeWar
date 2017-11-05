using UnityEngine;
using System.Collections;

public class Boss : Character
{
    private Animator _HPAni;

    // Use this for initialization
    void Start()
    {
        GameObject hpImagePrefab= Resources.Load("BossHP") as GameObject;
        GameObject hp = Instantiate(hpImagePrefab);
        _HPAni = hp.transform.Find("BossHP").Find("HP").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _HPAni.SetFloat("Length", System.Convert.ToSingle(HP) / MaxHp);
    }
    void FixedUpdata()
    {
        if (this.GetComponent<Boss>().SP < this.GetComponent<Boss>().MaxSp)
        {
            this.GetComponent<Boss>().SP++;
        }
    }

    public override void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Damage"))
        {
            Damage damage = coll.GetComponent<Damage>();
            if (damage != null)
            {
                if (this.gameObject.tag == "Player" && damage.damageForm == DamageFrom.Enemy)
                {
                    TakeDamage(damage.GetDamage());
                }
                else if (this.gameObject.layer == LayerMask.NameToLayer("Enemy") && damage.damageForm == DamageFrom.Human)
                {
                    TakeDamage(damage.GetDamage());
                }
            }
        }
    }
}
