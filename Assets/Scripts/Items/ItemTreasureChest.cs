using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 一个地图上的宝箱
/// </summary>
public class ItemTreasureChest : MonoBehaviour {

    public string guid;
    public List<EquipItem> listEquipItems = new List<EquipItem>();

    [Tooltip("百分比_基础id_最小个数_最大个数:baseid&百分比_id:legendid&百分比_tlevel_最小个数_最大个数_稀有几率百分比_魔法几率百分比:tlevel")]
    public string drop;  // 掉落信息

    [Tooltip("是否已经初始化物品列表")]
    public bool hasInitItems = false; // 是否已经初始化物品列表

	// Use this for initialization
	void Start () {
        if (!string.IsNullOrEmpty(guid) && GameView.Inst.HasChestRecord(guid))
        {
            //已经被打开过
            MonoKit.DestroyObject(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InitItems()
    {
        if (!string.IsNullOrEmpty(drop))
        {
            listEquipItems.Clear();
            listEquipItems.AddRange(GameManager.gameView.GetMonsterDropList(drop));
            hasInitItems = true;
        }
        
    }

    [ContextMenu("GenGUID")]
    public void GenGUID()
    {
        guid = Tools.GetGUID();
    }

}
