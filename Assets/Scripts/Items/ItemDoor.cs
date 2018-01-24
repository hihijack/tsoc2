using UnityEngine;
using System.Collections;
using System;

public enum EDoorType
{
    SingleMap,//单地图门
    MulMap  //跨地图门
}
public class ItemDoor : MonoBehaviour
{
    public string guid;
    public EDoorType type;
    [Tooltip("需要的道具ID")]
    public int needItemID;
    [Tooltip("打开状态")]
    public bool opened;//打开状态
    public EDirection[] enableDirs;//允许的方向
    [Tooltip("是否允许打开。跨地图门才有用")]
    public bool enableOpen;//允许打开
    [Tooltip("另一侧的门GUID。跨地图门才使用")]
    public string guidOtherSide;//另一侧的门GUID

    public SpriteRenderer txu;
    public Sprite txuClose;
    public Sprite txuOpen;

    MapGrid mg;

    // Use this for initialization
    void Start()
    {
        EventsMgr.GetInstance().AttachEvent(eEventsKey.OpenDoor, EventOpenDoor);

        mg = transform.parent.GetComponent<MapGrid>();

        if (!string.IsNullOrEmpty(guid) && GameView.Inst.HasRecordDoorOpend(guid))
        {
            SetOpen(true);
        }
        RefreshState();
    }

    public bool EnableOpen()
    {
        bool enable = false;
        if (type == EDoorType.SingleMap)
        {
            MapGrid mgHero = Hero.Inst.GetCurMapGrid();
            EDirection dir = mg.GetDirToOther(mgHero);
            if (CommonCPU.Inst.ContainDirs(enableDirs, dir))
            {
                enable = true;
            }
        }
        else if (type == EDoorType.MulMap)
        {
            //跨地图
            if (enableOpen)
            {
                enable = true;
            }
        }
        return enable;
    }

    private void SetOpen(bool v)
    {
        opened = true;
    }

    private void RefreshState()
    {
        if (opened)
        {
            txu.sprite = txuOpen;
        }
        else
        {
            txu.sprite = txuClose;
        }
    }

    void OnDestroy()
    {
        EventsMgr.GetInstance().DetachEvent(eEventsKey.OpenDoor, EventOpenDoor);
    }

    private void EventOpenDoor(object data)
    {
        object[] datas = (object[])data;
        bool open = (bool)datas[0];
        string guid = (string)datas[1];
        if (guid == this.guid)
        {
            Open();
        }
    }

    [ContextMenu("GenGUID")]
    public void GenGUID()
    {
        guid = Tools.GetGUID();
    }

    /// <summary>
    /// 开门
    /// </summary>
    internal void Open()
    {
        if (!opened)
        {
            SetOpen(true);
            RefreshState();
            //保存记录
            GameView.Inst.SaveDoorOpend(guid);
            //打开关联的门
            if (type == EDoorType.MulMap && !string.IsNullOrEmpty(guidOtherSide))
            {
                GameView.Inst.SaveDoorOpend(guidOtherSide);
                EventsMgr.GetInstance().TriigerEvent(eEventsKey.OpenDoor, new object[] { true, guidOtherSide });
            }
        }
    }
}
