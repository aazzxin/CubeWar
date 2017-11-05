using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    //脚本挂载在Frame上，检测子对象是否active
    public void OnPointerDown(PointerEventData data)//点下鼠标时
    {
        isPointerDown = true;
        
        //将所在的层级放在最下层，以便可以与其余所以icon交换
        transform.SetAsFirstSibling();
        transform.parent.SetAsFirstSibling();
        transform.parent.parent.SetAsFirstSibling();
    }
    public void OnPointerUp(PointerEventData data)//释放鼠标时
    {
        if(isPointerDown)
        {
            transform.SetSiblingIndex(index);//换回原来层级

            if (DragType == 1 && MainGameManager.itemManager.ItemTemp2Drag.DragType != 2)
            {
                //将_ref传给ItemManger
                MainGameManager.itemManager.ItemTemp1 = _ref;
                MainGameManager.itemManager.ItemTemp1Drag = this;
                MainGameManager.itemManager.Swap();
            }
            if (DragType == 2 && MainGameManager.itemManager.ItemTemp2Drag.DragType != 1)
            {
                //将_ref传给ItemManger
                MainGameManager.itemManager.ItemTemp1 = _ref;
                MainGameManager.itemManager.ItemTemp1Drag = this;
                MainGameManager.itemManager.Swap();
            }

            RectTransform rt = _icon.GetComponent<RectTransform>();
            rt.localPosition = _iconRectPosition;
            
        }

        isPointerDown = false;
    }
    public void OnPointerEnter(PointerEventData data)//鼠标进入时
    {
        isEnter = true;
        //将_ref传给ItemManger
        MainGameManager.itemManager.ItemTemp2Drag = this;
        MainGameManager.itemManager.ItemTemp2 = _ref;

    }
    public void OnPointerExit(PointerEventData data)
    {
        isEnter = false;
        MainGameManager.itemManager.ItemTemp2 = null;
    }
    public void OnDrag(PointerEventData data)
    {
        //拖动时，图标子对象跟着鼠标位置拖动
        var rt = _icon.GetComponent<RectTransform>();

        // transform the screen point to world point int rectangle
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
        }
    }

    
    public int DragType = 1;//1为背包、储物柜中的道具，互相可以拖动交换；2为商品道具，与1道具不可交换；0道具为买卖品,当买卖栏有道具时，变成对应号码。默认为1道具

    private GameObject _icon;
    private Item _ref;//对应到相应的道具。在UIManager设定好
    public void SetRef(Item item) { _ref = item; }

    private Vector3 _iconRectPosition;//初始的位置
    private int index;//用于记录当前在组里的序号，与层级有关

    private bool isPointerDown = false;
    [SerializeField]
    private bool isEnter = false;

    void Awake() {
        _icon = transform.GetChild(0).gameObject;
        _iconRectPosition = new Vector3(0, 0, 0);
        index = transform.GetSiblingIndex();
    }
    
}
