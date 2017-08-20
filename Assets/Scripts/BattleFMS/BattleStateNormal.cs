public class BattleStateNormal : IBattleState
{
    private ManagerBattleState manager;

    public BattleStateNormal(ManagerBattleState managerBattleState)
    {
        this.manager = managerBattleState;
        _stateType = EBattleState.Normal;
    }

    public override void Update()
    {
        base.Update();
        manager.hero.OnBSUpdateIdle();
    }

    public override IBattleState ActionAtk()
    {
        IBattleState next = null;
        //TODO攻击消耗精力
        if (manager.hero.Prop.Vigor >= 0)
        {
            next = manager.bsAtkBefore;
        }
        else
        {
            UIManager.Inst.ShowFloatTip("精力不足");
        }
        return next;
    }

    public override void Start()
    {
        base.Start();
        manager.hero.OnBSStartNormal();
    }

    public override IBattleState ActionDef()
    {
        return manager.bsDefing;
    }

    public override IBattleState ActionPowerStart()
    {
        IBattleState next = null;
        //TODO蓄力消耗精力
        if (manager.hero.Prop.Vigor >= 0)
        {
            next = manager.bsPowering;
        }
        else
        {
            UIManager.Inst.ShowFloatTip("精力不足");
        }
        return next;
    }

    public override IBattleState ActionSKill(ISkill skill)
    {
        skill.SetCaster(manager.hero);
        if (skill.GetBaseData().targetType != ESkillTargetType.None)
        {
            skill.SetTarget(manager.hero.curTarget);
        }
        if (skill.CheckCast())
        {
            manager.bsSkilling.skill = skill;
            return manager.bsSkilling;
        }
        else
        {
            return null;
        }
    }

    public override IBattleState ActionDodge(float dur)
    {
        IBattleState r = null;
        //闪避消耗
        if (manager.hero.Prop.Vigor >= manager.hero.GetVigorCostDodge())
        {
            manager.bsDodge.dur = dur;
            r = manager.bsDodge;
        }
        else
        {
            UIManager.Inst.ShowFloatTip("精力不足");
        }
        return r;
    }
}