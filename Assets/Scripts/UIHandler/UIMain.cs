using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIMain : MonoBehaviour {

    public UISprite heroHeadIcon;
    public UISlider heroHP;
    public UILabel txtHeroHp;
    public UISlider heroMP;
    public UILabel txtHeroMP;
    public UISlider heroTL;
    public UILabel txtHeroTL;

    public GameObject[] gobjTargets;
    //public UISprite targetHeadIcon;
    public UISlider[] targetHPs;
    public UILabel[] txtTargetHPs;

    public GameObject gobjBattle;

    public UISlider expCurLevel;
    public UILabel txtExp;

    GameView gameView;

    public GameObject targetMask;

    public GameObject gobjGridSkills;

    public Dictionary<int, object> dicModelViews = new Dictionary<int, object>();

    bool hasInitSkill = false;

    public UIGrid gridBuffs;
    public GameObject gobjPrafabBuffItem;


    public GameObject gobjSkillTractics;
    public GameObject gobjGridItemUsed;

    public UIBattle uiBattle;

    public UIProgressBar progAtk;

    AtkBar atkBar;

    public UIChooseTarget gUIChooseTarget;

    public AtkBar _AtkBar
    {
        get 
        {
            if (atkBar == null)
            {
                atkBar = progAtk.GetComponent<AtkBar>();
            }

            return atkBar;
        }
    }

    public GameObject gobjUIBtns;
    public UIButton btnChara;
    public UIButton btnBag;
    public UIButton btnMission;
    public UIButton btnSkill;
    public UIButton btnWait;

    public UILabel txtMapName;

    public UILabel txtCurRound;

    public GameObject energyPoints;

    public void Init(GameView gameView)
    {
        this.gameView = gameView;
        gobjTargets[0].SetActive(false);
        gobjTargets[1].SetActive(false);
        gobjTargets[2].SetActive(false);
        gobjTargets[3].SetActive(false);
        SetUIBattleVisble(false);
        targetMask.SetActive(false);

        gUIChooseTarget.SetVisible(false);
        //InitUISkillTracics();

        InitItemUsed();

        // 初始化按钮
        btnChara.onClick.Add(new EventDelegate(OnBtn_Character));
        btnBag.onClick.Add(new EventDelegate(OnBtn_Bag));
        btnMission.onClick.Add(new EventDelegate(OnBtn_Mission));
        btnSkill.onClick.Add(new EventDelegate(OnBtn_ShowSkill));
        btnWait.onClick.Add(new EventDelegate(OnBtn_Wait));
    }

    public void SetUIBtnsShow(bool isshow) 
    {
        gobjUIBtns.SetActive(isshow);
    }

    /// <summary>
    /// 按钮点击-技能
    /// </summary>
    void OnBtn_ShowSkill() 
    {
        UIManager.Inst.ToggleUI_Skill();
    }

    /// <summary>
    /// 按钮 - 等待
    /// </summary>
    private void OnBtn_Wait()
    {
        GameView._Inst.RoundLogicWaitARound();
    }

    /// <summary>
    /// 按钮点击-任务
    /// </summary>
    void OnBtn_Mission() 
    {
        UIManager.Inst.ToggleUIMission();
    }

    /// <summary>
    /// 点击按钮-背包
    /// </summary>
    void OnBtn_Bag() 
    {
        UIManager.Inst.ToggleUI_Bag();
    }

    /// <summary>
    /// 点击按钮-角色查看
    /// </summary>
    void OnBtn_Character() 
    {
        UIManager.Inst.ToggleUI_HeroInfo();
    }

    /// <summary>
    /// 初始化战术技能UI
    /// </summary>
    void InitUISkillTracics()
    {
        gobjSkillTractics.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            ISkill skill = null;
            if (i < gameView._MHero.skillsTractics.Length)
            {
                skill = gameView._MHero.skillsTractics[i];
            }
            GameObject gobjSpell = Tools.GetGameObjectInChildByPathSimple(gobjGridItemUsed, "spell" + i);
            if (gobjSpell != null)
            {
                UISprite us = gobjSpell.GetComponent<UISprite>();
                if (skill != null)
                {
                    us.spriteName = skill.GetBaseData().iconName;
                    UIButton btn = gobjSpell.GetComponent<UIButton>();
                    btn.onClick.Clear();
                    btn.onClick.Add(new EventDelegate(Btn_SkillTractics));
                    btn.data = skill;
                    // 数据绑定
                    BindData(skill.GetInstanceID(), gobjSpell);
                    BindData(gobjSpell.GetInstanceID(), skill);
                }
                else
                {
                    gobjSpell.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// 初始化道具使用
    /// </summary>
    void InitItemUsed() 
    {
        for (int i = 0; i < GameManager.hero.arrItemUesd.Length; i++)
        {
            GameObject gobjSpell = Tools.GetGameObjectInChildByPathSimple(gobjGridItemUsed, "item" + i);
            ItemUesdGrid iug = gobjSpell.GetComponent<ItemUesdGrid>();
            int idItemUsed = GameManager.hero.arrItemUesd[i];
            if (idItemUsed > 0)
            {
                EquipItem ei = GameManager.hero.GetEquipItemInBagById(idItemUsed);
                iug.SetEquipItem(ei);
                iug.RefershUI();
            }
            else
            {
                iug.RefershUI();
            }
        }   
    }

    public void RefreshItemUsed(EquipItem ei) 
    {
        foreach (Transform tfChild in gobjGridItemUsed.transform)
        {
            ItemUesdGrid iug = tfChild.GetComponent<ItemUesdGrid>();
            EquipItem eiSettd = iug.GetEquipItem();
            if (eiSettd != null && eiSettd.id == ei.id)
            {
                iug.RefershUI();
            }
        }
    }

    public void ClearItemUsed(EquipItem ei) 
    {
        foreach (Transform tfChild in gobjGridItemUsed.transform)
        {
            ItemUesdGrid iug = tfChild.GetComponent<ItemUesdGrid>();
            EquipItem eiSettd = iug.GetEquipItem();
            if (eiSettd != null && eiSettd.id == ei.id)
            {
                iug.SetEquipItem(null);
                iug.RefershUI();
            }
        }
    }

    /// <summary>
    /// 点击战术技能
    /// </summary>
    void Btn_SkillTractics()
    {
        ISkill skill = UIButton.current.data as ISkill;
        if (gameView._RoundLogicState == GameRoundLogicState.Normal)
        {
            gameView.StartSkillTractics(skill);
        }
    }

    void SetUIBattleVisble(bool visble)
    {
        if (visble)
        {
            gobjBattle.transform.localPosition = Vector3.zero;
            uiBattle.OnShow();
        }
        else
        {
            gobjBattle.transform.localPosition = new Vector3(0f, -5000f, 0f);
        }
    }

    public void RefreshExp(int curVal, int needVal)
    {
        expCurLevel.value = curVal * 1.0f / needVal;
        txtExp.text = curVal + "/" + needVal;
    }

    public void RefreshHeroHP()
    {
        // hp
        int curHp = gameView._MHero.Prop.Hp;
        int maxHp = gameView._MHero.Prop.HpMax;
        float hpVal = (float)curHp / maxHp;
        heroHP.value = hpVal;
        txtHeroHp.text = curHp + "/" + maxHp;
    }

    public void RefreshHeroTL()
    {
        // 体力
        //int curTL = gameView._MHero.tl;
        //int maxTL = gameView._MHero.tlMax;
        //float tlVal = (float)curTL / maxTL;
        //heroTL.value = tlVal;
        //txtHeroTL.text = curTL + "/" + maxTL;
    }

    public void RefreshHeroVigor()
    {
        // 魔法值
        float curMP = Hero.Inst.Prop.Vigor;
        int maxMP = Hero.Inst.Prop.VigorMax;
        float mpVal = curMP / maxMP;
        heroMP.value = mpVal;
        txtHeroMP.text = curMP.ToString("0") + "/" + maxMP;

        //RefreshHeroEnergy();
    }

    //public void RefreshHeroEnergy()
    //{
    //    //能力点
    //    for (int i = 0; i < 5; i++)
    //    {
    //        UISprite icon = Tools.GetComponentInChildByPath<UISprite>(energyPoints, i.ToString());
    //        if (i < gameView._MHero._Prop.EnergyPoint)
    //        {
    //            icon.alpha = 1f;
    //        }
    //        else
    //        {
    //            icon.alpha = 0f;
    //        }
    //    }
    //}

    public void RefreshTargetHP(Enermy target)
    {
        targetHPs[target.uiIndex].value = (float)target.Prop.Hp / target.Prop.HpMax;
        txtTargetHPs[target.uiIndex].text = target.Prop.Hp + "/" + target.Prop.HpMax;
    }

    // TODO ShowTargetUI
    public void ShowTargetUI(bool isshow)
    {
        //gobjTargets[target.uiIndex].SetActive(isshow);
    }

    public void ShowUIBattle(bool isshow)
    {
        SetUIBattleVisble(isshow);

        gobjSkillTractics.SetActive(!isshow);
        SetUIBtnsShow(!isshow);

        if (isshow)
        {
            //if (!hasInitSkill)
            {
                for (int j = 0; j < 4; j++)
                {
                    GameObject gobjSkill = Tools.GetGameObjectInChildByPathSimple(gobjGridSkills, "spell" + j);
                    UISprite spriteSkill = gobjSkill.GetComponent<UISprite>();
                    spriteSkill.spriteName = "btn_bg";
                    RemoveBind(gobjSkill.GetInstanceID());
                }
                ISkill[] skills = gameView._MHero.mSkills;
                int index = 0;
                for (int i = 0; i < skills.Length; i++)
                {
                    ISkill skill = skills[i];
                    if (skill != null && skill.GetBaseData().type == ESkillType.Battle)
                    {
                        GameObject gobjSkill = Tools.GetGameObjectInChildByPathSimple(gobjGridSkills, "spell" + index);
                        index++;
                        UISprite spriteSkill = gobjSkill.GetComponent<UISprite>();

                        if (skill != null)
                        {
                            spriteSkill.spriteName = skill.GetBaseData().iconName;
                            UIButton btnSkill = gobjSkill.GetComponent<UIButton>();
                            btnSkill.onClick.Clear();
                            btnSkill.onClick.Add(new EventDelegate(OnBtn_Skill));
                            btnSkill.data = skill;
                            // 数据绑定
                            BindData(skill.GetInstanceID(), gobjSkill);
                            BindData(gobjSkill.GetInstanceID(), skill);
                        }
                    }
                }
                //hasInitSkill = true;
            }
        }
        else
        {
            for (int i = 0; i < gobjTargets.Length; i++)
            {
                gobjTargets[i].SetActive(false);
            }
        }
    }

    void OnBtn_Skill()
    {
        ISkill skill = UIButton.current.data as ISkill;
        skill.SetCaster(gameView._MHero);
        if (skill.GetBaseData().targetType != ESkillTargetType.None)
        {
            skill.SetTarget(gameView._MHero.curTarget);
        }
        StartCoroutine(skill.Act());
    }

    public void StartSkillCD(ISkill skill, float cdTime)
    {
        // 找到技能对应图标
        GameObject gobjSkill = null;
        if (dicModelViews.ContainsKey(skill.GetInstanceID()))
        {
            gobjSkill = dicModelViews[skill.GetInstanceID()] as GameObject;
        }
        
        // 刷新动画
        if (gobjSkill != null)
        {
            UISprite spriteCD = Tools.GetComponentInChildByPath<UISprite>(gobjSkill, "cd");
            UITweenSpriteFill utsf = spriteCD.GetComponent<UITweenSpriteFill>();

            if (utsf == null)
            {
                utsf = spriteCD.gameObject.AddComponent<UITweenSpriteFill>();
            }
            utsf.ResetToBeginning();
            utsf.from = 1f;
            utsf.to = 0f;
            utsf.duration = skill.GetBaseData().cd;
            utsf.PlayForward();
        }

    }

    public void InitTargetUI(List<Enermy> enermys)
    {
        for (int i = 0; i < enermys.Count; i++)
        {
            Enermy enermyItem = enermys[i];
            GameObject gobjUIEnermyItem = gobjTargets[enermyItem.uiIndex];
            gobjUIEnermyItem.SetActive(true);
            UILabel txtname = Tools.GetComponentInChildByPath<UILabel>(gobjUIEnermyItem, "txt_name");
            txtname.text = "LV" + enermyItem._MonsterBD.level + enermyItem._MonsterBD.name;
            RefreshTargetHP(enermyItem);
            UIButton btn = gobjUIEnermyItem.GetComponent<UIButton>();
            btn.onClick.Add(new EventDelegate(OnBtn_EnermyTarget));
            btn.data = enermyItem;

            // 移除buff UI
            GameObject gobjBuffs = Tools.GetGameObjectInChildByPathSimple(gobjUIEnermyItem, "buffs");
            foreach (Transform tfChild in gobjBuffs.transform)
            {
                DestroyImmediate(tfChild.gameObject);
            }

            // 数据绑定
            BindData(enermyItem.GetInstanceID(), gobjUIEnermyItem);
            BindData(gobjUIEnermyItem.GetInstanceID(), enermyItem);
        }
    }

    void RemoveBind(int id) 
    {
        if (dicModelViews.ContainsKey(id))
        {
            dicModelViews.Remove(id);
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

    void OnBtn_EnermyTarget()
    {
        Enermy enermy = UIButton.current.data as Enermy;
        if (enermy._State != EActorState.Dead)
        {
            gameView._MHero.SetAttackTarget(enermy);
        }
    }

    public void SetTarget(Enermy target)
    {
        targetMask.SetActive(true);
        targetMask.transform.parent = gobjTargets[target.uiIndex].transform;
        targetMask.transform.localPosition = Vector3.zero;
    }

    public void AddABuffToTarget(IActor actor, IBaseBuff buff)
    {
        if (actor.isHero)
        {
            GameObject gobjBuffItem = NGUITools.AddChild(gridBuffs.gameObject, gobjPrafabBuffItem);
            // icon
            UISprite icon = gobjBuffItem.GetComponent<UISprite>();
            icon.spriteName = buff.baseData.icon;
            gridBuffs.Reposition();
            // 数据绑定
            BindData(buff.GetInstanceID(), gobjBuffItem);
            BindData(gobjBuffItem.GetInstanceID(), buff);
        }
        else
        {
            GameObject gobjUIEnermyItem = dicModelViews[actor.GetInstanceID()] as GameObject;
            if (gobjUIEnermyItem != null)
            {
                UIGrid gridBuffsEnermy = Tools.GetComponentInChildByPath<UIGrid>(gobjUIEnermyItem, "buffs");
                GameObject gobjBuffItem = NGUITools.AddChild(gridBuffsEnermy.gameObject, gobjPrafabBuffItem);
                // icon
                UISprite icon = gobjBuffItem.GetComponent<UISprite>();
                icon.spriteName = buff.baseData.icon;
                gridBuffsEnermy.Reposition();
                // 数据绑定
                BindData(buff.GetInstanceID(), gobjBuffItem);
                BindData(gobjBuffItem.GetInstanceID(), buff);
            }
        }
    }

    public void RemoveABuff(IActor actor, IBaseBuff buff)
    {
        if (actor.isHero)
        {
            GameObject gobjUIBuffItem = dicModelViews[buff.GetInstanceID()] as GameObject;
            if (gobjUIBuffItem != null)
            {
                gobjUIBuffItem.SetActive(false);
                gridBuffs.Reposition();
                DestroyObject(gobjUIBuffItem);
            }
        }
        else
        {
            GameObject gobjUIBuffItem = dicModelViews[buff.GetInstanceID()] as GameObject;
            if (gobjUIBuffItem != null)
            {
                GameObject gobjUIEnermyItem = dicModelViews[actor.GetInstanceID()] as GameObject;
                UIGrid gridBuffsEnermy = Tools.GetComponentInChildByPath<UIGrid>(gobjUIEnermyItem, "buffs");
                gobjUIBuffItem.SetActive(false);
                gridBuffsEnermy.Reposition();
                DestroyObject(gobjUIBuffItem);
            }
        }
    }

    public GameObject GetGobjOfASpell(ISkill skill) 
    {
        return dicModelViews[skill.GetInstanceID()] as GameObject;
    }

    public void ShowMapName(string mapName) 
    {
        txtMapName.text = mapName;
    }

    public void ShowRound(int val) 
    {
        txtCurRound.text = val.ToString();
    }
}
