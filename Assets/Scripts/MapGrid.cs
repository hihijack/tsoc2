using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGrid : MonoBehaviour {

	#region 属性
	public int g_Id;

    public int x;
    public int y;

	public EGridType g_Type;
	public EGridType Type
	{
		get
		{
			return g_Type;
		}
		set
		{
			g_Type = value;
		}
	}

    public bool enableCreateAMon = true;// 允许刷新怪物

    public int F;
    public int G;
    public int H;

    public void CalcF()
    {
        this.F = this.G + this.H;
    }

    public T GetItem<T> () where T : Component
    {
        Component r = null;
        if (transform.childCount > 0)
        {
            GameObject gobjItemGobj = GetItemGobj();
            if (gobjItemGobj != null)
            {
                r = gobjItemGobj.GetComponent<T>();
            }
            
        }
        return r as T;
    }

    public GameObject GetItemGobj()
    {
        GameObject gobj = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform tfChild = transform.GetChild(i);
            if (tfChild.CompareTag("MapItem"))
            {
                gobj = tfChild.gameObject;
                break;
            }
        }
        return gobj;
    }

    public Vector3 GetPos()
    {
        return gameObject.transform.position;
    }

#region 地图切换
    public int toMapId;
    public int _ToMapId
    {
        get { return toMapId; }
        set { toMapId = value; }
    }

    public int toMapTargetGrid;

    public int _ToMapTargetGrid
    {
        get { return toMapTargetGrid; }
        set { toMapTargetGrid = value; }
    }

#endregion


#region 动作点
    GameObject g_GobjActionPoint;
    bool isEndACP;

    public bool IsEndACP
    {
        get { return isEndACP; }
        set 
        { 
            isEndACP = value; 
        }
    }
#endregion
    #endregion
    public void ClearACP()
    {
        SetActionPoint(false);
        IsEndACP = false;
    }

    public void SetActionPoint(bool show)
    {
        if (show)
        {
            if (g_GobjActionPoint == null)
            {
                g_GobjActionPoint = Tools.LoadResourcesGameObject("Prefabs/action_point", gameObject, 0f, 0f, 0f);
            }
            g_GobjActionPoint.SetActive(true);
            GameObject flag = Tools.GetGameObjectInChildByPathSimple(g_GobjActionPoint, "priateflag");
            flag.SetActive(false);
        }
        else
        {
            if (g_GobjActionPoint != null)
            {
                g_GobjActionPoint.SetActive(false);
            }
        }
    }

    //public bool IsEndACP()
    //{
    //    return isEndACP;
    //}

    /// <summary>
    /// 设置当前动作点为终点
    /// </summary>
    public void SetActionPointToEnd()
    {
        if (HasActionPoint())
        {
            GameObject flag = Tools.GetGameObjectInChildByPathSimple(g_GobjActionPoint, "priateflag");
            flag.SetActive(true);
            IsEndACP = true;
        }
    }

    public bool HasActionPoint()
    {
        return g_GobjActionPoint != null && g_GobjActionPoint.activeInHierarchy;
    }


	public void InitByTypeAndItemID()
	{
        //foreach (Transform tfItem in transform)
        //{
        //    DestroyImmediate(tfItem.gameObject);
        //}

        //if (Type == EGridType.Trader) 
        //{
        //    TraderBaseData trader = GameResources.GetTrader(ItemId);
        //    if (trader != null) 
        //    {
        //        GameObject gobjTrader = Tools.LoadResourcesGameObject(IPath.Items + trader.model, gameObject, 0f, 0f, 0f);
        //        SetIemDirection(gobjTrader, Dir);
        //    }
        //}
        //else if (Type == EGridType.Monster) {
        //    MonsterBaseData mon = GameResources.GetMonsterBaseData(ItemId);
        //    if (mon != null) {
        //        GameObject gobjMon = Tools.LoadResourcesGameObject(IPath.Items + mon.model, gameObject, 0f, 0f, 0f);
        //        SetIemDirection(gobjMon, Dir);
        //    }
        //}
        //else if (Type == EGridType.NPC)
        //{
        //    NPCBaseData npc = GameResources.GetNPCBaseData(ItemId);
        //    if (npc != null)
        //    {
        //        GameObject gobjNPC = Tools.LoadResourcesGameObject(IPath.NPCs + npc.model, gameObject, 0f, 0f, 0f);
        //        SetIemDirection(gobjNPC, Dir);
        //    }
        //}
	}

    public void SetTxu(Sprite sprite)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = sprite;
        }
    }

    void SetIemDirection(GameObject gobjItem, EDirection dir)
    {
        float angy = 0f;
        switch (dir)
        {
            case EDirection.Up:
                angy = 0f;
                break;
            case EDirection.Down:
                angy = 180f;
                break;
            case EDirection.Left:
                angy = -90f;
                break;
            case EDirection.Right:
                angy = 90f;
                break;
            default:
                break;
        }
        gobjItem.transform.eulerAngles = new Vector3(0f, angy, 0f);
    }

    /// <summary>
    /// 移除格子上物品
    /// </summary>
    public void SetToNone()
    {
        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            Transform tfChild = transform.GetChild(i);
            if (tfChild.CompareTag("MapItem"))
            {
                DestroyObject(tfChild.gameObject);
            }
        }
        //Type = EGridType.None;
    }

    /// <summary>
    /// 是否相邻一个格子
    /// </summary>
    /// <param name="otherGrid"></param>
    /// <returns></returns>
    public bool IsNear(MapGrid otherGrid) 
    {
        bool isnear = true;
        int difVal = Mathf.Abs(g_Id - otherGrid.g_Id);
        if (difVal > 1 && difVal != GameManager.gameView.gGameMapOri.width)
        {
            isnear = false;
        }
        return isnear;
    }
    
    /// <summary>
    /// 获取周围的格子
    /// </summary>
    /// <returns></returns>
    public List<MapGrid> GetNearGrids()
    {
        List<MapGrid> list = new List<MapGrid>();
        int idUp = g_Id - GameManager.gameView.gGameMapOri.width;
        if (idUp >= 0)
        {
            MapGrid mgUp = GameManager.gameView.GetMapGridById(idUp);
            if (mgUp != null)
            {
                list.Add(mgUp);
            }
        }

        int idBottom = g_Id + GameManager.gameView.gGameMapOri.width;
        if (idBottom >= 0)
        {
            MapGrid mgBottom = GameManager.gameView.GetMapGridById(idBottom);
            if (mgBottom != null)
            {
                list.Add(mgBottom);
            }
            
        }

        if (!IsMapLeft(GameManager.gameView.gGameMapOri.width))
        {
            int idLeft = g_Id - 1;
            MapGrid mgLeft = GameManager.gameView.GetMapGridById(idLeft);
            if (mgLeft != null)
            {
                list.Add(mgLeft);
            }
            
        }

        if (!IsMapRight(GameManager.gameView.gGameMapOri.width))
        {
            int idRight = g_Id + 1;
            MapGrid mgRight = GameManager.gameView.GetMapGridById(idRight);
            if (mgRight != null)
            {
                list.Add(mgRight);
            }
        }

        return list;
    }

    public MapGrid GetNextGrid(EDirection dir) 
    {
        MapGrid nextMG = null;
        if (dir == EDirection.Up)
        {
            int idUp = g_Id - GameManager.gameView.gGameMapOri.width;
            if (idUp >= 0)
            {
                MapGrid mgUp = GameManager.gameView.GetMapGridById(idUp);
                if (mgUp != null)
                {
                    nextMG = mgUp;
                }
            }
        }
        else if (dir == EDirection.Down)
        {
            int idBottom = g_Id + GameManager.gameView.gGameMapOri.width;
            if (idBottom >= 0)
            {
                MapGrid mgBottom = GameManager.gameView.GetMapGridById(idBottom);
                if (mgBottom != null)
                {
                    nextMG = mgBottom;
                }

            }
        }
        else if (dir == EDirection.Left)
        {
            if (!IsMapLeft(GameManager.gameView.gGameMapOri.width))
            {
                int idLeft = g_Id - 1;
                MapGrid mgLeft = GameManager.gameView.GetMapGridById(idLeft);
                if (mgLeft != null)
                {
                    nextMG = mgLeft;
                }

            }
        }
        else if (dir == EDirection.Right)
        {
            if (!IsMapRight(GameManager.gameView.gGameMapOri.width))
            {
                int idRight = g_Id + 1;
                MapGrid mgRight = GameManager.gameView.GetMapGridById(idRight);
                if (mgRight != null)
                {
                    nextMG = mgRight;
                }
            }
        }
        return nextMG;
    }

    /// <summary>
    /// 获取4个边角格子
    /// </summary>
    /// <returns></returns>
    public List<MapGrid> GetCornerGrids() 
    {
        List<MapGrid> mgs = new List<MapGrid>();
        
        if (!IsMapLeft(GameManager.gameView.gGameMapOri.width))
        {
            int idTopLeft = g_Id - GameManager.gameView.gGameMapOri.width - 1;
            if (idTopLeft >= 0)
            {
                MapGrid mgTL = GameManager.gameView.GetMapGridById(idTopLeft);
                if (mgTL != null)
                {
                    mgs.Add(mgTL);
                }
            }

            int idBottomLeft = g_Id + GameManager.gameView.gGameMapOri.width - 1;
            if (idBottomLeft >= 0)
            {
                MapGrid mgBL = GameManager.gameView.GetMapGridById(idBottomLeft);
                if (mgBL != null)
                {
                    mgs.Add(mgBL);
                }
            }
        }

        if (!IsMapRight(GameManager.gameView.gGameMapOri.width))
        {
            int idTopRight = g_Id - GameManager.gameView.gGameMapOri.width + 1;
            if (idTopRight >= 0)
            {
                MapGrid mgTR = GameManager.gameView.GetMapGridById(idTopRight);
                if (mgTR != null)
                {
                    mgs.Add(mgTR);
                }
            }

            int idBottomRight = g_Id + GameManager.gameView.gGameMapOri.width + 1;
            if (idBottomRight >= 0)
            {
                MapGrid mgBR = GameManager.gameView.GetMapGridById(idBottomRight);
                if (mgBR != null)
                {
                    mgs.Add(mgBR);
                }
            }
        }

        return mgs;
    }

    /// <summary>
    /// 是否地图左边缘
    /// </summary>
    /// <returns></returns>
    public bool IsMapLeft(int mapWidth)
    {
        return g_Id % mapWidth == 0;
    }

    /// <summary>
    /// 是否地图右边缘
    /// </summary>
    /// <param name="mapWidth"></param>
    /// <returns></returns>
    public bool IsMapRight(int mapWidth) 
    {
        return (g_Id + 1) % mapWidth == 0;
    }


    //public Enermy InitAEnermy()
    //{
    //    Enermy enermy = null;
    //    GameObject gobjEnermy = transform.GetChild(0).gameObject;
    //    enermy = gobjEnermy.GetComponent<Enermy>();
    //    if (enermy != null)
    //    {
    //        return enermy;
    //    }
    //    enermy = gobjEnermy.AddComponent<Enermy>();
    //    MonsterBaseData mbd = GameResources.GetMonsterBaseData(ItemId);
    //    enermy.monsterBD = mbd;
    //    enermy.hp = mbd.hp;
    //    enermy.hpMax = mbd.hp;
    //    enermy.atkPhy = mbd.atkMin;

    //    enermy.atkAnimTimeBeforeBase = mbd.atkTimeBefore;
    //    enermy.atkAnimTimeAfterBase = mbd.atkTimeAfter;
    //    enermy._IAS = mbd.ias;
    //    enermy._CurGridid = g_Id;
    //    return enermy;
    //}

    /// <summary>
    /// 计算两个格子的距离。斜线距离为2
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static int GetDis(MapGrid from, MapGrid to)
    {
        // 距离为x的差+y的差
        int dis = 0;
        dis = Mathf.Abs(from.GetX() - to.GetX()) + Mathf.Abs(from.GetY() - to.GetY());
        return dis;
    }

    /// <summary>
    /// 取其他相邻格子在当前格子的方向
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public EDirection GetDirToOther(MapGrid other) 
    {
        EDirection dir = EDirection.Up;
        int yOffset = other.GetY() - this.GetY();
        int xOffset = other.GetX() - this.GetX();
        if (yOffset < 0)
        {
            dir = EDirection.Up;
        }
        else if (yOffset > 0)
        {
            dir = EDirection.Down;
        }
        else if(xOffset > 0)
        {
            dir = EDirection.Right;
        }
        else if(xOffset < 0)
        {
            dir = EDirection.Left;
        }
        
        return dir;
    }

    /// <summary>
    /// 计算格子所处列数
    /// </summary>
    /// <returns></returns>
    public int GetX()
    {
        int x = 0;
        x = g_Id % GameManager.gameView.gGameMapOri.width;
        return x;
    }

    /// <summary>
    /// 计算格子所处行数
    /// </summary>
    /// <returns></returns>
    public int GetY()
    {
        int y = 0;
        y = g_Id / GameManager.gameView.gGameMapOri.width;
        return y;
    }

}
