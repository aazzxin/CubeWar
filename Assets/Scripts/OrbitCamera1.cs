using UnityEngine;
using System.Collections;

public class OrbitCamera1 : MonoBehaviour {

    [SerializeField]
    private Transform target;

    public float rotSpeed = 1.5f;

    private float _rotY;
    private Vector3 _offset;
	// Use this for initialization
	void Start () {
        _rotY = target.rotation.y;
        _offset = target.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
    }

    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(0, _rotY, 0);//将旋转角转换为一个四元数
        transform.position = target.position - (rotation * _offset);//将四元数乘以一个偏移向量，得到向量做选择操作后的偏移位置，改变向量方向
        //transform.LookAt(target);//上一步只是定位，还需要选择Camera的方向
        //transform.rotation = Quaternion.Euler(transform.rotation.x + 30, transform.rotation.y, transform.rotation.z);
    }
}
