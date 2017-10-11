using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameMap : MonoBehaviour {

    public GameMapBaseData baseData;
    public List<Enermy> mListEnermys = new List<Enermy>();
    public List<MapGrid> mListMGs = new List<MapGrid>();

    internal void Init(GameMapBaseData data)
    {
        this.baseData = data;
    }

    public void AddToEnermyList(Enermy e)
    {
        mListEnermys.Add(e);
    }

    public void RemoveFormEnermyList(Enermy e)
    {
        if (mListEnermys.Contains(e))
        {
            mListEnermys.Remove(e);
        }
    }

    public void AddToMGList(MapGrid mg)
    {
        mListMGs.Add(mg);
    }

    public void RemoveFromMGList(MapGrid mg)
    {
        if (mListMGs.Contains(mg))
        {
            mListMGs.Remove(mg);
        }
    }
}
