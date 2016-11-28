using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System.Collections.Generic;

/// <summary>
/// 地图上的一个NPC
/// </summary>
public class ItemNPC : MonoBehaviour {

    public int npcId;
    public NPCBaseData npcData;
    public ENPCActionType[] arrActions;
    public List<EquipItem> sells = new List<EquipItem>();

    bool autoInit = true;

    public void Start()
    {
        if (npcId > 0 && autoInit)
        {
            npcData = GameDatas.GetNPCBaseData(npcId);
            InitActionType();
            InitSells();
        }
    }

    public void Init(NPCBaseData data) 
    {
        this.npcData = data;
        this.npcId = data.id;
        InitActionType();
        autoInit = false;
    }

    public string GetWords(int mainMissionId)
    {
        string words = "";
        SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from npc_words where npc_id = " + npcData.id + " and condition = " + mainMissionId);
        if (sdr != null && sdr.Read())
        {
            words = sdr["content"].ToString();
            sdr.Close();
        }
        else
        {
            sdr = GameManager.dba.ExecuteQuery("select * from npc_words where npc_id = " + npcData.id + " and condition = 0");
            if (sdr.Read())
            {
                words = sdr["content"].ToString();
                sdr.Close();
            }else
            {
                Debug.LogError("未设置默认台词");    
            }
        }
        return words;
    }

    public void InitActionType() 
    {
        string[] strActs = npcData.action.Split('_');
        arrActions = new ENPCActionType[strActs.Length];
        for (int i = 0; i < strActs.Length; i++)
        {
            arrActions[i] = (ENPCActionType)(int.Parse(strActs[i]));
        }
    }

    void InitSells() 
    {
        string strSells = npcData.sells;
        if (!string.IsNullOrEmpty(strSells))
        {
           sells = GameManager.gameView.GetMonsterDropList(strSells);
        }
    }

    public void RemoveSells(EquipItem ei)
    {
        if (sells.Contains(ei))
        {
            sells.Remove(ei);
        }
    }
}
