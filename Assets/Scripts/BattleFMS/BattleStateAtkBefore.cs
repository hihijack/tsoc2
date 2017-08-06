public class BattleStateAtkBefore : IBattleState
{
    float durTime;
    private ManagerBattleState manager;

    public BattleStateAtkBefore(ManagerBattleState managerBattleState)
    {
        this.manager = managerBattleState;
        this._stateType = EBattleState.AtkBefore;
    }

    public override void Start()
    {
        base.Start();
        manager.hero.OnBSStartAtkBefore();
        durTime = 0f;
    }

    public override void Update()
    {
        base.Update();
        durTime += UnityEngine.Time.deltaTime;
        if (durTime >= manager.hero.AtkAnimTimeBefore)
        {
            manager.ActionAtkBeforeTimeEnd();
        }
    }

    public override IBattleState ActionAtkBeforeTimeEnd()
    {
        return manager.bsHit;
    }

    public override IBattleState ActionStop()
    {
        return manager.bsNormal;
    }

    public override IBattleState ActionDef()
    {
        return manager.bsDefing;
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
        //TODO 闪避消耗
        if (manager.hero.Prop.Vigor >= 10)
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