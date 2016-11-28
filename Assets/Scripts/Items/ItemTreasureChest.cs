using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 一个地图上的宝箱
/// </summary>
public class ItemTreasureChest : MonoBehaviour {

    public List<EquipItem> listEquipItems = new List<EquipItem>();
    public string drop;  // 掉落信息

    [Tooltip("是否已经初始化物品列表")]
    public bool hasInitItems = false; // 是否已经初始化物品列表

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InitItems()
    {
        if (!string.IsNullOrEmpty(drop))
        {
            listEquipItems.AddRange(GameManager.gameView.GetMonsterDropList(drop));
            hasInitItems = true;
        }
        
    }
    
}
