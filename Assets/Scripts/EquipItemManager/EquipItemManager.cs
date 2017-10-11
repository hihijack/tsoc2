using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

/// <summary>
/// 装备数据管理器
/// </summary>
public class EquipItemManager
{
    // 使用的道具id
    public int[] arrItemUesd = new int[3]; 
    // 已装备的装备
    public List<EquipItem> itemsHasEquip = new List<EquipItem>();
    // 背包里的物品
    public List<EquipItem> itemsInBag = new List<EquipItem>();

    private int gold; // 金币


    public int _Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            if (gold < 0)
            {
                gold = 0;
            }
            PlayerPrefs.SetInt(IConst.KEY_GOLD, gold);
            UIManager.Inst.RefreshHeroBagGold();
        }
    }


    /// <param name="ei"></param>
    public void AddToItemsHasEquip(EquipItem ei)
    {
        itemsHasEquip.Add(ei);
    }

    public void RemoveFromItemHasEquip(EquipItem ei)
    {
        if (itemsHasEquip.Contains(ei))
        {
            itemsHasEquip.Remove(ei);
        }
    }

    public void AddToItemsInBag(EquipItem ei)
    {
        itemsInBag.Add(ei);
    }

    public void RemoveFromItemsInBag(EquipItem ei)
    {
        if (itemsInBag.Contains(ei))
        {
            itemsInBag.Remove(ei);
        }
    }

    /// <summary>
    /// 取背包里指定类型的装备
    /// </summary>
    /// <returns></returns>
    public EquipItem GetEquipItemInBagById(int id)
    {
        EquipItem ei = null;
        for (int i = 0; i < itemsInBag.Count; i++)
        {
            EquipItem eiItem = itemsInBag[i];
            if (eiItem.baseData.id == id)
            {
                ei = eiItem;
                break;
            }
        }
        return ei;
    }

    /// <summary>
    /// 取背包里指定装备的数量；id-装备基础id
    /// </summary>
    /// <returns></returns>
    public int GetEquipItemCountInBag(int id)
    {
        int count = 0;
        for (int i = 0; i < itemsInBag.Count; i++)
        {
            EquipItem eiItem = itemsInBag[i];
            if (eiItem.baseData.id == id)
            {
                count += eiItem.count;
            }
        }
        return count;
    }

    public void SetItemUsed(int index, int eiId)
    {
        arrItemUesd[index] = eiId;
    }
    #region 保存读取
    /// <summary>
    /// 保存玩家装备
    /// </summary>
    public void SaveEquipItems()
    {
        string fileName = Application.persistentDataPath + "/eiinbag.dat";
        Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
        BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器
        binFormat.Serialize(fStream, itemsInBag);

        string fileName2 = Application.persistentDataPath + "/eihasequip.dat";
        Stream fStream2 = new FileStream(fileName2, FileMode.Create, FileAccess.ReadWrite);
        BinaryFormatter binFormat2 = new BinaryFormatter();//创建二进制序列化器
        binFormat.Serialize(fStream2, itemsHasEquip);

        Debug.Log("SaveEquipItems Complete");
        fStream.Close();
        fStream2.Close();
    }

    public void ReadEquipItem()
    {
        string fileName = Application.persistentDataPath + "/eiinbag.dat";
        string fileName2 = Application.persistentDataPath + "/eihasequip.dat";

        if (File.Exists(fileName))
        {
            Stream fStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
            BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器
            itemsInBag = (List<EquipItem>)binFormat.Deserialize(fStream);
            fStream.Close();
        }

        if (File.Exists(fileName2))
        {
            Stream fStream2 = new FileStream(fileName2, FileMode.Open, FileAccess.ReadWrite);
            BinaryFormatter binFormat2 = new BinaryFormatter();//创建二进制序列化器
            itemsHasEquip = (List<EquipItem>)binFormat2.Deserialize(fStream2);
            fStream2.Close();
        }

        //Debug.Log("ReadEquipItem" + GameManager.hero.itemsInBag.Count + "  " + GameManager.hero.itemsHasEquip.Count);//###########
    }

    /// <summary>
    /// 保存使用的道具
    /// </summary>
    public void SaveItemUesd()
    {
        string strData = "";
        for (int i = 0; i < arrItemUesd.Length; i++)
        {
            int itemId = arrItemUesd[i];
            strData = strData + itemId + "&";
        }
        PlayerPrefs.SetString(IConst.KEY_ITEM_USED, strData);
    }

    public void ReadAndSetItemUsed()
    {
        if (PlayerPrefs.HasKey(IConst.KEY_ITEM_USED))
        {
            string strData = PlayerPrefs.GetString(IConst.KEY_ITEM_USED);
            string[] strs = strData.Split('&');
            for (int i = 0; i < strs.Length; i++)
            {
                if (!string.IsNullOrEmpty(strs[i]))
                {
                    SetItemUsed(i, int.Parse(strs[i]));
                }
            }
        }
    }
    #endregion

    #region 装备管理
    /// <summary>
    /// 生成一件装备/道具
    /// </summary>
    /// <param name="id"></param>
    /// <param name="qlevel"></param>
    /// <returns></returns>
    public EquipItem GenerateAEquipItem(int id, EEquipItemQLevel qlevel)
    {
        EquipItem ei = new EquipItem();
        // 生成词缀
        int baseId; // 基础物品表id
        EquipItemLegendBaseData eiLegendbd = null; // 传奇基础数据
        if (qlevel == EEquipItemQLevel.Legend)
        {
            eiLegendbd = GameDatas.GetLegendEIBD(id);
            baseId = eiLegendbd.baseId;
        }
        else
        {
            baseId = id;
        }

        EquipItemBaseData eibd = GameDatas.GetEIBD(baseId);


        ei.id = GenerateEquipItemId();
        ei.baseData = eibd;
        ei.legendBaseData = eiLegendbd;
        ei.qLevel = qlevel;
        ei.words = new List<EquipItemWord>();

        if (qlevel == EEquipItemQLevel.Normal || eibd.qLevel == 0)
        {
            // 无词缀
            ei.qLevel = EEquipItemQLevel.Normal;
        }
        else if (qlevel == EEquipItemQLevel.Magic)
        {
            int wordsCount = UnityEngine.Random.Range(IConst.EQUIPITEM_MAGIC_WORDS_MINCOUNT, IConst.EQUIPITEM_MAGIC_WORDS_MAXCOUNT);
            int wordLevlMin = GetMinWordLevel(eibd.qLevel);
            int wordLevelMax = GetMaxWordLevel(eibd.qLevel);
            List<EquipItemWordsBaseData> wordBDs = GameDatas.GetEIWordsBetweenLevel(wordLevlMin, wordLevelMax, wordsCount, (int)ei.baseData.type);
            for (int i = 0; i < wordBDs.Count; i++)
            {
                EquipItemWordsBaseData bdT = wordBDs[i];
                ei.words.Add(GenerateAEquipItemWord(bdT));
            }
        }
        else if (qlevel == EEquipItemQLevel.Uncommon)
        {
            int wordsCount = UnityEngine.Random.Range(IConst.EUIIPITEM_UNNORMAL_WORDS_MINCOUNT, IConst.EUIIPITEM_UNNORMAL_WORDS_MAXCOUNT);
            int wordLevelMin = GetMinWordLevel(eibd.qLevel);
            int wordLevelMax = GetMaxWordLevel(eibd.qLevel);
            List<EquipItemWordsBaseData> wordBDs = GameDatas.GetEIWordsBetweenLevel(wordLevelMin, wordLevelMax, wordsCount, (int)ei.baseData.type);
            for (int i = 0; i < wordBDs.Count; i++)
            {
                EquipItemWordsBaseData bdT = wordBDs[i];
                ei.words.Add(GenerateAEquipItemWord(bdT));
            }
        }
        else if (qlevel == EEquipItemQLevel.Legend)
        {
            // 传奇使用固定词缀
            if (eiLegendbd != null)
            {
                for (int i = 0; i < eiLegendbd.wordIds.Length; i++)
                {
                    int wordId = eiLegendbd.wordIds[i];
                    EquipItemWordsBaseData wordBD = GameDatas.GetEIWord(wordId);
                    ei.words.Add(GenerateAEquipItemWord(wordBD));
                }
            }
        }

        string desc = "生成装备:" + qlevel + eibd.name + "，护甲:" + eibd.arm + "，攻击力:" + eibd.atk + "攻击速度:" + eibd.ias + ",";
        for (int i = 0; i < ei.words.Count; i++)
        {
            desc = desc + ei.words[i].ToString() + ",";
        }

        Debug.Log(desc);

        return ei;
    }

    /// <summary>
    /// 随机一个词缀的属性
    /// </summary>
    /// <param name="bd"></param>
    /// <returns></returns>
    EquipItemWord GenerateAEquipItemWord(EquipItemWordsBaseData bd)
    {
        EquipItemWord eiw = new EquipItemWord();
        eiw.wordBaseData = bd;
        eiw.val = UnityEngine.Random.Range(bd.valMin, bd.valMax);
        return eiw;
    }

    /// <summary>
    /// 根据装备品质等级获取词缀最低等级
    /// </summary>
    /// <param name="qlevel"></param>
    /// <returns></returns>
    int GetMinWordLevel(int qlevel)
    {
        int r = qlevel - 1;
        if (r < 1)
        {
            r = 1;
        }
        return r;
    }

    /// <summary>
    /// 根据装备品质等级获取词缀最高等级
    /// </summary>
    /// <param name="qlevel"></param>
    /// <returns></returns>
    int GetMaxWordLevel(int qlevel)
    {
        int r = qlevel;
        return r;
    }

    // 生成一个装备的唯一标识
    public string GenerateEquipItemId()
    {
        return System.Guid.NewGuid().ToString();
    }

    /// <summary>
    /// 给英雄装备一件装备
    /// </summary>
    /// <param name="eiToEquip"></param>
    public void EquipAEquipItem(EquipItem eiToEquip)
    {
        // 如果该部位已经有装备，则将该部位装备放入背包
        EEquipPart partToEquip = EEquipPart.None;
        switch (eiToEquip.baseData.type)
        {
            case EEquipItemType.Helm:
                partToEquip = EEquipPart.Helm;
                break;
            case EEquipItemType.Necklace:
                partToEquip = EEquipPart.Necklace;
                break;
            //case EEquipItemType.Shoulder:
            //    partToEquip = EEquipPart.Shoulder;
            //    break;
            case EEquipItemType.Breastplate:
                partToEquip = EEquipPart.Breastplate;
                break;
            //case EEquipItemType.Cuff:
            //    partToEquip = EEquipPart.Cuff;
            //    break;
            case EEquipItemType.Glove:
                partToEquip = EEquipPart.Glove;
                break;
            case EEquipItemType.Pants:
                partToEquip = EEquipPart.Pants;
                break;
            case EEquipItemType.Shoe:
                partToEquip = EEquipPart.Shoe;
                break;
            case EEquipItemType.WeaponOneHand:
                partToEquip = EEquipPart.Hand1;
                break;
            case EEquipItemType.WeaponTwoHand:
                partToEquip = EEquipPart.Hand1;
                break;
            case EEquipItemType.Shield:
                partToEquip = EEquipPart.Hand2;
                break;
            default:
                break;
        }
        EquipItem eiInThisPart = GetEquipItemHasEquip(partToEquip);
        // 确定目标部位，及需要移除的装备
        if (eiInThisPart != null && eiToEquip.baseData.type == EEquipItemType.WeaponOneHand)
        {
            // 如果是单手武器，则再检查副手部位
            EquipItem eiInHand2 = GetEquipItemHasEquip(EEquipPart.Hand2);
            if (eiInHand2 == null)
            {
                // 如果副手部位已经有物品，则还是替换主手装备
                // 否则则装备到副手
                partToEquip = EEquipPart.Hand2;
                eiInThisPart = null;
            }
        }

        if (eiInThisPart != null)
        {
            // 将装备移到背包
            eiInThisPart._Part = EEquipPart.None;
            int toBagGridId = eiToEquip.BagGridId;
            if (toBagGridId == 0)
            {
                toBagGridId = GetAEmptyBagGridid();
            }
            MoveAEquipItemToBag(eiInThisPart, toBagGridId);
        }

        // 装备装备到指定部位
        AddToItemsHasEquip(eiToEquip);
        eiToEquip._Part = partToEquip;
        eiToEquip.BagGridId = 0;
    }

    /// <summary>
    /// 获取身上某个部位的装备
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public EquipItem GetEquipItemHasEquip(EEquipPart part)
    {
        EquipItem ei = null;
        for (int i = 0; i < itemsHasEquip.Count; i++)
        {
            EquipItem eiHasEquip = itemsHasEquip[i];
            if (eiHasEquip._Part == part)
            {
                ei = eiHasEquip;
                break;
            }
        }
        return ei;
    }
    /// <summary>
    /// 获取金币
    /// </summary>
    /// <param name="val"></param>
    public void GetGold(int val)
    {
        _Gold += val;
    }

    public void ReadSavedGold()
    {
        if (PlayerPrefs.HasKey(IConst.KEY_GOLD))
        {
            _Gold = PlayerPrefs.GetInt(IConst.KEY_GOLD);
        }
        else
        {
            _Gold = 0;
        }
    }

    public bool AddAEquipItemToBag(EquipItem ei, out EquipItem eiAlert)
    {
        bool success = false;
        bool canPile = false;
        eiAlert = null;
        // 取背包里同类型的物品
        EquipItem eiInBag = GetEquipItemInBagById(ei.baseData.id);
        if (eiInBag != null)
        {
            if (eiInBag.count < ei.baseData.pile)
            {
                // 可堆叠
                canPile = true;
            }
        }

        if (canPile)
        {
            eiInBag.count++;
            eiAlert = eiInBag;
            success = true;
        }
        else
        {
            // 找出可以放置的格子
            int emptyGridId = GetAEmptyBagGridid();
            if (emptyGridId > 0)
            {
                AddToItemsInBag(ei);
                ei.BagGridId = emptyGridId;
                eiAlert = ei;
                success = true;
            }
            else
            {
                Debug.LogError("取不到空的格子");
            }
        }
        return success;
    }

    /// <summary>
    /// 将一件装备移到背包指定格子。返回是否从身上移除
    /// </summary>
    /// <param name="hero"></param>
    /// <param name="ei"></param>
    public bool MoveAEquipItemToBag(EquipItem ei, int gridid)
    {
        bool moveFromEQ = false;
        if (itemsHasEquip.Contains(ei))
        {
            moveFromEQ = true;
            // 卸下装备
            RemoveFromItemHasEquip(ei);
            ei._Part = EEquipPart.None;
            AddToItemsInBag(ei);
            ei.BagGridId = gridid;
            GameView.Inst.RemoveEquipItemPropFromHero(ei);
        }
        else
        {
            ei.BagGridId = gridid;
        }
        return moveFromEQ;
    }

    /// <summary>
    /// 将一件装备装备到指定部位.返回是否从背包移出
    /// </summary>
    /// <param name="hero"></param>
    /// <param name="ei"></param>
    /// <param name="part"></param>
    public bool MoveAEquipItemToEquip(EquipItem ei, EEquipPart part)
    {
        bool fromBag = false;
        if (!itemsHasEquip.Contains(ei))
        {
            fromBag = true;
            // 从背包移到装备栏
            if (itemsInBag.Contains(ei))
            {
                RemoveFromItemsInBag(ei);
            }

            AddToItemsHasEquip(ei);
            ei._Part = part;
            ei.BagGridId = 0;

            GameView.Inst.AddEuqipItemPrpToHero(ei);
        }
        else
        {
            fromBag = false;
            ei._Part = part;
            ei.BagGridId = 0;
        }
        return fromBag;
    }

    /// <summary>
    /// 从身上或背包移除装备
    /// </summary>
    /// <param name="ei"></param>
    public void RemoveEquipItem(EquipItem ei)
    {
        if (ei.BagGridId > 0)
        {
            RemoveFromItemsInBag(ei);
        }
        else
        {
            RemoveFromItemHasEquip(ei);
            GameView.Inst.RemoveEquipItemPropFromHero(ei);
        }
    }



    /// <summary>
    /// 获取背包里一个空的格子id
    /// </summary>
    /// <returns></returns>
    int GetAEmptyBagGridid()
    {
        int empty = 0;
        int[] ids = new int[60];

        // 在数组中标记已被占用格子
        for (int i = 0; i < itemsInBag.Count; i++)
        {
            EquipItem ei = itemsInBag[i];
            ids[ei.BagGridId - 1] = 1;
        }
        // 找到第一个未被占用格子
        for (int i = 0; i < ids.Length; i++)
        {
            if (ids[i] == 0)
            {
                empty = i + 1;
                break;
            }
        }
        return empty;
    }

    public string GetEIName(EquipItem ei)
    {
        string name = "";
        if (ei.qLevel == EEquipItemQLevel.Normal)
        {
            name = ei.baseData.name;
        }
        else if (ei.qLevel == EEquipItemQLevel.Magic)
        {
            name = "[436EEE]" + ei.baseData.name + "[-]";
        }
        else if (ei.qLevel == EEquipItemQLevel.Uncommon)
        {
            name = "[EEEE00]" + ei.baseData.name + "[-]";
        }
        else if (ei.qLevel == EEquipItemQLevel.Legend)
        {
            name = "[FF7F00]" + ei.legendBaseData.name + "\n[i]" + ei.baseData.name + "[/i][-]";
        }
        return name;
    }

    /// <summary>
    /// 获取装备描述
    /// </summary>
    /// <param name="ei"></param>
    /// <returns></returns>
    public string GetEquipItemDesc(EquipItem ei)
    {
        StringBuilder desc = new StringBuilder();

        // 名字
        string name = GetEIName(ei);

        desc.Append(name);
        desc.Append("\n");

        if (ei.baseData.arm > 0)
        {
            // 显示护甲
            desc.Append("护甲：" + ei.baseData.arm + "\n");
        }
        else if (ei.baseData.atk > 0)
        {
            // 显示攻击
            if (ei.baseData.type == EEquipItemType.WeaponOneHand)
            {
                desc.Append("单手伤害:" + ei.baseData.atk + "\n");
            }
            else if (ei.baseData.type == EEquipItemType.WeaponTwoHand)
            {
                desc.Append("双手伤害:" + ei.baseData.atk + "\n");
            }
            desc.Append("武器速度:" + ei.baseData.ias.ToString("f1") + "次/秒\n");
        }
        //基础格挡率
        if (ei.baseData.parry > 0)
        {
            desc.Append("格挡伤害削减:" + ei.baseData.parry + "%\n");
            desc.Append("格挡值:" + ei.baseData.parryVigor + "\n");
        }
        //重量
        desc.Append("重量:" + ei.baseData.weight + "\n");
        //移动速度
        if (ei.baseData.movespeed > 0)
        {
            desc.Append("速度:" + ei.baseData.movespeed);
        }
        // 显示魔法属性
        string strProp = "";
        for (int i = 0; i < ei.words.Count; i++)
        {
            strProp = strProp + ei.words[i].ToString() + "\n";
        }

        if (ei.qLevel == EEquipItemQLevel.Magic)
        {
            strProp = "[436EEE]" + strProp + "[-]";
        }
        else if (ei.qLevel == EEquipItemQLevel.Uncommon)
        {
            strProp = "[EEEE00]" + strProp + "[-]";
        }
        else if (ei.qLevel == EEquipItemQLevel.Legend)
        {
            strProp = "[FF7F00]" + strProp + "[-]";
        }

        desc.Append(strProp);

        // 描述
        if (!string.IsNullOrEmpty(ei.baseData.desc))
        {
            desc.Append(ei.baseData.desc);
        }

        // 特殊描述
        if (ei.legendBaseData != null)
        {
            if (!string.IsNullOrEmpty(ei.legendBaseData.desc))
            {
                desc.Append("\n[4F4F4F][i]" + ei.legendBaseData.desc + "[/i][-]");
            }
        }
        return desc.ToString();
    }

    public EquipItem GetEIDataInBtnGobj(GameObject gobj)
    {
        UIButton btn = gobj.GetComponent<UIButton>();
        return btn.data as EquipItem;
    }

    /// <summary>
    /// 能否装备到指定部位
    /// </summary>
    /// <param name="ei"></param>
    /// <param name="part"></param>
    /// <returns></returns>
    public bool CanEquip(EquipItem ei, EEquipPart part)
    {
        bool r = false;
        switch (ei.baseData.type)
        {
            case EEquipItemType.Helm:
                if (part == EEquipPart.Helm)
                {
                    r = true;
                }
                break;
            case EEquipItemType.Necklace:
                if (part == EEquipPart.Necklace)
                {
                    r = true;
                }
                break;

            case EEquipItemType.Breastplate:
                if (part == EEquipPart.Breastplate)
                {
                    r = true;
                }
                break;

            case EEquipItemType.Glove:
                if (part == EEquipPart.Glove)
                {
                    r = true;
                }
                break;
            case EEquipItemType.Pants:
                if (part == EEquipPart.Pants)
                {
                    r = true;
                }
                break;
            case EEquipItemType.Shoe:
                if (part == EEquipPart.Shoe)
                {
                    r = true;
                }
                break;
            case EEquipItemType.WeaponOneHand:
                if (part == EEquipPart.Hand1 || part == EEquipPart.Hand2)
                {
                    r = true;
                }
                break;
            case EEquipItemType.WeaponTwoHand:
                if (part == EEquipPart.Hand1)
                {
                    r = true;
                }
                break;
            case EEquipItemType.Shield:
                if (part == EEquipPart.Hand2)
                {
                    r = true;
                }
                break;
            default:
                break;
        }

        return r;
    }
    #endregion

}