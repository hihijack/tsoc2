public class BattleStateNormal : IBattleState
{
    private ManagerBattleState managerBattleState;

    public BattleStateNormal(ManagerBattleState managerBattleState)
    {
        this.managerBattleState = managerBattleState;
        stateType = EBattleState.Normal;
    }

    public override void Update()
    {
        base.Update();
    }

    public override IBattleState ActionAtk()
    {
        IBattleState next = null;
        next = managerBattleState.bsAtkBefore;
        return next;
    }

    public override void Start()
    {
        base.Start();
        managerBattleState.hero.OnBSStartNormal();
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

    public override IBattleState ActionPowerStart()
    {
        return managerBattleState.bsPowering;
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