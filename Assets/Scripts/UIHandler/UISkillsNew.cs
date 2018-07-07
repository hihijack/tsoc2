using UnityEngine;
using System.Collections;
using System;

public class UISkillsNew : MonoBehaviour
{
    public UIVisibleCtl vCtl;
    public UIItemSkill[] itemSkills;
    public GameObject pfbItemSkill;
    public UIGrid grid;
    public UISprite txuSelect;

    int curSelectIndex;
    public static bool needRefresh = true;

    public void Init()
    {
        //已分配技能
        ISkill[] skills = Hero.Inst.mSkills;
        for (int i = 0; i < skills.Length; i++)
        {
            if (i >= itemSkills.Length)
            {
                break;
            }

            ISkill skill = skills[i];
            if (skill != null)
            {
                itemSkills[i].data = skill.GetBaseData();
            }
            else
            {
                itemSkills[i].data = null;
            }
            itemSkills[i].Refresh();
            UIButton btn = itemSkills[i].GetComponent<UIButton>();
            btn.onClick.Add(new EventDelegate(OnClickMineSkill));
        }
        //已解锁技能
        //先添加一个卸下的控技能
        AddASkillItem(null);
        string strData = CommonCPU.Inst.GetSavedSkillUnlock();
        string[] strSkills = strData.Split('&');
        for (int i = 0; i < strSkills.Length; i++)
        {
            string strSkillNode = strSkills[i];
            if (!string.IsNullOrEmpty(strSkillNode))
            {
                int skillId = int.Parse(strSkillNode);
                SkillBD skillBD = GameDatas.GetSkillBD(skillId);
                AddASkillItem(skillBD);
            }
        }
        grid.Reposition();

        txuSelect.alpha = 0f;
        curSelectIndex = -1;
    }

    

    void AddASkillItem(SkillBD skillBD)
    {
        GameObject gobjItem = NGUITools.AddChild(grid.gameObject, pfbItemSkill);
        UIItemSkill uiItemsSkill = gobjItem.GetComponent<UIItemSkill>();
        uiItemsSkill.data = skillBD;
        uiItemsSkill.RefreshForOther();
        UIButton btn = uiItemsSkill.GetComponent<UIButton>();
        btn.onClick.Add(new EventDelegate(OnClickASkillItem));
    }

    /// <summary>
    /// 点击我的技能
    /// </summary>
    private void OnClickMineSkill()
    {
        string name = UIButton.current.name;
        curSelectIndex = int.Parse(name);
        txuSelect.alpha = 1f;
        txuSelect.transform.position = UIButton.current.transform.position;
    }

    /// <summary>
    /// 点击一个技能
    /// </summary>
    private void OnClickASkillItem()
    {
        if (curSelectIndex >= 0)
        {
            UIItemSkill targetSkill = UIButton.current.GetComponent<UIItemSkill>();
            ISkill curSkill = Hero.Inst.mSkills[curSelectIndex];
            if ((targetSkill.data == null && curSkill == null) 
                ||
                (targetSkill.data != null && curSkill != null && targetSkill.data.id == curSkill.GetBaseData().id) 
                )
            {
                //相同
                return;
            }

            //旧技能卸下
            if (curSkill != null)
            {
                Hero.Inst.SetBattleSkillLevel(curSkill.GetBaseData().id, 0);
                itemSkills[curSelectIndex].data = null;
                itemSkills[curSelectIndex].Refresh();
            }

            //装上目标技能
            if (targetSkill.data != null)
            {
                int indexOri = Hero.Inst.GetSkillHasAllotIndex(targetSkill.data.id);
                if (indexOri >= 0)
                {
                    //目标已经被分配，分配到当前，原先槽变成为分配
                    //卸下原先槽
                    Hero.Inst.mSkills[curSelectIndex] = Hero.Inst.mSkills[indexOri];
                    Hero.Inst.mSkills[indexOri] = null;

                    itemSkills[indexOri].data = null;
                    itemSkills[indexOri].Refresh();
                    itemSkills[curSelectIndex].data = targetSkill.data;
                    itemSkills[curSelectIndex].Refresh();
                }
                else
                {
                    Hero.Inst.SetBattleSkillLevel(targetSkill.data.id, 1, curSelectIndex);
                    itemSkills[curSelectIndex].data = targetSkill.data;
                    itemSkills[curSelectIndex].Refresh();
                }

               
            }
            else
            {
                //卸下技能
                Hero.Inst.SetBattleSkillLevel(curSkill.GetBaseData().id, 0);
                itemSkills[curSelectIndex].data = null;
                itemSkills[curSelectIndex].Refresh();
            }

            CommonCPU.Inst.SaveSkills();
        }
    }

    public void RefreshUnlockASkill(SkillBD skillBD)
    {
        AddASkillItem(skillBD);
        grid.Reposition();
    }

    public void InvokeClose()
    {
        UIManager.Inst.CloseUISkill();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
