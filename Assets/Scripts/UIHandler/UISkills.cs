using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UISkills : MonoBehaviour
{
    public UIGrid gridWQ;
    public UIGrid gridKB;
    public UIGrid gridZD;
    public UIGrid gridYS;
    public UIGrid gridFY;

    public UISprite iconInfo;
    public UILabel txtNameInfo;
    public UILabel txtTipInfo;
    public UILabel txtDescInfo;
    public UILabel txtLevelInfo;
    public UIButton btnUpgradeInfo;
    public UIButton btnDowngradeInfo;

    public GameObject gobjPrefabSkillItem;

    public UILabel txtSPNum;

    Dictionary<int, object> dicModelViews = new Dictionary<int, object>();

    SkillBD curSelectSkill;
    int levelCurCurSelect;
    GameObject gobjCurSelect;

    public void Init()
    {
        List<SkillBD> sklils = GameDatas.GetAllBattleSkills();
        sklils.Sort(new ComparSkillBDByLevel());
        for (int i = 0; i < sklils.Count; i++)
        {
            SkillBD skill = sklils[i];
            int curlevel = GameManager.hero.GetSkillLevel(skill.id);
            GameObject gobjGrid = null;
            switch (skill.branch)
            {
                case ESkillBranch.Wq:
                    gobjGrid = gridWQ.gameObject;
                    break;
                case ESkillBranch.Zd:
                    gobjGrid = gridZD.gameObject;
                    break;
                case ESkillBranch.Kb:
                    gobjGrid = gridKB.gameObject;
                    break;
                case ESkillBranch.Ys:
                    gobjGrid = gridYS.gameObject;
                    break;
                case ESkillBranch.Fy:
                    gobjGrid = gridFY.gameObject;
                    break;
                default:
                    break;
            }
            GameObject gobjItemSkill = NGUITools.AddChild(gobjGrid, gobjPrefabSkillItem);
            // 图标
            UISprite icon = Tools.GetComponentInChildByPath<UISprite>(gobjItemSkill, "icon");
            icon.spriteName = skill.iconName;
            // 名字
            UILabel txtName = Tools.GetComponentInChildByPath<UILabel>(gobjItemSkill, "txt_name");
            txtName.text = skill.name;
            // 等级
            UILabel txtLevel = Tools.GetComponentInChildByPath<UILabel>(gobjItemSkill, "txt_level");
            if (curlevel > 0)
            {
                txtLevel.text = curlevel.ToString();
            }
            else
            {
                txtLevel.gameObject.SetActive(false);
            }
            // 点击监听
            UIButton btnItemSkill = gobjItemSkill.GetComponent<UIButton>();
            btnItemSkill.onClick.Add(new EventDelegate(OnBtn_ItemSkill));
            // 数据绑定
            btnItemSkill.data = skill;
            btnItemSkill.data2 = curlevel;

            if (i == 0)
            {
                btnItemSkill.Click();
            }
        }
        gridWQ.Reposition();
        gridZD.Reposition();
        gridKB.Reposition();
        gridYS.Reposition();
        gridFY.Reposition();

        // 可分配技能点
        Refresh_SkillPoint();
    }

    void Refresh_SkillPoint()
    {
        txtSPNum.text = GameManager.hero._SkillNeedAllot.ToString();
    }

    void OnBtn_ItemSkill()
    {
        // 显示详细信息
        SkillBD skillBD = UIButton.current.data as SkillBD;
        int curLevel = (int)UIButton.current.data2;
        curSelectSkill = skillBD;
        levelCurCurSelect = curLevel;
        gobjCurSelect = UIButton.current.gameObject;
        // 图标
        iconInfo.spriteName = skillBD.iconName;
        // 名字
        txtNameInfo.text = skillBD.name;
        // 学习提示
        if (GameManager.hero.level < skillBD.needLevel)
        {
            txtTipInfo.gameObject.SetActive(true);
            txtTipInfo.text = "需要" + skillBD.needLevel + "级";
        }
        else
        {
            txtTipInfo.gameObject.SetActive(false);
        }
        // 描述
        txtDescInfo.text = skillBD.ToDesc(curLevel == 0 ? 1 : curLevel);
        // 当前等级
        txtLevelInfo.text = curLevel.ToString();
        // 等级设置按键监听
        if (btnDowngradeInfo.onClick.Count == 0)
        {
            btnDowngradeInfo.onClick.Add(new EventDelegate(OnBtn_Downgrade));
        }

        if (btnUpgradeInfo.onClick.Count == 0)
        {
            btnUpgradeInfo.onClick.Add(new EventDelegate(OnBtn_Upgrade));
        }

        CheckBtnUpDowngradeEnable();
    }

    /// <summary>
    /// 升级技能
    /// </summary>
    void OnBtn_Upgrade()
    {
        if (GameManager.hero.level >= curSelectSkill.needLevel)
        {
            levelCurCurSelect++;
            GameManager.hero.SetBattleSkillLevel(curSelectSkill.id, levelCurCurSelect);
            GameManager.hero._SkillNeedAllot--;

            UIButton btnCurSelect = gobjCurSelect.GetComponent<UIButton>();
            btnCurSelect.data2 = levelCurCurSelect;

            // 刷新界面
            RefreshUIOnChangeLevel();

            CheckBtnUpDowngradeEnable();

            GameManager.commonCPU.SaveSkills();
        }
        else
        {
            UIManager.Inst.GeneralTip("等级不足", Color.red);
        }
        
    }

    void OnBtn_Downgrade()
    {
        levelCurCurSelect--;
        GameManager.hero.SetBattleSkillLevel(curSelectSkill.id, levelCurCurSelect);
        GameManager.hero._SkillNeedAllot++;

        UIButton btnCurSelect = gobjCurSelect.GetComponent<UIButton>();
        btnCurSelect.data2 = levelCurCurSelect;

        // 刷新界面
        RefreshUIOnChangeLevel();

        CheckBtnUpDowngradeEnable();

        GameManager.commonCPU.SaveSkills();
    }

    void RefreshUIOnChangeLevel()
    {
        // 技能点
        Refresh_SkillPoint();
        // 技能等级
        UILabel txtLevel = Tools.GetComponentInChildByPath<UILabel>(gobjCurSelect, "txt_level");
        if (levelCurCurSelect > 0)
        {
            txtLevel.gameObject.SetActive(true);
            txtLevel.text = levelCurCurSelect.ToString();
        }
        else
        {
            txtLevel.gameObject.SetActive(false);
        }

        // 详情界面技能等级
        txtLevelInfo.text = levelCurCurSelect.ToString();
        // 详情界面技能描述
        // 当前等级为0时也显示1级描述
        txtDescInfo.text = curSelectSkill.ToDesc(levelCurCurSelect == 0 ? 1 : levelCurCurSelect);
    }

    void CheckBtnUpDowngradeEnable()
    {
        if (GameManager.hero._SkillNeedAllot <= 0 || levelCurCurSelect >= curSelectSkill.lvUplimit)
        {
            btnUpgradeInfo.enabled = false;
        }
        else
        {
            btnUpgradeInfo.enabled = true;
        }


        if (levelCurCurSelect <= 0)
        {
            btnDowngradeInfo.enabled = false;
        }
        else
        {
            btnDowngradeInfo.enabled = true;
        }
    }

    void BindData(int id, object obj)
    {
        if (dicModelViews.ContainsKey(id))
        {
            dicModelViews[id] = obj;
        }
        else
        {
            dicModelViews.Add(id, obj);
        }
    }
}
