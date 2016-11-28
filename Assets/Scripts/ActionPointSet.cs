using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionPointSet : MonoBehaviour {

    public GameView gameView;

    bool touchingHero = false;

    bool hasEndACP = false;

    public bool setttingAC = false;

    bool isEnd = false;

    public List<MapGrid> mgsSetedAC = new List<MapGrid>();

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (gameView.EnableInput && gameView.State == GameState.Normal && !UIManager._Instance.HasUI())
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = gameView.cameraControl.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit2D[] rhs = null;
                rhs = Physics2D.RaycastAll(gameView.cameraControl.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                bool touchHero = false;
                for (int i = 0; i < rhs.Length; i++)
                {
                    RaycastHit2D rh = rhs[i];
                    GameObject gobjTouch = rh.collider.gameObject;
                    if (gobjTouch.CompareTag("PlayerTouch"))
                    {
                        touchHero = true;
                        if (!touchingHero)
                        {
                            setttingAC = true;
                            OnTouchHero();
                        }
                    }
                    else if (gobjTouch.CompareTag("MapGrid"))
                    {
                        MapGrid mgTouched = gobjTouch.GetComponent<MapGrid>();
                        if (mgTouched.g_Id != gameView._MHero._CurGridid)
                        {
                            if (setttingAC && !isEnd)
                            {
                                if (mgTouched.Type != EGridType.Block)
                                {
                                    if (!mgTouched.HasActionPoint())
                                    {
                                        mgTouched.SetActionPoint(true);
                                        mgsSetedAC.Add(mgTouched);
                                        hasEndACP = true;
                                        if (mgTouched.GetItemGobj() != null || (GameManager.hero.HasAlternessEnermy() && mgsSetedAC.Count >= 2))
                                        {
                                            // 遇到了某些东西,设置终点。或者被敌人注意时，只能行走一格
                                            if (!isEnd)
                                            {
                                                if (CheckAC())
                                                {
                                                    SetACEnd(mgTouched);
                                                }
                                                else
                                                {
                                                    CancelSetACP();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!touchHero)
                {
                    touchingHero = false;
                }
            }
            else
            {
                if (setttingAC)
                {
                    if (!hasEndACP)
                    {
                        setttingAC = false;
                        touchingHero = false;
                        CancelSetACP();
                    }
                    else
                    {
                        if (!isEnd)
                        {
                            if (CheckAC())
                            {
                                SetACEnd(mgsSetedAC[mgsSetedAC.Count - 1]);
                            }
                            else
                            {
                                CancelSetACP();
                            }
                        }
                    }
                }
            }
        }
	}

    // TODO SetACEnd
    void SetACEnd(MapGrid mg)
    {
        if (!isEnd)
        {
            if (mg.g_Id != gameView._MHero.GetCurMapGrid().g_Id)
            {
                mg.SetActionPointToEnd();
            }
            else
            {
                CancelSetACP();
            }
        }
        isEnd = true;
    }


    /// <summary>
    /// 检查动作点的连续性
    /// </summary>
    /// <returns></returns>
    bool CheckAC()
    {
        bool success = true;
        MapGrid mgTemp = null;
        for (int i = 0; i < mgsSetedAC.Count; i++)
        {
            MapGrid mg = mgsSetedAC[i];
            if (mgTemp == null)
            {
                mgTemp = mg;
            }
            else
            {
                int difVal = Mathf.Abs(mgTemp.g_Id - mg.g_Id);
                if (difVal > 1 && difVal != gameView.gGameMapOri.width)
                {
                    success = false;
                    break;
                }

                mgTemp = mg;
            }
        }
        return success;
    }

    void OnTouchHero()
    {
        touchingHero = true;
        MapGrid mgHero = gameView._MHero.GetCurMapGrid();
        if (!isEnd)
        {
            mgHero.SetActionPoint(true);
            mgsSetedAC.Add(mgHero);
        }
    }

    /// <summary>
    /// 取消设置动作点
    /// </summary>
    public void CancelSetACP()
    {
        touchingHero = false;
        hasEndACP = false;
        setttingAC = false;
        isEnd = false;

        for (int i = 0; i < mgsSetedAC.Count; i++)
        {
            MapGrid mg = mgsSetedAC[i];
            mg.ClearACP();
        }
        mgsSetedAC.Clear();
    }

    public void OnTapAMapGrid(MapGrid mapGrid) 
    {
        if (mapGrid.g_Id == gameView._MHero._CurGridid)
        {
            if (isEnd)
            {
                CancelSetACP();
            }
        }
        else if (mapGrid.IsEndACP)
        {
            StartCoroutine(gameView._MHero.CoMoveByGrids(mgsSetedAC));
        }
    }
}
