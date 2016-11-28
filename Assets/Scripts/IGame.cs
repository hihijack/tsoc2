﻿using System.Collections;
using SimpleJSON;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

public enum EGridType
{
	None,	// 无
	ChangeMap,	// 地图出口入口
	Block,		// 路障
    Start,       // 玩家开始位置
    StartAndToHome, // 试炼塔开始及返回城镇
}

/// <summary>
/// 格子上物体类型
/// </summary>
public enum EItemType
{
    Enermy, // 敌人
    TreasureChest, // 宝箱
    NPC
}

/// <summary>
/// 商人
/// </summary>
public class TraderBaseData
{
	public int id;
	public string name;
	public string model;
	public int[] sellList;

	public TraderBaseData(JSONNode data)
	{
		this.id = data["id"].AsInt;
		this.name = data["name"];
		this.model = data["model"];
		JSONArray arrList = data["sell_list"].AsArray;
		sellList = arrList.ToIntArr();
	}
}

public class MonsterBaseData
{
	public int id;
	public string name;
	public string model;
	public int atkMin;
    public int atkMax;
	public int hp;
	public int level;
    public float atkTimeBefore;
    public float atkTimeAfter;
    //public float atkTimeInterval;
    public float ias; // 每秒攻击次数

    public int arm; // 护甲
    public int hit; //命中
    public int dodge; // 躲闪
    public int resfire; // 火抗
    public int reslighting; //电抗
    public int respoison; //毒抗
    public int resfrozen; //冰抗

    public string drops;// 掉落列表信息

    public string skills;   // 技能列表

    public bool isBoss; // 是否是boss

	public MonsterBaseData(JSONNode data)
	{
		this.id = data["id"].AsInt;
		this.name = data["name"];
		this.model = data["model"];
		this.atkMin = data["atk"].AsInt;
        this.atkTimeBefore = data["atktimebefore"].AsFloat;
        this.atkTimeAfter = data["atktimeafter"].AsFloat;
        //this.atkTimeInterval = data["atktimeinterval"].AsFloat;
		this.hp = data["hp"].AsInt;
		this.level = data["level"].AsInt;
	}

    public MonsterBaseData(SqliteDataReader sdr) 
    {
        this.id = (int)sdr["id"];
        this.name = sdr["name"].ToString();
        this.model = sdr["model"].ToString();
        this.atkMin = (int)sdr["atk_min"];
        this.atkMax = (int)sdr["atk_max"];
        this.hp = (int)sdr["hp"];
        this.level = (int)sdr["level"];

        this.atkTimeBefore = float.Parse(sdr["atktimebefore"].ToString());
        //this.atkTimeInterval = float.Parse(sdr["atktimeinterval"].ToString());
        this.atkTimeAfter = float.Parse(sdr["atktimeafter"].ToString());
        this.ias = float.Parse(sdr["ias"].ToString());
        this.drops = sdr["drops"].ToString();

        this.arm = (int)sdr["arm"];
        this.hit = (int)sdr["hit"];
        this.dodge = (int)sdr["dodge"];
        this.resfire = (int)sdr["resfire"];
        this.reslighting = (int)sdr["reslighting"];
        this.respoison = (int)sdr["respoison"];
        this.resfrozen = (int)sdr["resfrozen"];

        this.skills = sdr["skills"].ToString();

        this.isBoss = (bool)sdr["boss"];
    }
}

public enum ENPCActionType
{
    Bye = 0,     // 再见
    Talk = 1,   // 交谈
    Trade = 2,  // 交易
    Treat = 3,  // 治疗
    Forge = 4,   // 锻造
    Trial = 5,     // 试炼塔
    Active = 6      // 激活
}

public enum ENPCType
{
    Normal = 1, // 普通npc
    Altar = 2 // 神坛
}

public enum EAltarType
{
	Recover = 1,	// 恢复神坛
	Def = 2,		// 防御神坛
	Fight = 3,		// 战斗神坛
	Fury = 4		// 狂怒神坛
}

public class NPCBaseData
{
    public int id;
    public string model;
    public string name;
    public string action;
    public string sells;
    public ENPCType type;
	public int subType;
    public JSONNode data;

    public NPCBaseData(SqliteDataReader sdr) 
    {
        this.id = (int)sdr["id"];
        this.name = sdr["name"].ToString();
        this.model = sdr["model"].ToString();
        this.action = sdr["action"].ToString();
        this.sells = sdr["sells"].ToString();
        this.type = (ENPCType)sdr["type"];
		this.subType = (int)sdr ["subtype"];
        this.data = JSONNode.Parse(sdr["data"].ToString());
    }
}

public enum EEquipItemType
{
    Helm = 1,   // 头盔
    Necklace = 2,   // 项链
    Shoulder = 3,   // 肩膀
    Breastplate = 4,// 胸甲
    Cuff = 5,       // 护腕
    Glove = 6,      // 手套
    Pants = 7,      // 裤子
    Shoe = 8,       // 鞋子
    WeaponOneHand = 9,  // 单手武器
    WeaponTwoHand = 10, // 双手武器
    Shield = 11,         // 盾
    Gold = 20,          // 金钱
    HPPotion = 21,        // 治疗药水
    ResPoiPotion = 22         // 毒抗药水
}

/// <summary>
/// 装备部位
/// </summary>
public enum EEquipPart
{
    None = 0,   // 无
    Helm = 1,   // 头盔
    Necklace = 2,   // 项链
    Shoulder = 3,   // 肩膀
    Breastplate = 4,// 胸甲
    Cuff = 5,       // 护腕
    Glove = 6,      // 手套
    Pants = 7,      // 裤子
    Shoe = 8,       // 鞋子
    Hand1 = 9,  // 主手
    Hand2 = 10, // 副手
    BaseBody = 11, // 身体
    BaseEar = 12    // 耳朵
}

public struct NodeSprite
{
    public EEquipPart part;
    public string spName;
    public UnityEngine.Color color;
    public int layer;
    public NodeSprite(EEquipPart part, string spName, UnityEngine.Color color) 
    {
        this.part = part;
        this.spName = spName;
        this.color = color;
        this.layer = 0;
        switch (part)
        {
            case EEquipPart.Helm:
                this.layer = 2;
                break;
            case EEquipPart.Shoulder:
                this.layer = 2;
                break;
            case EEquipPart.Breastplate:
                this.layer = 2;
                break;
            case EEquipPart.Cuff:
                this.layer = 2;
                break;
            case EEquipPart.Glove:
                this.layer = 2;
                break;
            case EEquipPart.Pants:
                this.layer = 2;
                break;
            case EEquipPart.Shoe:
                this.layer = 2;
                break;
            case EEquipPart.Hand1:
                this.layer = 3;
                break;
            case EEquipPart.Hand2:
                this.layer = 3;
                break;
            case EEquipPart.BaseBody:
                this.layer = 0;
                break;
            case EEquipPart.BaseEar:
                this.layer = 0;
                break;
            default:
                break;
        }
    }
}

/// <summary>
/// 装备使用
/// </summary>
public enum EEquipItemUseType 
{
    None = 0,
    AtOnce = 1,
    NeedTarget = 2
}

/// <summary>
/// 装备基础数据
/// </summary>
[Serializable]
public class EquipItemBaseData
{
    public int id;
    public string icon;
    public string name;
    public EEquipItemType type;
    public int tLevel;  // 决定是否掉落
    public int qLevel;  // 决定生成的词缀等级
    public int arm;
    public int atk;
    public float ias;
    public string model;
    public string color;
    public int price;
    public EEquipItemUseType useType;
    public int pile; //堆叠上限
    public string data;

    public EquipItemBaseData(SqliteDataReader sdr)
    {
        this.id = (int)sdr["id"];
        this.icon = sdr["icon"].ToString();
        this.name = sdr["name"].ToString();
        this.type = (EEquipItemType)sdr["type"];
        this.tLevel = (int)sdr["tlevel_base"];
        this.qLevel = (int)sdr["qlevel_base"];
        this.arm = (int)sdr["arm"];
        this.atk = (int)sdr["atk"];
        this.ias = float.Parse(sdr["ias"].ToString());
        this.model = sdr["model"].ToString();
        this.color = sdr["color"].ToString();
        this.price = (int)sdr["price"];
        this.useType = (EEquipItemUseType)sdr["use"];
        this.pile = (int)sdr["pile"];
        this.data = sdr["data"].ToString();
    }
}

/// <summary>
/// 传奇基础数据
/// </summary>
[Serializable]
public class EquipItemLegendBaseData
{
    public int id;
    public int baseId;
    public string name;
    public string icon;
    public int[] wordIds;
    public string desc;
    public string model;

    public EquipItemLegendBaseData(SqliteDataReader sdr)
    {
        this.id = (int)sdr["id"];
        this.baseId = (int)sdr["base_id"];
        this.name = sdr["name"].ToString();
        this.icon = sdr["icon"].ToString();
        string strWords = sdr["words"].ToString();
        string[] strIdWords = strWords.Split(',');
        wordIds = new int[strIdWords.Length];
        for (int i = 0; i < strIdWords.Length; i++)
        {
            wordIds[i] = int.Parse(strIdWords[i]);
        }
        this.desc = sdr["desc"].ToString();

        this.model = sdr["model"].ToString();
    }
}

/// <summary>
/// 装备属性类型
/// </summary>
public enum EEquipItemProperty
{
    Str = 1,    // 力量
    Agi = 2,    // 敏捷
    Int = 3,    // 精神力
    Sta = 4,    // 体能
    MaxLife = 5,    // 最大生命值
    TL = 6,         // 体力
    MaxMp = 7,      // 最大魔法值
    Arm = 8,         // 护甲
    IAS = 9,        // 攻速
    ResFire = 10,   // 火抗
    ResThunder = 11,    // 电抗
    ResPoison = 12,     // 毒抗
    ResFrozen = 13,     // 冰抗
    CriticalStrike = 14,    // 致命一击
    Hit = 15,           // 命中
    Dodge = 16,         // 躲闪
    Parry = 17,         // 格挡
    ParryDamage = 18,     // 格挡伤害
    FireDamage = 19,      // 火焰伤害
    ThunderDamage = 20,   // 闪电伤害
    PoisonDamage = 21,             // 毒素伤害
    ForzenDamage = 22,            // 冰冷伤害
    AddDamage = 23          // 增强伤害
}

/// <summary>
/// 装备词缀基础数据
/// </summary>
[Serializable]
public class EquipItemWordsBaseData
{
    public int id;
    public string name;
    public int level;
    public EEquipItemProperty propertyType;
    public int valMin;
    public int valMax;
    public string color;


    public int classid;

    public EquipItemWordsBaseData(SqliteDataReader sdr)
    {
        this.id = (int)sdr["id"];
        this.name = sdr["name"].ToString();
        this.level = (int)sdr["level"];
        this.propertyType = (EEquipItemProperty)sdr["prop_id"];
        this.valMin = (int)sdr["prop_val_min"];
        this.valMax = (int)sdr["prop_val_max"];
        this.color = sdr["color"].ToString();
    }
}

public enum EDirection
{
    Up,
    Down,
    Left,
    Right
}

#region SkillBaseData
public enum ESkill
{
    MortalStrike = 1,
    BattleRoar = 2
}

/// <summary>
/// 伤害类型
/// </summary>
public enum EDamageType
{
    Phy = 1,    // 物理
    Fire = 2,   // 火
    Lighting = 3, // 电
    Poison = 4,     // 毒
    Forzen = 5      // 冰冻
}

public class SkillBD
{
    public int id;
    public string name;
    public string desc;
    public bool manual;
    public string iconName;
    public float cd;
    public int cost;
    public string anim;
    public float casttime;  // 施法时间
    public JSONNode jdData;
    public ESkillTargetType targetType; // 目标类型
    public ESkillType type;
    public bool intoBattle;
    public int lvUplimit; // 技能等级上限
    public ESkillBranch branch;
    public int needLevel;   // 学习需要等级

    public SkillBD(SqliteDataReader sdr)
    {
        id = (int)sdr["id"];
        name = sdr["name"].ToString();
        iconName = sdr["icon"].ToString();
        desc = sdr["desc"].ToString();
        this.manual = (bool)sdr["manual"];
        cd = float.Parse(sdr["cd"].ToString());
        cost = (int)sdr["cost"];
        anim = sdr["anim"].ToString();
        casttime = float.Parse(sdr["casttime"].ToString());
        jdData = JSONNode.Parse(sdr["data"].ToString());
        targetType = (ESkillTargetType)sdr["target_type"];
        type = (ESkillType)sdr["type"];
        intoBattle = (bool)sdr["into_battle"];
        lvUplimit = (int)sdr["lv_uplimit"];
        branch = (ESkillBranch)sdr["branch"];
        needLevel = (int)sdr["need_lv"];
    }

    /// <summary>
    /// 根据等级获取描述
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public string ToDesc(int level)
    {
        string r = desc;
        
        if (level <= 0)
        {
            return r;
        }

        JSONNode jdDataCurLevl = jdData[level - 1];
        Regex reg = new Regex(@"(?<=\[)[^\[\]]+(?=\])");
        MatchCollection mc = reg.Matches(desc);
        foreach (Match m in mc)
        {
            string mv = m.Value;
            string strToReplace = "[" + mv + "]";
            if (mv.Equals("cost"))
            {
                r = r.Replace(strToReplace, cost.ToString());
            }
            else if (mv.Equals("cd"))
            {
                r = r.Replace(strToReplace, cd.ToString());
            }
            else
            {
                if (mv.StartsWith("%"))
                {
                    string key = mv.Substring(1);
                    float val = jdDataCurLevl[key].AsFloat;

                    r = r.Replace(strToReplace, val.ToString("0%"));
                }
                else
                {
                    r = r.Replace(strToReplace, jdDataCurLevl[m.Value]);
                }
                
            }
        }
        return r;
    }
}

public class ComparSkillBDByLevel : IComparer<SkillBD>
{
    public int Compare(SkillBD x, SkillBD y)
    {
        return x.needLevel > y.needLevel ? 1 : -1;
    }
}

/// <summary>
/// 怪物技能基础数据
/// </summary>
public class MonSkillBD
{
    public int id;
    public string name;
    public string icon;
    public string desc;
    public JSONNode jdData;

    public MonSkillBD(SqliteDataReader sdr) 
    {
        id = (int)sdr["id"];
        name = sdr["name"].ToString();
        icon = sdr["icon"].ToString();
        desc = sdr["desc"].ToString();
        jdData = JSONNode.Parse(sdr["data"].ToString());
    }

    /// <summary>
    /// 根据等级获取描述
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public string ToDesc(int level)
    {
        string r = desc;

        if (level <= 0)
        {
            return r;
        }

        JSONNode jdDataCurLevl = jdData[level - 1];
        Regex reg = new Regex(@"(?<=\[)[^\[\]]+(?=\])");
        MatchCollection mc = reg.Matches(desc);
        foreach (Match m in mc)
        {
            string mv = m.Value;
            string strToReplace = "[" + mv + "]";
            if (mv.StartsWith("%"))
            {
                string key = mv.Substring(1);
                float val = jdDataCurLevl[key].AsFloat;
                r = r.Replace(strToReplace, (int)(val * 100) + "%");
            }
            else
            {
                r = r.Replace(strToReplace, jdDataCurLevl[m.Value]);
            }
        }
        return r;
    }

    public int GetIntVal(int level, string param) 
    {
        int val = 0;
        if (jdData != null)
        {
            val = jdData[level - 1][param].AsInt;
        }
        return val;
    }

    public float GetFloatVal(int level, string param) 
    {
        float val = 0f;
        if (jdData != null)
        {
            val = jdData[level - 1][param].AsFloat;
        }
        return val;
    }
}

public static class DBTableNames
{
    public static string TABLE_ITEMBASE = "item_base";  // 物品基础表
    public static string TABLE_ITEMLEGEND = "item_legend";  // 传奇物品表
    public static string TABLE_PROPERTYS = "item_propertys";    // 物品属性表
    public static string TABLE_ITEMWORDS = "item_words";    // 物品词缀表
    public static string TABLE_MONS = "monsters";           // 怪物表
    public static string TABLE_SKILLS = "skills";           // 技能表
    public static string TABLE_BUFFS = "buffs";             // buff表
    public static string TABLE_NPCS = "npcs";               // npc表
    public static string TABLE_MISSION = "missions";        // 任务表
    public static string TABLE_GAMEMAP = "maps";            // 游戏地图
    public static string TABLE_MONSKILLS = "mon_skills";    // 怪物技能表
}

//装备品质
public enum EEquipItemQLevel
{
    Normal = 1, // 普通
    Magic = 2,  // 魔法
    Uncommon = 3,   // 稀有
    Legend = 4      // 传奇
}

// 装备上的一条词缀
[Serializable]
public class EquipItemWord
{
    public EquipItemWordsBaseData wordBaseData;  // 词缀基础数据
    public int val;       // 词缀值

    public string ToString()
    {
        return GameTools.Prop2Desc(wordBaseData.propertyType) + "+" + val;
    }
}

// 游戏中的一个物品装备
[Serializable]
public class EquipItem
{
    public string id;  // 唯一标识

    public EquipItemBaseData baseData;  // 每个物品都有该基础数据
    public EquipItemLegendBaseData legendBaseData;  // 传奇装备会有传奇基础数据

    public EEquipItemQLevel qLevel; // 品质
    public List<EquipItemWord> words; // 词缀列表

    public int bagGridId;   // 在背包中的位置
    private EEquipPart part = EEquipPart.None;     // 装备的部位

    public EEquipPart _Part
    {
        get { return part; }
        set { part = value;}
    }

    public int count = 1;   // 堆叠

    public string GetIcon()
    {
        string icon = "";
        if (legendBaseData != null)
        {
            icon = legendBaseData.icon;
        }
        else
        {
            icon = baseData.icon;
        }
        return icon;
    }

    public string GetModel()
    {
        string model = "";
        if (legendBaseData != null)
        {
            model = legendBaseData.model;
        }
        else
        {
            model = baseData.model;
        }
        return model;
    }

    /// <summary>
    /// 装备的最终颜色
    /// </summary>
    /// <returns></returns>
    public Color GetColor() 
    {
        // 普通装备使用基础颜色
        // 魔法装备取第一条词缀颜色
        Color color = Color.white;
        if (qLevel == EEquipItemQLevel.Normal)
        {
            color = Tools.hexToColor(baseData.color);
        }
        else
        {
            if (words.Count > 0)
            {
                EquipItemWord eiwFirst = words[0];
                color = Tools.hexToColor(eiwFirst.wordBaseData.color);
            }
        }
        return color;
    }

    /// <summary>
    /// 交易价格
    /// </summary>
    /// <returns></returns>
    public int GetTradePrice() 
    {
        int price = 0;
        int basePrice = baseData.price;
        // 普通品质使用原价
        // 蓝色品质使用3倍原价
        // 黄色品质使用10倍原价
        // 传奇使用30倍原价
        switch (qLevel)
        {
            case EEquipItemQLevel.Normal:
                price = basePrice;
                break;
            case EEquipItemQLevel.Magic:
                price = Mathf.CeilToInt(basePrice * 3f);
                break;
            case EEquipItemQLevel.Uncommon:
                price = basePrice * 10;
                break;
            case EEquipItemQLevel.Legend:
                price = basePrice * 30;
                break;
            default:
                break;
        }
        return price;
    }

    /// <summary>
    /// 取品质颜色
    /// </summary>
    /// <returns></returns>
    public Color GetQLevelColor() 
    {
        Color color = Color.white;
        if (qLevel == EEquipItemQLevel.Magic)
        {
            color = Color.blue;
        }
        else if (qLevel == EEquipItemQLevel.Uncommon)
        {
            color = Color.yellow;
        }
        else if (qLevel == EEquipItemQLevel.Legend)
        {
            color = new Color(1f, 140f / 255f, 0f);
        }
        return color;
    }
}

public static class IConst
{
    public const int EQUIPITEM_MAGIC_WORDS_MINCOUNT = 1;    // 魔法品质物品的词缀最多个数
    public const int EQUIPITEM_MAGIC_WORDS_MAXCOUNT = 3;    // 魔法品质物品的词缀最多个数
    public const int EUIIPITEM_UNNORMAL_WORDS_MINCOUNT = 3; // 稀有品质物品词缀最少个数；
    public const int EUIIPITEM_UNNORMAL_WORDS_MAXCOUNT = 7; // 稀有品质物品词缀最多个数；
    public const int BASE_STR = 10; // 基础力量
    public const int BASE_AGI = 10; // 基础敏捷
    public const int BASE_INT = 10; // 基础智力
    public const int BASE_STA = 10; // 基础体能
    public const int MP_PER_INT = 10;   // 每点智力增加的魔法上限
    public const int TL_PER_STA = 10;   // 每点体能增加的体力上限
    public const int HP_PER_STA = 10;   // 每点体能增加的生命值上限
    public const int ATK_PHY_PER_STR = 2;  // 每点力量提供的物理攻击力
    public const int ATK_MAG_PER_INT = 2; // 每点智力提供的魔法攻击力
    public const int HIT_PER_AGI = 2;     // 每点敏捷提供的命中
    public const int DODGE_PER_AGI = 2;    // 每点敏捷提供的躲闪
    public const float IAS_PERCENT_PER_AGI = 1f;  // 每点敏捷提供的攻击速度百分比
    public const float BaseIAS = 0.1f; // 基础攻击速度

    public const string KEY_HASALLOT_STR = "strallot";     // 已分配的力量点
    public const string KEY_HASALLOT_AGI = "agiallot";     // 已分配的敏捷点
    public const string KEY_HASALLOT_INT = "intallot";     // 已分配的精神力点
    public const string KEY_HASALLOT_STA = "staallot";      // 已分配的体能点
    public const string KEY_NEED_ALLOT = "noallot";         // 还未分配的属性点
    public const string KEY_LEVEL = "level";    // 等级
    public const string KEY_EXP_CURLEVEL = "expcurlevel";   // 当前等级经验
    public const string KEY_SKILLS = "skills";
    public const string KEY_SKILLPOINT_NEEDALLOT = "spnotallot"; // 未分配的技能点
    public const string KEY_MISSION = "mission";    // 当前任务进度
    public const string KEY_HOMEMAP = "curhomemap"; // 当前城镇地图id
    public const string KEY_BEST_TRIAL = "besttrial"; // 试炼塔最好层数

    public const string KEY_GOLD = "gold";  // 金币数

    public const string KEY_ITEM_USED = "itemused";

    public const int EXP_A = 50;    // 经验函数A
    public const int EXP_B = -50; // 经验函数B
    public const int EXP_MON_BASE = 40; // 怪提供的经验
    public const int EXP_MON_K = 10;
    public const int LOST_GOLD_LEVEL = 300; // 死亡损失金钱
}

/// <summary>
/// 技能对象类型
/// </summary>
public enum ESkillTargetType
{
    None = 0,   // 无目标
    Unit = 1,  // 单位目标
    Grid = 2,   // 点目标
    UnitOrGrid = 3  // 单位或点目标
}

public enum ESkillType
{
    Battle = 1, // 战斗技能
    Tactics = 2 // 战术技能
}

/// <summary>
/// 战斗技能技能分支
/// </summary>
public enum ESkillBranch
{
    Wq = 1, // 武器
    Zd = 2, // 战斗
    Kb = 3, // 狂暴
    Ys = 4,// 元素
    Fy = 5  // 防御
}


public class BuffBaseData
{
    public int id;
    public string name;
    public string icon;
    public string effect;
    public string desc;

    public BuffBaseData(SqliteDataReader sdr) 
    {
        id = (int)sdr["id"];
        name = sdr["name"].ToString();
        icon = sdr["icon"].ToString();
        effect = sdr["effect"].ToString();
        desc = sdr["desc"].ToString();
    }
}

/// <summary>
/// 游戏地图
/// </summary>
public class GameMapBaseData
{
    public int id;
    public string name;
    public string scene;
    public int width;
    public int height;
    public int minLevel;
    public int maxLevel;
    public int monsterCount;
    public bool isHome; // 是否城镇。城镇不会生成怪物
    public int tier; // 层
    public int bossId; // 出现的boss

    public GameMapBaseData(SqliteDataReader sdr) 
    {
        id = (int)sdr["id"];
        name = sdr["name"].ToString();
        scene = sdr["scene"].ToString();
        width = (int)sdr["width"];
        height = (int)sdr["height"];
        minLevel = (int)sdr["minlevel"];
        maxLevel = (int)sdr["maxlevel"];
        monsterCount = (int)sdr["monstercount"];
        isHome = (bool)sdr["ishome"];
        tier = (int)sdr["tier"];
        bossId = (int)sdr["boss"];
    }
}

#endregion

#region 任务
public enum EMissionType
{
    Kill = 1, // 需要杀死目标
    InterActive = 2, // 需要交互目标
    IntoMap = 3      // 进入地图
}

public class MissionBD
{
    public int id;
    public int next;
    public int parent;
    public int step;
    public string targetDesc;
    public string desc;
    public string reward;
    public EMissionType targetType;
    public int targetId;
    public int targetNum;

    public MissionBD(SqliteDataReader sdr) 
    {
        this.id = (int)sdr["id"];
        this.next = (int)sdr["next"];
        this.parent = (int)sdr["parent"];
        this.step = (int)sdr["step"];
        this.targetDesc = sdr["target_desc"].ToString();
        this.desc = sdr["desc"].ToString();
        this.reward = sdr["reward"].ToString();
        this.targetType = (EMissionType)sdr["target_type"];
        this.targetId = (int)sdr["target_id"];
        this.targetNum = (int)sdr["target_num"];
    }
}

public class ComparMissionByStep : IComparer<MissionBD>
{
    int IComparer<MissionBD>.Compare(MissionBD x, MissionBD y)
    {
        return x.step > y.step ? 1 : -1;
    }
}
#endregion