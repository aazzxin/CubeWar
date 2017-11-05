using UnityEngine;
using System.Collections;

public class BossEnemy : MonoBehaviour
{
    public GameObject DamageTriggerPrefab;
    private GameObject _damageTrigger;
    public GameObject me;
    //public GameObject obstacle;
    public GameObject dropThing;
    public GameObject[] dropThingPrefab;

    public Rigidbody rig;
    private Boss _enemy;
    private Animator _ani;
    public GameObject bullet;
    public Vector3 mHomePosition;

    private float backUptime;
    public float RotateSpeed = 10;
    public float MoveSpeed = 2;
    public float FireSpeed = 1.0f;
    public float FireTime = 0.2f;
    public float distanceToMe;
    //public float distanceToObstacle;
    public float alarmedRange = 30;
    public float attackRange = 1.5f;
    public float forceToAvoid = 50.0f;
    public float minDistanceToAvoid = 10.0f;
    public float SkillOneCoolTime_oringal = 3;
    public float SkillTwoCoolTime_oringal = 6;
    public float SkillOneCoolTime;
    public float SkillTwoCoolTime;
    public float SkillOneLastTime = 1.0f;
    public float SkillTwoLastTime = 3.0f;
    public float distanceToHome;
    public float dangerHP = 50;

    public bool canFly = true;
    private bool isSeeking;//判断是否在追寻，用于动画
    private bool isAttacking = false;//判断是否在攻击，用于动画
    private bool isSkillOne;
    private bool isSkillTwo;
    public bool isDroped = false;
    private bool isDied = false;

    public float homeRange = 15.0f;
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
        Respawn,
        Avoid,
        Homing,
        Dead,
        Stop,
        SkillOne,
        SkillTwo
    }

    public EnemyState Enemystate = EnemyState.Idle;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        _enemy = GetComponent<Boss>();
        _ani = GetComponent<Animator>();
        distanceToHome = Vector3.Distance(mHomePosition, transform.position);

        EnemyType = 1;
        me = GameObject.FindWithTag("Player");
        //obstacle = GameObject.FindWithTag("Obstacle");

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
            if (Enemystate == EnemyState.Idle || Enemystate == EnemyState.Stop)
            {
                //设置漫游方向
                if (Time.time - backUptime >= AI_THINK_TIME)
                {
                    backUptime = Time.time;
                    int rand = Random.Range(0, 2);
                    if (rand == 0)
                    {
                        Enemystate = EnemyState.Idle;
                    }
                    else if (rand == 1)
                    {
                        Quaternion rotate = Quaternion.Euler(0, Random.Range(1, 5) * 90, 0);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 1000);
                        Enemystate = EnemyState.Stop;
                    }
                }
                if (_enemy.HP < _enemy.MaxHp)
                {
                    StartCoroutine(RecoveryHP());
                }
            }
            if (SkillOneCoolTime >= 0 && this.GetComponent<Boss>().SP > this.GetComponent<Boss>().MaxSp * 0.2f)
            {
                SkillOneCoolTime -= Time.deltaTime;
                if (SkillOneCoolTime < 0)
                    SkillOneCoolTime = 0;
            }
            if (SkillTwoCoolTime >= 0 && this.GetComponent<Boss>().SP > this.GetComponent<Boss>().MaxSp * 0.5f)
            {
                SkillTwoCoolTime -= Time.deltaTime;
                if (SkillTwoCoolTime < 0)
                    SkillTwoCoolTime = 0;
            }
        }
    }

    IEnumerator RecoveryHP()
    {
        //躲在家里可以回复生命
        yield return new WaitForSeconds(10.0f);
        _enemy.HP += 1;

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
            //distanceToObstacle = Vector3.Distance(obstacle.transform.position, this.transform.position);
            distanceToHome = Vector3.Distance(mHomePosition, transform.position);

            //if (distanceToObstacle < minDistanceToAvoid) Enemystate = EnemyState.Avoid;
            //else
            //{
            if (distanceToMe > alarmedRange) Enemystate = EnemyState.Idle;
            if (distanceToMe < alarmedRange && distanceToMe > attackRange && isAttacking == false) Enemystate = EnemyState.Seek;
            if ((distanceToMe < attackRange) && SkillOneCoolTime == 0)
            {
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                }
                else if (rand == 1)
                {
                    Enemystate = EnemyState.SkillOne;
                }
            }
            else if ((distanceToMe < attackRange) && SkillTwoCoolTime == 0)
            {
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                }
                else if (rand == 1)
                {
                    Enemystate = EnemyState.SkillTwo;
                }
            }
            else if (distanceToMe < attackRange) Enemystate = EnemyState.Attack;
        }
        if (distanceToHome > homeRange) Enemystate = EnemyState.Homing;
        if (_enemy.HP < dangerHP)
        {
            Enemystate = EnemyState.RunAway;
            if (distanceToMe > alarmedRange)
            {
                Enemystate = EnemyState.Idle;
            }
        }


        //}

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
            case EnemyState.Respawn:
                Respawn();
                break;
            case EnemyState.RunAway:
                RunAway();
                break;
            case EnemyState.Homing:
                Homing();
                break;
            case EnemyState.Dead:
                Dead();
                break;
            case EnemyState.SkillOne:
                StartCoroutine(UseSkillOne(SkillOneLastTime));
                break;
            case EnemyState.SkillTwo:
                StartCoroutine(UseSkillTwo(SkillTwoLastTime));
                break;
            case EnemyState.Stop:
                break;
        }
        //else this.transform.Rotate(-1.8f, 0, 0);

        SetAnimation();
    }

    IEnumerator UseSkillOne(float last_time)
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

        if (!isSkillOne)
        {
            this.transform.LookAt(me.transform);
            _enemy.STR += 10;
            isSkillOne = true;
            _enemy.SP *= 0.5f;//消耗50%sp

            yield return new WaitForSeconds(last_time);
            //技能1，下劈，播放动画，斧头附加damagetrigger，玩家在下劈地点附近会击退
            SkillOneCoolTime = SkillOneCoolTime_oringal;
            isSkillOne = false;
            _enemy.STR -= 10;
            if (_damageTrigger != null)
                Destroy(_damageTrigger);
        }

        yield return null;
    } //

    IEnumerator UseSkillTwo(float last_time)
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

        if (!isSkillTwo)
        {
            this.transform.LookAt(me.transform);
            //旋风斩，怪物开始抡起斧头旋转，同时速度减慢，朝玩家移动，斧头附加damagetrigger，会击退玩家
            _enemy.DEF += 50;
            _enemy.STR -= 10;
            _enemy.SP *= 0.2f;//消耗80%sp
            isSkillTwo = true;

            yield return new WaitForSeconds(last_time);
            SkillTwoCoolTime = SkillTwoCoolTime_oringal;
            isSkillTwo = false;
            _enemy.DEF -= 50;
            _enemy.STR += 10;
            if (_damageTrigger != null)
                Destroy(_damageTrigger);
        }

        yield return null;
    }

    void Homing()
    {
        //追击玩家或漫游超出据点范围时会回到范围内
        if (distanceToHome > homeRange && distanceToMe > alarmedRange)
        {
            if (!canFly)
            {
                float x = mHomePosition.x;
                float y = this.transform.position.y;
                float z = mHomePosition.z;

                this.transform.LookAt(new Vector3(x, y, z));
            }
            else
            {
                this.transform.LookAt(mHomePosition);
            }

            //  this.transform.Rotate(-x, 0, -z);
            this.transform.Translate(Vector3.forward * MoveSpeed * 0.5f);
            isSeeking = true;
        }
        else if (distanceToHome > homeRange && distanceToMe < alarmedRange)
        {
            while (distanceToMe > alarmedRange)
            {
                if (!canFly)
                {
                    float x = mHomePosition.x;
                    float y = this.transform.position.y;
                    float z = mHomePosition.z;

                    this.transform.LookAt(new Vector3(x, y, z));
                }
                else
                {
                    this.transform.LookAt(mHomePosition);
                }

                //  this.transform.Rotate(-x, 0, -z);
                this.transform.Translate(Vector3.forward * MoveSpeed * 0.5f);
            }
            isSeeking = true;
        }
        //   else
        //   {
        //       state = EnemyState.Idle;
        //   }
    } //在警戒范围内巡逻

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
            isSkillOne = false;
            isSkillTwo = false;
            Enemystate = EnemyState.Stop;
            _ani.SetBool("Died", isDied);

        }
    }

    void RunAway()
    {
        if (!isDied)
        {

            //怪物逃跑，会逃到怪物据点中心去
            float x = me.transform.position.x * -1.0f;
            float y = this.transform.position.y;
            float z = me.transform.position.z * -1.0f;

            this.transform.LookAt(new Vector3(x, y, z));
            this.transform.Translate(Vector3.forward * MoveSpeed * 0.6f);
            this.GetComponent<Enemy>().SP--;

            if (distanceToMe > alarmedRange) Enemystate = EnemyState.Stop;
            if (distanceToMe < alarmedRange && this.GetComponent<Enemy>().SP == 0) Enemystate = EnemyState.Seek;

        }
        else isDied = true;
    } //怪物残血逃跑

    void Respawn()
    {
        //暂时无重生功能
    }//重生功能暂无

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

        distanceToHome = Vector3.Distance(mHomePosition, transform.position);
        if (distanceToHome > homeRange) Enemystate = EnemyState.Homing;
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
    } //靠近角色，开始攻击

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
        _ani.SetBool("SkillOne", isSkillOne);
        _ani.SetBool("SkillTwo", isSkillTwo);
    }

    void SetAttack() { isAttacking = false; }//用于攻击动画，动画结束末尾就不在攻击，等待下一次攻击开始

    void SetDamageTrigger()//用于攻击动画造成伤害的一瞬间，再释放伤害触发器
    {
        if (EnemyType == 1)
        {
            _damageTrigger = Instantiate(DamageTriggerPrefab) as GameObject;
            _damageTrigger.GetComponent<Damage>().SetDamage(_enemy.STR);
            _damageTrigger.GetComponent<Damage>().damageForm = DamageFrom.Enemy;
            _damageTrigger.transform.SetParent(this.transform.Find("root"));
            _damageTrigger.transform.position = transform.forward + transform.position;
            _damageTrigger.transform.localRotation = new Quaternion(0, 0, 0, 0);

            StartCoroutine(WaitFireTime(FireTime));
        }
        else if (EnemyType == 2)
        {
            _damageTrigger = Instantiate(bullet) as GameObject;
            _damageTrigger.transform.position = transform.forward + transform.position;
            _damageTrigger.transform.Find("DamageTrigger").GetComponent<Damage>().damageForm = DamageFrom.Enemy;

            Bullet bulletTemp = _damageTrigger.GetComponent<Bullet>();
            bulletTemp.SetDamage(bulletTemp.GetDamage() + _enemy.STR);

            Ray ray = new Ray(this.transform.position, me.transform.position - this.transform.position);
            float deviationX = Random.Range(0, bulletTemp.GetDeviation());
            float deviationY = Random.Range(0, bulletTemp.GetDeviation());
            float deviationZ = Random.Range(0, bulletTemp.GetDeviation());

            _damageTrigger.transform.rotation = Quaternion.LookRotation(ray.direction + new Vector3(deviationX, deviationY, deviationZ));//设定方向
            _damageTrigger.GetComponent<Rigidbody>().velocity = _damageTrigger.transform.forward * bullet.GetComponent<Bullet>().GetSpeed();

            //_damageTrigger.transform.Translate(Vector3.forward * FireSpeed);
        }
    }

    void SetSeek()
    {
        isSeeking = false;
    }//用于攻击动画，动画结束末尾就不追寻，等待下一次判断追寻

    void SetDied()
    {
        Destroy(this.gameObject);
    }//死亡动画结束后销毁
}
