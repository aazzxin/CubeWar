using UnityEngine;
using System.Collections;

public class MyCamera : MonoBehaviour {

    //摄像机朝向的目标模型
    public Transform target;

    private Vector3 _position;
    private float smothing = 5f;

    [SerializeField]
    private bool hitTerrain = false;
    void Start()
    {
        _position = transform.localPosition;
    }

    void FixedUpdate()
    {

        hitTerrain = false;

        //这里是计算射线的方向，从主角发射方向是射线机方向
        //Vector3 aim = target.position;
        ////得到方向
        //Vector3 ve = (target.position - transform.position).normalized;
        //float an = transform.eulerAngles.y;
        //aim -= an * ve;
        //在场景视图中可以看到这条射线
        Debug.DrawLine(target.position, transform.position, Color.red);
        //主角朝着这个方向发射射线
        RaycastHit hit;
        if (Physics.Linecast(target.position, transform.position, out hit))
        {
            string name = hit.collider.gameObject.tag;
            if ( hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                //当碰撞到地形 那么直接移动摄像机的坐标
                transform.position = Vector3.Lerp(transform.position, hit.point, smothing * Time.deltaTime);
                hitTerrain = true;
            }
        }

        if(!hitTerrain)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _position, smothing * Time.deltaTime);
        }
        //// 让射线机永远看着主角
        //transform.LookAt(target);

    }
}
