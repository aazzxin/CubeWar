using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    private string SpeedParam = "AbsSpeed";
    private string LRTurn = "LRTurn";

    [SerializeField]
    private Camera _camera;
    private Rigidbody _rd;
    private Human _human;
    private Animator _anim;
    private Transform pivotTarget;
    private Transform rightHandPivot;//用于持枪使，手臂跟着视角旋转

    private float _rotSpeed = 1.5f;
    private float rotationValue = 0f;
    private float maxSpeed = 10f;

    public float mos;

    private bool isGrounded;
    private bool isSprinting;
    private bool canLeftAttack=true;//判断是否能释放近战伤害触发器
    private bool leftAttacking;//是否左键攻击中
    private bool rightAttacking;//是否右键攻击中
    private bool takeGun;//是否持枪状态
    private bool isDied = false;
    public void SetDied(bool temp) { isDied = temp; }

    public void SetTakeGun (bool temp) { takeGun = temp; }
    private bool canShoot = true;//判断是否可远程攻击

    [SerializeField]
    private GameObject damagePrefab;
    private GameObject damageTrigger;

    private GameObject leftWeapon;
    private GameObject rightWeapon;
    private GameObject bullet;
    public void SetBullet(GameObject temp) { bullet = temp; }
    private bool isProduceBullet = false;//判断是否在生成子弹


    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag==("Ground"))
        {
            isGrounded = true;
            _anim.SetBool("Ground", true);
        }
    }


    // Use this for initialization

    void OnGUI()//准心
    {
        int size = 12;
        float posX = _camera.pixelWidth / 2 - size / 4;
        float posY = _camera.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posX, posY, size, size), "*");
    }

    void Start () {

        Cursor.lockState = CursorLockMode.Locked;//锁定鼠标
        Cursor.visible = false;

        _rd = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _human=GetComponent<Human>();

        mos = this.GetComponent<Human>().MOS;
        pivotTarget = transform.Find("pivot");
        rightHandPivot = transform.Find("root").Find( "C01_hand_right_pivot");

        damageTrigger = null;

        leftWeapon = MainGameManager.playerManager.leftWeapon;
        rightWeapon = MainGameManager.playerManager.rightWeapon;
        //rightWeapon.SetActive(false);
        //bullet = Resources.Load("bullet/" + rightWeapon.GetComponentInChildren<Weapon>().gameObject.name + "_bullet") as GameObject;
    }
    void Update()
    {
        if(!isDied)
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                ////_rd.AddForce(new Vector3(0, 200, 0));
                //_rd.velocity = new Vector3(_rd.velocity.x, _rd.velocity.y + MOS * 0.03f, _rd.velocity.z);
                //_anim.SetTrigger("Jump");
                _anim.SetBool("Ground", false);
                isGrounded = false;
            }

            Sprint();
            Attack();

            if (_human.HP <= 0)
                isDied = true;
        }
        else
        {
            _anim.SetBool("Died", isDied);
        }
        
    }

    // Update is called once per frame
    void FixedUpdate () {
        if(!isDied)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            float jump = Input.GetAxis("Jump");

            //setMove
            SetForwardMove(y); SetLRMove(x);

            //
            SetRotation(mouseX, mouseY);

            //
            Fly(jump);

            if (!(jump > 0 || (isSprinting && (Mathf.Abs(x) > 0 || Mathf.Abs(y) > 0))) && _human.SP < _human.MaxSp)  //当不是飞行，或不是在冲刺,且未满MaxSp时
            {
                _human.SP += 1;
            }

            //
            SetAnimationControllerParam(y, x, jump, (-1 * pivotTarget.localRotation.x), leftAttacking, rightAttacking, takeGun);
        }
        
	}

    void SetForwardMove(float y)
    {
        if (!isGrounded)
            transform.Translate(Vector3.forward * Time.fixedDeltaTime * y * mos * 0.1f);
        else
            transform.Translate(Vector3.forward * Time.fixedDeltaTime * y * mos * 0.08f);
        if (isSprinting)
            _human.SP -= 2 * Mathf.Abs(y);
    }

    void SetLRMove(float x)
    {
        if (!isGrounded)
            transform.Translate(Vector3.left * Time.fixedDeltaTime * -x * mos * 0.1f);
        else
            transform.Translate(Vector3.left * Time.fixedDeltaTime * -x * mos * 0.08f);
        if (isSprinting)
            _human.SP -= 2 * Mathf.Abs(x);
    }

    void SetRotation(float mouseX,float mouseY)
    {
        transform.Rotate(0, mouseX * _rotSpeed * Time.fixedDeltaTime*60, 0);
        rotationValue += -1 * mouseY * _rotSpeed * Time.fixedDeltaTime*60;
        rotationValue = Mathf.Clamp(rotationValue, -30, 30);
        pivotTarget.localEulerAngles = new Vector3(rotationValue, pivotTarget.rotation.y,pivotTarget.rotation.z);
        rightHandPivot.localEulerAngles = new Vector3(rotationValue, rightHandPivot.rotation.y, rightHandPivot.rotation.z);
    }

    void Fly(float jump)
    {
        if (_human.SP > 0) 
        {
            if (_rd.velocity.y < 0.01f * mos)
                _rd.velocity = new Vector3(_rd.velocity.x, (_rd.velocity.y + 0.005f * jump * mos), _rd.velocity.z);
            else
                _rd.velocity = new Vector3(_rd.velocity.x, 0.02f * jump * mos, _rd.velocity.z);
            _human.SP -= jump;
            if (isSprinting)
                _human.SP -= jump;
        }
    }

    void SetAnimationControllerParam(float speed,float LRTurn, float jump,float UDDirection,bool leftAttack,bool rightAttack,bool gun)
    {
        _anim.SetFloat(SpeedParam, Mathf.Abs(speed));
        _anim.SetFloat(this.LRTurn, LRTurn);
        _anim.SetFloat("Jump", jump);
        _anim.SetFloat("Speed", speed);

        _anim.SetFloat("UDDirection", UDDirection);
        
        //攻击动画设置
        _anim.SetBool("LeftAttack", leftAttack);//左键攻击中
        _anim.SetBool("RightAttack", rightAttack);//右键攻击中
        _anim.SetBool("TakeGun", gun);//远程武器状态
    }

    void Sprint()
    {
        if (Input.GetButton("Sprint"))
        {
            if (_human.SP > 0)
            {
                isSprinting = true;
                mos = 300.0f;
            }
            else
            {
                isSprinting = false;
                mos = 100.0f;
            }
        }
        else if (Input.GetButtonUp("Sprint") || _human.SP <= 0)
        {
            isSprinting = false;
            mos = 100.0f;
        }
            
    }

    void Attack()
    {
        if (Input.GetMouseButton(0) && System.Convert.ToInt32(MainGameManager.playerManager.package.weapon[1].GetID()) > 0)
        {
            leftWeapon.SetActive(true);
            rightWeapon.SetActive(false);
            if (canLeftAttack)
                leftAttacking = true;
            takeGun = false;
            canShoot = false;
            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            leftAttacking = false;
            if (damageTrigger != null)
            {
                Destroy(damageTrigger);
            }
        }
        //右键
        if (Input.GetMouseButton(1) && System.Convert.ToInt32(MainGameManager.playerManager.package.weapon[0].GetID()) > 0) 
        {
            if (takeGun == false)
            {
                takeGun = true;
                leftWeapon.SetActive(false);
                rightWeapon.SetActive(true);
            }
            else if (canShoot && !isProduceBullet)
            {
                rightAttacking = true;
                canShoot = false;

                StartCoroutine(GunCD());
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            rightAttacking = false;
        }
    }

    void SetDamageTrigger()//用于近战攻击动画造成伤害的一瞬间，再释放伤害触发器
    {
        if(Input.GetMouseButton(0))
        {
            if (damageTrigger == null)
            {
                damageTrigger = GameObject.Instantiate(damagePrefab);
                //近战需要直接给伤害触发器的_damage添加STR
                damageTrigger.GetComponent<Damage>().SetDamage(leftWeapon.GetComponentInChildren<Weapon>().GetDamage() + _human.STR);
                damageTrigger.GetComponent<Damage>().damageForm = DamageFrom.Human;
                damageTrigger.transform.SetParent(this.transform);
                damageTrigger.transform.position = transform.forward + transform.position;
                damageTrigger.transform.localRotation = new Quaternion(0, 0, 0, 0);

                canLeftAttack = false;
                StartCoroutine(WaitFireTime(0.1f));
                StartCoroutine(WaitCanAttack((2 / (10 + _human.ATS)) + (leftWeapon.GetComponentInChildren<Weapon>().GetCD())));
            }
        }
    }

    IEnumerator WaitFireTime(float time)
    {
        leftAttacking = false;
        yield return new WaitForSeconds(time);
        if (damageTrigger != null)
        {
            Destroy(damageTrigger);
        }
    }

    IEnumerator WaitCanAttack(float time)
    {
        yield return new WaitForSeconds(time);
        canLeftAttack = true;
    }

    IEnumerator GunCD()
    {
        isProduceBullet = true;
        MainGameManager.playerManager.package.weapon[0].SetCurrentQuantity(MainGameManager.playerManager.package.weapon[0].GetCurrentQuantity() - 1);
        MainGameManager.playerManager.package.weaponNum[0] = MainGameManager.playerManager.package.weapon[0].GetCurrentQuantity();
        if (MainGameManager.playerManager.package.weaponNum[0] <= 0)
            MainGameManager.playerManager.package.SetWeapon();
        MainGameManager.uiManager.SetWeapon();

        Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
        Ray ray = _camera.ScreenPointToRay(point);//确定基本射线方向

        for(int i=0;i<bullet.GetComponent<Bullet>().GetCountsTimes();i++)
        {
            //这里damageTrigger即子弹
            damageTrigger = GameObject.Instantiate(bullet);
            damageTrigger.transform.Find("DamageTrigger").GetComponent<Damage>().damageForm = DamageFrom.Human;

            //远程则通过给子弹附加STR,从而影响伤害触发器的_damage,增加攻击力
            Bullet bulletTemp = damageTrigger.GetComponent<Bullet>();
            bulletTemp.SetDamage(bulletTemp.GetDamage() + _human.STR);

            //damageTrigger.transform.SetParent(this.transform);
            damageTrigger.transform.position = transform.forward + transform.position + new Vector3(0, 0.7f, 0);//设定位置



            float deviationX = Random.Range(0, bulletTemp.GetDeviation());
            float deviationY = Random.Range(0, bulletTemp.GetDeviation());
            float deviationZ = Random.Range(0, bulletTemp.GetDeviation());

            damageTrigger.transform.rotation = Quaternion.LookRotation(ray.direction + new Vector3(0, 0.15f, 0) + new Vector3(deviationX, deviationY, deviationZ));//设定方向
            damageTrigger.GetComponent<Rigidbody>().velocity = damageTrigger.transform.forward * bullet.GetComponent<Bullet>().GetSpeed();
        }

        yield return new WaitForSeconds((rightWeapon.GetComponentInChildren<Weapon>().GetCD()) + (2 / (10 + _human.ATS)));
        canShoot = true;
        isProduceBullet = false;
    }

    void SetGun() { canShoot = true; }//用于动画事件，TakeGun播放完后canShoot才为真。避免换枪时就射子弹
    void SetRightAttack() { rightAttacking = false; }//用于动画事件，动画一遍结束,在末尾停止射击，等待下一次canShoot
    void GameOver()
    {
        Application.LoadLevel("GameOver");
    }
}