using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
    public int HP, MaxHp, STR, DEF, MOS, ATS;
    public float SP, MaxSp;
    // Use this for initialization
    void Start () {
        HP = 100;
	}

    virtual public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Damage")) 
        {
            Damage damage = coll.GetComponent<Damage>();
            if(damage!=null)
            {
                if(this.gameObject.tag=="Player"&& damage.damageForm==DamageFrom.Enemy)
                {
                    TakeDamage(damage.GetDamage());
                    this.transform.Translate(damage.GetVectorForward(), Space.World);
                }
                else if(this.gameObject.layer == LayerMask.NameToLayer("Enemy") && damage.damageForm == DamageFrom.Human)
                {
                    TakeDamage(damage.GetDamage());
                    this.transform.Translate(damage.GetVectorForward(), Space.World);
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        HP -= (10 * damage) / (10 + DEF);
        if(this.tag=="Player")
        {
            Globe.playerDamage += damage;
        }
        else if(this.gameObject.layer==LayerMask.NameToLayer("Enemy"))
        {
            Globe.ennemyDamage += damage;
        }
    }

    public void TakeDebuff(string _medicineType, float _duration)
    {
        switch (_medicineType)
        {
            case "ATSDown":
                StartCoroutine(ATSDown(_duration));
                break;
            case "MOSDown":
                StartCoroutine(MOSDown(_duration));
                break;
            case "DEFDown":
                StartCoroutine(DEFDown(_duration));
                break;
        }
    }


    IEnumerator ATSDown(float _duration)
    {
        ATS /= 2;
        yield return new WaitForSeconds(_duration);
        ATS *= 2;

    }

    IEnumerator MOSDown(float _duration)
    {
        MOS /= 2;
        yield return new WaitForSeconds(_duration);
        MOS *= 2;
    }

    IEnumerator DEFDown(float _duration)
    {
        DEF /= 2;
        yield return new WaitForSeconds(_duration);
        DEF *= 2;
    }

}
