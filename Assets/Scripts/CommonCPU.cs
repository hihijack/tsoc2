using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using SimpleJSON;

public class CommonCPU : MonoBehaviour
{
    public static CommonCPU Inst;

    void Awake()
    {
        Inst = this;
    }

    /// <summary>
    /// 保存玩家技能配置
    /// </summary>
    public void SaveSkills()
    {
        string strData = "";
        for (int i = 0; i < Hero.Inst.mSkills.Length; i++)
        {
            ISkill skill = Hero.Inst.mSkills[i];
            if (skill != null)
            {
                strData = strData + skill.GetBaseData().id + "_" + skill._Level + "&";
            }
            else
            {
                strData = strData + "0_0&";
            }
        }

        PlayerPrefs.SetString(IConst.KEY_SKILLS, strData);
    }

    // 保存未分配的技能点
    public void SaveSP()
    {
        PlayerPrefs.SetInt(IConst.KEY_SKILLPOINT_NEEDALLOT, GameManager.hero._SkillNeedAllot);
    }

    public void SaveSkillUnlock(int skillid)
    {
        string skills = GetSavedSkillUnlock();
        skills = skills + skillid + "&";
        PlayerPrefs.SetString(IConst.KEY_SKILL_UNLOCKED, skills);
    }

    public string GetSavedSkillUnlock()
    {
        string r = "";
        if (PlayerPrefs.HasKey(IConst.KEY_SKILL_UNLOCKED))
        {
            r = PlayerPrefs.GetString(IConst.KEY_SKILL_UNLOCKED);
        }
        return r;
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
                    GameManager.hero.SetBattleSkillLevel(skillId, level, i);
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
        int curhomemap = 1; // 默认是地图6
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
        sps = Resources.LoadAll<Sprite>("Texus/TiledMap");
        for (int i = 0; i < sps.Length; i++)
        {
            Sprite sp = sps[i];
            dicSprites.Add(sp.name, sp);
        }
        sps = Resources.LoadAll<Sprite>("Texus/Items");
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
    public void CreateEffect(string effName, Vector3 pos, Color color, float dur, bool comstColor = true) 
    {
        GameObject gobjEff = Tools.LoadResourcesGameObject(IPath.Effects + effName);
        gobjEff.name = effName;
        gobjEff.transform.position = pos;
        SpriteEffect se = gobjEff.GetComponent<SpriteEffect>();
        if (se != null && comstColor)
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

    public void PlayerAudio(string res)
    {
        if (!string.IsNullOrEmpty(res))
        {
            NGUITools.PlaySound(Resources.Load<AudioClip>(IPath.Audio + res));
        }
    }

    public void CreateEffAtGobj(string effName, GameObject target, Vector3 offset, Color color, float dur)
    {
        GameObject gobjEff = Tools.LoadResourcesGameObject(IPath.Effects + effName);
        gobjEff.name = effName;
        gobjEff.transform.parent = target.transform;
        gobjEff.transform.localPosition = offset;
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

    public void RemoveEff(GameObject gobjEff)
    {
        if (gobjEff != null)
        {
            DestroyObject(gobjEff);
        }
    }

    public bool ContainDirs(EDirection[] dirs, EDirection dir)
    {
        bool contain = false;
        for (int i = 0; i < dirs.Length; i++)
        {
            if (dirs[i] == dir)
            {
                contain = true;
                break;
            }
        }
        return contain;
    }
#endregion
}
