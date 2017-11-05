using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public GameObject DamageTriggerPrefab;
    private GameObject _damageTrigger;
    public GameObject me;
    private GameObject dropThing;
    public GameObject[] dropThingPrefab;
    public GameObject obstacle;
    public GameObject bullet;

    public Rigidbody rig;
    private Enemy _enemy;
    private Animator _ani;

    private float backUptime;
    public float RotateSpeed = 10;
    public float MoveSpeed = 2;
    public float FireSpeed = 1.0f;
    public float FireTime = 0.2f;
    public float distanceToMe;
    public float distanceToObstacle;
    public float alarmedRange = 30;
    public float attackRange = 1.5f;
    public float forceToAvoid = 50.0f;
    public float minDistanceToAvoid = 10.0f;

    public bool isDroped = false;
    public bool canFly = true;
    private bool isSeeking;//判断是否在追寻，用于动画
    private bool isAttacking;//判断是否在攻击，用于动画
    private bool isDied = false;

    public const int AI_THINK_TIME = 1;
    public int fireCoolTime;
    public int rotateCount = 1;
    public int EnemyType;

    public enum EnemyState
    {
        Idle,
        Seek,
        Attack,
        RunAway,
        Dead,
        Stop,
        Avoid

    }
    public EnemyState Enemystate = EnemyState.Idle;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        _enemy = GetComponent<Enemy>();
        _ani = GetComponent<Animator>();

        //EnemyType = 1;
        me = GameObject.FindWithTag("Player");
        obstacle = GameObject.FindWithTag("Obstacle");

        if (EnemyType == 1)
        {
            attackRange = 1.5f;
        }
        if (EnemyType == 2)
        {
            attackRange = 10;
        }

        Enemystate = EnemyState.Idle;
    }

    void Update()
    {
        if (!isDied)
        {
            //if (_enemy.HP <= 0)
            //{
            //    isDied = true;
            //    isSeeking = false;
            //    isAttacking = false;
            //    Enemystate = EnemyState.Stop;
            //    _ani.SetBool("Died", isDied);
            //}
            //设置漫游方向
            if (Time.time - backUptime >= AI_THINK_TIME)
            {
                backUptime = Time.time;
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    Enemystate = 0;
                }
                else if (rand == 1)
                {
                    Quaternion rotate = Quaternion.Euler(0, Random.Range(1, 5) * 90, 0);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 1000);
                    Enemystate = EnemyState.Stop;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!canFly)
        {
            rig.AddForce(0, -500, 0);
        }
        else
        {
            rig.AddForce(0, 0, 0);
        }

        if (!isDied)
        {
            //   Mathf.Clamp(transform.position.y, 0, 1.5f);

            distanceToMe = Vector3.Distance(me.transform.position, this.transform.position);
            distanceToObstacle = Vector3.Distance(obstacle.transform.position, this.transform.position);

            if (distanceToObstacle < minDistanceToAvoid) Enemystate = EnemyState.Avoid;
            else
            {
                if (distanceToMe > alarmedRange) Enemystate = EnemyState.Idle;
                if (distanceToMe < alarmedRange && distanceToMe > attackRange && isAttacking == false) Enemystate = EnemyState.Seek;
                if (distanceToMe < attackRange) Enemystate = EnemyState.Attack;
            }
        }
        if (_enemy.HP <= 0)
        {
            Enemystate = EnemyState.Dead;
        }

        switch (Enemystate)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Seek:
                Seek();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Avoid:
                AvoidObstacles();
                break;
            case EnemyState.Stop:
                break;
            case EnemyState.Dead:
                Dead();
                break;
        }
        //else this.transform.Rotate(-1.8f, 0, 0);

        SetAnimation();
    }

    void Dead()
    {
        if (!isDroped)
        {
            int rand1 = Random.Range(0, 3);
            int randCoin = Random.Range(1, 3);
            if (rand1 < 1)
            {
                //只掉落随机金额金币
                dropThing = Instantiate(Resources.Load("Item/coin")) as GameObject;
                dropThing.transform.position = this.transform.position;

                dropThing.GetComponent<Coin>().num = randCoin;
            }
            for (int i = 0; i < dropThingPrefab.Length; i++)
            {
                int rand2 = Random.Range(0, 5);
                if (rand2 < 1)
                {
                    //掉落装备 / 道具

                    dropThing = Instantiate(dropThingPrefab[i]) as GameObject;
                    dropThing.transform.position = this.transform.position + new Vector3(-0.5f, 0, -0.5f);
                    dropThing.transform.Rotate(90, 90, 0);
                }
            }
            //StartCoroutine(WaitFireTime(FireTime * 10));
            isDroped = true;
        }
        if (!isDied)
        {
            Globe.kill++;
            isDied = true;
            isSeeking = false;
            isAttacking = false;
            Enemystate = EnemyState.Stop;
            _ani.SetBool("Died", isDied);

        }
    }

    void Idle()
    {
        transform.Translate(Vector3.forward * Time.deltaTime);
    }

    void Seek()
    {
        if (!canFly)
        {
            float x = me.transform.position.x;
            float y = this.transform.position.y;
            float z = me.transform.position.z;

            this.transform.LookAt(new Vector3(x, y, z));
        }
        else
        {
            this.transform.LookAt(me.transform.position);
        }

        //  this.transform.Rotate(-x, 0, -z);
        this.transform.Translate(Vector3.forward * MoveSpeed * 0.5f);
        isSeeking = true;
    }

    void Attack()
    {
        this.transform.LookAt(me.transform);
        if (EnemyType == 1)
        {
            if (fireCoolTime > 50 / FireSpeed)
            {
                if (_damageTrigger == null)
                {
                    isAttacking = true;

                    fireCoolTime = 0;

                    Enemystate = EnemyState.Stop;//攻击一次使怪物不动，而不继续追寻连续攻击

                }
            }
            else fireCoolTime++;
        }
        else if (EnemyType == 2)
        {
            if (fireCoolTime > 50 / FireSpeed)
            {
                this.transform.LookAt(me.transform);
                isAttacking = true;

                fireCoolTime = 0;

                Enemystate = EnemyState.Stop;//攻击一次使怪物不动，而不继续追寻连续攻击
            }
            else fireCoolTime++;
        }
    }

    IEnumerator WaitFireTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (_damageTrigger != null)
            Destroy(_damageTrigger);
    }

    void AvoidObstacles()
    {
        //功能不齐全，暂时不可用
        transform.Translate(0, 0, MoveSpeed * Time.deltaTime * 20);
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.1f, out hit))
        {
            if (hit.distance < minDistanceToAvoid)
            {
                float angle = Random.Range(-30, 30);
                transform.Rotate(0, angle, 0);
            }

        }
    }

    void SetAnimation()
    {
        //_ani.SetFloat("AbsSpeed", rig.velocity.magnitude);
        _ani.SetBool("Seeking", isSeeking);
        _ani.SetBool("Attacking", isAttacking);

    }

    void SetAttack() { isAttacking = false; }//用于攻击动画，动画结束末尾就不在攻击，等待下一次攻击开始

    void SetDamageTrigger()//用于攻击动画造成伤害的一瞬间，再释放伤害触发器
    {
        if (EnemyType == 1)
        {
            _damageTrigger = Instantiate(DamageTriggerPrefab) as GameObject;
            _damageTrigger.GetComponent<Damage>().SetDamage(_enemy.STR);
            _damageTrigger.GetComponent<Damage>().damageForm = DamageFrom.Enemy;
            _damageTrigger.transform.SetParent(this.transform);
            _damageTrigger.transform.position = transform.forward + transform.position;
            _damageTrigger.transform.localRotation = new Quaternion(0, 0, 0, 0);

            StartCoroutine(WaitFireTime(FireTime));
        }
        else if (EnemyType == 2)
        {
            _damageTrigger = Instantiate(bullet) as GameObject;
            Vector3 startPoint = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z) + transform.forward;
            _damageTrigger.transform.position = startPoint;
            _damageTrigger.transform.Find("DamageTrigger").GetComponent<Damage>().damageForm = DamageFrom.Enemy;

            Bullet bulletTemp = _damageTrigger.GetComponent<Bullet>();
            bulletTemp.SetDamage(bulletTemp.GetDamage() + _enemy.STR);

            
            Ray ray = new Ray(startPoint, (me.transform.position - startPoint).normalized);
            float deviationX = Random.Range(0, bulletTemp.GetDeviation());
            float deviationY = Random.Range(0, bulletTemp.GetDeviation());
            float deviationZ = Random.Range(0, bulletTemp.GetDeviation());

            _damageTrigger.transform.rotation = Quaternion.LookRotation(ray.direction + new Vector3(deviationX, deviationY, deviationZ));//设定方向
            _damageTrigger.GetComponent<Rigidbody>().velocity = _damageTrigger.transform.forward * bullet.GetComponent<Bullet>().GetSpeed();

            //_damageTrigger.transform.Translate(Vector3.forward * FireSpeed);
        }
    }

    void SetSeek() { isSeeking = false; }//用于攻击动画，动画结束末尾就不追寻，等待下一次判断追寻

    void SetDied()
    {
        Destroy(this.gameObject);
    }//死亡动画结束后销毁
}
