using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using Mono.Data.Sqlite;

public static class GameDatas {

	public static Dictionary<int, TraderBaseData> dicTraders = new Dictionary<int, TraderBaseData>();
	static void AddTrades(TraderBaseData trade)
	{
		if (!dicTraders.ContainsKey(trade.id)) {
			dicTraders.Add(trade.id, trade);
		}
	}

	public static TraderBaseData GetTrader(int id)
	{
		TraderBaseData r = null;
		if (dicTraders.ContainsKey(id)) 
		{
			r = dicTraders[id];
		}else
		{
			Debug.LogError("无法找到商人:" + id);
		}
		return r;
	} 
	#region 怪物
	public static MonsterBaseData GetMonsterBaseData(int id)
	{
		MonsterBaseData actor = null;
        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from " + DBTableNames.TABLE_MONS + " where id =" + id);
            if (sdr.Read())
            {
                actor = new MonsterBaseData(sdr);
                sdr.Close();
            }
            else
            {
                Debug.LogWarning("找不到怪物:" + id);
            }
        }
        else
        {
            Debug.LogError("未连接至数据库");
        }
		return actor;
	}

    /// <summary>
    /// 取一个等级区间的随机一个怪物
    /// </summary>
    /// <param name="minlv"></param>
    /// <param name="maxlv"></param>
    /// <returns></returns>
    public static MonsterBaseData GetMonsterBDBetweenLevel(int minlv, int maxlv) 
    {
        MonsterBaseData mbd = null;
        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery
                ("select * from " + DBTableNames.TABLE_MONS + " where level between " + minlv
                  + " and " + maxlv + " and not boss" + " order by random() limit 1");
            if (sdr.Read())
            {
                mbd = new MonsterBaseData(sdr);
                sdr.Close();
            }
        }
        return mbd;
    }
	#endregion

#region NPCS
    public static NPCBaseData GetNPCBaseData(int id) 
    {
        NPCBaseData npcBaseData = null;
        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from " + DBTableNames.TABLE_NPCS + " where id =" + id);
            if (sdr.Read())
            {
                npcBaseData = new NPCBaseData(sdr);
                sdr.Close();
            }
            else
            {
                Debug.LogError("找不到NPC:" + id);
            }
        }
        else
        {
            Debug.LogError("未连接至数据库");
        }
        return npcBaseData;
    }

    /// <summary>
    /// 取随机一种神坛
    /// </summary>
    /// <returns></returns>
    public static NPCBaseData GetARandomAltar()
    {
        NPCBaseData npcBaseData = null;
        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from " + DBTableNames.TABLE_NPCS + " where type = 2 order by random() limit 1");
            if (sdr.Read())
            {
                npcBaseData = new NPCBaseData(sdr);
                sdr.Close();
            }
        }
        else
        {
            Debug.LogError("未连接至数据库");
        }
        return npcBaseData;
    }
#endregion

    #region 物品装备
    public static EquipItemBaseData GetEIBD(int id)
    {
        EquipItemBaseData eibd = null;

        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from " + DBTableNames.TABLE_ITEMBASE + " where id =" + id);
            if (sdr.Read())
            {
                eibd = new EquipItemBaseData(sdr);
                sdr.Close();
            }
        }
        else
        {
            Debug.LogError("未连接至数据库");
        }

        return eibd;
    }

    public static EquipItemLegendBaseData GetLegendEIBD(int id) 
    {
        EquipItemLegendBaseData ei = null;
        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from " + DBTableNames.TABLE_ITEMLEGEND + " where id =" + id);
            if (sdr.Read())
            {
                ei = new EquipItemLegendBaseData(sdr);
                sdr.Close();
            }
            else
            {
                Debug.LogError("找不到指定传奇:" + id);
            }
        }
        else
        {
            Debug.LogError("未连接至数据库");
        }
        return ei;
    }
    /// <summary>
    /// 获取指定财宝等级区间内的随机N个物品id
    /// </summary>
    /// <param name="minTLevel"></param>
    /// <param name="maxTLevel"></param>
    /// <returns></returns>
    public static int[] GetEquipItemBaseIdsBetweenTLevel(int minTLevel, int maxTLevel, int count) 
    {
        int[] ids = new int[count];
        int index = -1;
        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select id from " + DBTableNames.TABLE_ITEMBASE + " where tlevel_base between " 
                + minTLevel + " and " + maxTLevel + " order by random() limit " + count);
            while (sdr.Read())
            {
                index++;
                ids[index] = int.Parse(sdr["id"].ToString());
            }
            sdr.Close();
        }
        else
        {
            Debug.LogError("未连接至数据库");
        }

        return ids;
    }

    public static EquipItemWordsBaseData GetEIWord(int id) 
    {
        EquipItemWordsBaseData eiWord = null;
        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from " + DBTableNames.TABLE_ITEMWORDS + " where id =" + id);
            if (sdr.Read())
            {
                eiWord = new EquipItemWordsBaseData(sdr);
                sdr.Close();
            }
            else
            {
                Debug.LogError("找不到指定词缀：" + id);
            }
        }
        else
        {
            Debug.LogError("未连接至数据库");
        }
        return eiWord;
    }

    /// <summary>
    /// 获取在指定等级区间的随机N个不同属性id的词缀
    /// </summary>
    /// <param name="levelMin"></param>
    /// <param name="levelMax"></param>
    /// <returns></returns>
    public static List<EquipItemWordsBaseData> GetEIWordsBetweenLevel(int levelMin, int levelMax, int count, int itemType) 
    {
        List<EquipItemWordsBaseData> listR = new List<EquipItemWordsBaseData>();

        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery(string.Format("select item_words.*,item_propertys.classid from item_words, item_propertys where item_words.prop_id = item_propertys.id and item_type like '%_{0}_%' and level between {1} and {2} group by prop_id order by random() limit {3}", itemType, levelMin, levelMax, count));
            while (sdr.Read())
            {
                EquipItemWordsBaseData tbd = new EquipItemWordsBaseData(sdr);
                tbd.classid = (int)sdr["classid"];
                listR.Add(tbd);
            }
            sdr.Close();
        }
        else
        {
            Debug.LogError("未连接至数据库");
        }

        Dictionary<int, EquipItemWordsBaseData> dicR = new Dictionary<int, EquipItemWordsBaseData>();
        for (int i = 0; i < listR.Count; i++)
        {
            EquipItemWordsBaseData dataT = listR[i];
            if (!dicR.ContainsKey(dataT.classid))
            {
                dicR.Add(dataT.classid, dataT);
            }
        }

        listR.Clear();

        foreach (EquipItemWordsBaseData item in dicR.Values)
        {
            listR.Add(item);
        }

        return listR;
    }
#endregion
#region 技能
    public static SkillBD GetSkillBD(int id)
    {
        SkillBD bd = null;
        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from " + DBTableNames.TABLE_SKILLS + " where id =" + id);
            if (sdr.Read())
            {
                bd = new SkillBD(sdr);
                sdr.Close();
            }
            else
            {
                Debug.LogWarning("找不到技能：" + id);
            }
        }
        else
        {
            Debug.LogWarning("未连接至数据库");
        }
        return bd;
    }

    public static List<SkillBD> GetAllBattleSkills()
    {
        List<SkillBD> list = new List<SkillBD>();
        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from " + DBTableNames.TABLE_SKILLS + " where type = 1");
            while (sdr.Read())
            {
                list.Add(new SkillBD(sdr));
            }
            sdr.Close();
        }
        return list;
    }

    public static BuffBaseData GetBuff(int id) 
    {
        BuffBaseData buffBD = null;
        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from " + DBTableNames.TABLE_BUFFS + " where id =" + id);
            if (sdr.Read())
            {
                buffBD = new BuffBaseData(sdr);
                sdr.Close();
            }
            else
            {
                Debug.LogWarning("找不到buff：" + id);
            }
        }
        return buffBD;
    }

    /// <summary>
    /// 取怪物技能
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static MonSkillBD GetMonSkillBD(int id)
    {
        MonSkillBD skilBD = null;
        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from " + DBTableNames.TABLE_MONSKILLS + " where id =" + id);
            if (sdr.Read())
            {
                skilBD = new MonSkillBD(sdr);
                sdr.Close();
            }
            else
            {
                Debug.LogError("找不到怪物技能,id=" + id);
            }
        }
        return skilBD;
    }
    //public static void InitSkillsBD()
    //{
    //    JSONNode jdNodes = JSONNode.Parse((Resources.Load("GameData/Skills", typeof(TextAsset)) as TextAsset).ToString());
    //    for (int i = 0; i < jdNodes.Count; i++)
    //    {
    //        JSONNode jd = jdNodes[i];
    //        ESkill skillType = (ESkill)jd["id"].AsInt;
    //        SkillBD skillBd = null;
    //        switch (skillType)
    //        {
    //            case ESkill.MortalStrike:
    //                {
    //                    // 致死打击
    //                    skillBd = new MortalStrikeBD(jd);
    //                }
    //                break;
    //            case ESkill.BattleRoar:
    //                {
    //                    // 战斗怒吼
    //                    skillBd = new BattleRoarBD(jd);
    //                }
    //                break;
    //            default:
    //                break;
    //        }
    //        if (skillBd != null)
    //        {
    //            AddSkillBD(skillBd);
    //        }
    //    }
    //}
#endregion
    #region 任务
    public static MissionBD GetMissionBD(int missionId)
    {
        MissionBD missionBD = null;
        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from " + DBTableNames.TABLE_MISSION + " where id =" + missionId);
            if (sdr.Read())
            {
                missionBD = new MissionBD(sdr);
                sdr.Close();
            }
            else
            {
                Debug.LogError("找不到任务：" + missionId);
            }
        }
        return missionBD;
    }

    /// <summary>
    /// 取指定任务的下一个任务
    /// </summary>
    /// <param name="missionId"></param>
    /// <returns></returns>
    public static MissionBD GetNextMission(MissionBD mission) 
    {
        MissionBD nextMissionBD = null;
        int missionNextId = mission.next;
        if (missionNextId != 0)
        {
            if (GameManager.dba != null)
            {
                SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from " + DBTableNames.TABLE_MISSION + " where id =" + missionNextId);
                if (sdr.Read())
                {
                    nextMissionBD = new MissionBD(sdr);
                    sdr.Close();
                }
                else
                {
                    Debug.LogError("找不到任务：" + missionNextId);
                }
            }
        }
       
        return nextMissionBD;
    }

    /// <summary>
    /// 获取指定父任务的所有子任务
    /// </summary>
    /// <param name="parentMissionId"></param>
    /// <returns></returns>
    public static List<MissionBD> GetChildMissions(int parentMissionId)
    {
        List<MissionBD> missions = new List<MissionBD>();
        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from " + DBTableNames.TABLE_MISSION + " where parent =" + parentMissionId + " order by step");
            while(sdr.Read())
            {
                missions.Add(new MissionBD(sdr));
            }
        }
        return missions;
    }
    #endregion
    #region 地图
    public static GameMapBaseData GetGameMapBD(int id)
    {
        GameMapBaseData mapBD = null;
        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from " + DBTableNames.TABLE_GAMEMAP + " where id =" + id);
            if (sdr.Read())
            {
                mapBD = new GameMapBaseData(sdr);
                sdr.Close();
            }
            else
            {
                Debug.LogError("找不到指定地图:" + id);
            }
        }
        return mapBD;
    }

    /// <summary>
    /// 获取指定层的地图
    /// </summary>
    /// <param name="tier"></param>
    /// <returns></returns>
    public static GameMapBaseData GetGameMapBDByTier(int tier) 
    {
        GameMapBaseData map = null;
        if (GameManager.dba != null)
        {
            SqliteDataReader sdr = GameManager.dba.ExecuteQuery("select * from " + DBTableNames.TABLE_GAMEMAP + " where tier = " + tier);
            if (sdr.Read())
            {
                map = new GameMapBaseData(sdr);
                sdr.Close();
            }
            else 
            {
                Debug.LogError("找不到指定层的地图:" + tier);
            }
        }
        return map;
    }
#endregion
}
