using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using SimpleJSON;

public class CommonCPU : MonoBehaviour
{
    /// <summary>
    /// 保存玩家装备
    /// </summary>
    public void SaveEquipItems()
    {
        string fileName = Application.persistentDataPath + "/eiinbag.dat";
        Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
        BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器
        binFormat.Serialize(fStream, GameManager.hero.itemsInBag);

        string fileName2 = Application.persistentDataPath + "/eihasequip.dat";
        Stream fStream2 = new FileStream(fileName2, FileMode.Create, FileAccess.ReadWrite);
        BinaryFormatter binFormat2 = new BinaryFormatter();//创建二进制序列化器
        binFormat.Serialize(fStream2, GameManager.hero.itemsHasEquip);

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
            GameManager.hero.itemsInBag = (List<EquipItem>)binFormat.Deserialize(fStream);
            fStream.Close();
        }

        if (File.Exists(fileName2))
        {
            Stream fStream2 = new FileStream(fileName2, FileMode.Open, FileAccess.ReadWrite);
            BinaryFormatter binFormat2 = new BinaryFormatter();//创建二进制序列化器
            GameManager.hero.itemsHasEquip = (List<EquipItem>)binFormat2.Deserialize(fStream2);
            fStream2.Close();
        }

        Debug.Log("ReadEquipItem" + GameManager.hero.itemsInBag.Count + "  " + GameManager.hero.itemsHasEquip.Count);//###########
    }

    /// <summary>
    /// 保存玩家技能配置
    /// </summary>
    public void SaveSkills()
    {
        string strData = "";
        ISkill[] skills = GameManager.hero.GetComponents<ISkill>();
        for (int i = 0; i < skills.Length; i++)
        {
            ISkill skill = skills[i];
            strData = strData + skill.GetBaseData().id + "_" + skill._Level + "&";
        }

        PlayerPrefs.SetString(IConst.KEY_SKILLS, strData);

        
        
    }

    // 保存未分配的技能点
    public void SaveSP()
    {
        PlayerPrefs.SetInt(IConst.KEY_SKILLPOINT_NEEDALLOT, GameManager.hero._SkillNeedAllot);
    }

    /// <summary>
    /// 保存使用的道具
    /// </summary>
    public void SaveItemUesd() 
    {
        string strData = "";
        for (int i = 0; i < GameManager.hero.arrItemUesd.Length; i++)
        {
            int itemId = GameManager.hero.arrItemUesd[i];
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
                    GameManager.hero.SetItemUsed(i, int.Parse(strs[i]));
                }
            }
        }
    }

    /// <summary>
    /// 读取玩家技能配置
    /// </summary>
    public void ReadAndInitSkills()
    {
        if (PlayerPrefs.HasKey(IConst.KEY_SKILLS))
        {
            string strData = PlayerPrefs.GetString(IConst.KEY_SKILLS);
            string[] strSkills = strData.Split('&');
            for (int i = 0; i < strSkills.Length; i++)
            {
                string strSkillNode = strSkills[i];
                if (!string.IsNullOrEmpty(strSkillNode))
                {
                    string[] strsTemp = strSkillNode.Split('_');
                    int skillId = int.Parse(strsTemp[0]);
                    int level = int.Parse(strsTemp[1]);
                    GameManager.hero.SetBattleSkillLevel(skillId, level);
                }
            }
        }
    }

    /// <summary>
    /// 读取未分配技能点
    /// </summary>
    public void ReadSP()
    {
        if (PlayerPrefs.HasKey(IConst.KEY_SKILLPOINT_NEEDALLOT))
        {
            GameManager.hero._SkillNeedAllot = PlayerPrefs.GetInt(IConst.KEY_SKILLPOINT_NEEDALLOT);
        }
    }

    /// <summary>
    /// 保存任务进度
    /// </summary>
    public void SaveMissionStep()
    {
        PlayerPrefs.SetInt(IConst.KEY_MISSION, GameManager.hero._CurMainMission.id);
    }

    /// <summary>
    /// 读取并设置当前任务进度
    /// </summary>
    public void ReadAndInitMissionStep()
    {
        int curMissionId = 0;
        if (PlayerPrefs.HasKey(IConst.KEY_MISSION))
        {
            curMissionId = PlayerPrefs.GetInt(IConst.KEY_MISSION);
        }
        
        if (curMissionId == 0)
        {
            curMissionId = 1;   
        }

        GameManager.gameView.StartAMission(curMissionId);
    }

    /// <summary>
    /// 保存当前城镇地图
    /// </summary>
    public void SaveCurHomeMap(int mapid)
    {
        PlayerPrefs.SetInt(IConst.KEY_HOMEMAP, mapid);
    }

    /// <summary>
    /// 读取当前城镇地图
    /// </summary>
    /// <returns></returns>
    public int ReadCurHomeMap()
    {
        int curhomemap = 6; // 默认是地图6
        if (PlayerPrefs.HasKey(IConst.KEY_HOMEMAP))
        {
            curhomemap = PlayerPrefs.GetInt(IConst.KEY_HOMEMAP);
        }
        return curhomemap;
    }

    #region Avtaor2D
    Dictionary<string, Sprite> dicSprites = new Dictionary<string, Sprite>();
    public Sprite GetSprite(string name)
    {
        Sprite r = null;
        if (dicSprites.ContainsKey(name))
        {
            r = dicSprites[name];
        }
        return r;
    }

    public void InitSprites() 
    {
        Sprite[] sps = Resources.LoadAll<Sprite>("Texus/Avtoar");
        for (int i = 0; i < sps.Length; i++)
        {
            Sprite sp = sps[i];
            dicSprites.Add(sp.name, sp);
        }
    }

    //public GameObject CreateA2DPlayer(JSONNode jdData)
    //{

    //}

    public void ShankASprite(SpriteRenderer sprite) 
    {
        StartCoroutine(CoShandASprite(sprite));
    }

    IEnumerator CoShandASprite(SpriteRenderer sprite) 
    {
        if (sprite != null)
        {
            float interTime = 0.1f;
            while (true)
            {
                if (sprite == null)
                {
                    break;
                }
                if (sprite.enabled)
                {
                    sprite.enabled = false;
                }
                else
                {
                    sprite.enabled = true;
                }
                yield return new WaitForSeconds(interTime);
            }
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="effName"></param>
    /// <param name="pos"></param>
    /// <param name="color"></param>
    /// <param name="dur">-1:表示仅播放一次。时间使用特效配置的时间</param>
    public void CreateEffect(string effName, Vector3 pos, Color color, float dur) 
    {
        GameObject gobjEff = Tools.LoadResourcesGameObject(IPath.Effects + effName);
        gobjEff.transform.position = pos;
        SpriteEffect se = gobjEff.GetComponent<SpriteEffect>();
        if (se != null)
        {
            se.SetColor(color);
        }
        GObjLife gl = gobjEff.AddComponent<GObjLife>();
        if (dur < 0)
        {
            // 仅播放一次。时间使用特效配置的时间
            dur = se.durTime;
        }
        gl.lifeTime = dur;

    }
#endregion
}
