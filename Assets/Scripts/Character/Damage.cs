using UnityEngine;
using System.Collections;

public enum DamageFrom { Human, Enemy }

public class Damage : MonoBehaviour {
    [SerializeField]
    private int _damage;
    public int GetDamage (){ return _damage; }
    public void SetDamage(int damage) { _damage = damage; }
    public void PulsDamage(int damage){ _damage += damage; }

    public DamageFrom damageForm = DamageFrom.Enemy;//默认为怪物攻击
	// Use this for initialization

    public Vector3 GetVectorForward() { return transform.forward; }
}
