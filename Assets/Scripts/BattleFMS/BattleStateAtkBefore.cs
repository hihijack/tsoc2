public class BattleStateAtkBefore : IBattleState
{
    float durTime;
    private ManagerBattleState managerBattleState;

    public BattleStateAtkBefore(ManagerBattleState managerBattleState)
    {
        this.managerBattleState = managerBattleState;
        this.stateType = EBattleState.AtkBefore;
    }

    public override void Start()
    {
        base.Start();
        managerBattleState.hero.OnBSStartAtkBefore();
        durTime = 0f;
    }

    public override void Update()
    {
        base.Update();
        durTime += UnityEngine.Time.deltaTime;
        if (durTime >= managerBattleState.hero.AtkAnimTimeBefore)
        {
            managerBattleState.ActionAtkBeforeTimeEnd();
        }
    }

    public override IBattleState ActionAtkBeforeTimeEnd()
    {
        return managerBattleState.bsHit;
    }

    public override IBattleState ActionStop()
    {
        return managerBattleState.bsNormal;
    }

    public override IBattleState ActionDef()
    {
        if (managerBattleState.hero._Prop.EnergyPoint > 0)
        {
            managerBattleState.hero._Prop.EnergyPoint--;
            UIManager.Inst.uiMain.RefreshHeroEnergy();
            return managerBattleState.bsDefing;
        }
        else
        {
            return null;
        }
    }

    public override IBattleState ActionSKill(ISkill skill)
    {
        skill.SetCaster(managerBattleState.hero);
        if (skill.GetBaseData().targetType != ESkillTargetType.None)
        {
            skill.SetTarget(managerBattleState.hero.curTarget);
        }
        if (skill.CheckCast())
        {
            managerBattleState.bsSkilling.skill = skill;
            return managerBattleState.bsSkilling;
        }
        else
        {
            return null;
        }

    }
}